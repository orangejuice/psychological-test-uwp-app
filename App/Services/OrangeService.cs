using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using App.Helpers;
using App.Models;
using CommonServiceLocator;
using Windows.Security.Credentials;

namespace App.Services
{

    public class OrangeService
    {

        // Private class instance
        private static OrangeService _instance;

        public static readonly string Domain = "127.0.0.1";
        public static readonly string BaseHost = "http://" + Domain + ":8000/";

        private Token _Token;
        private User _currentUser;

        private NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        /// <summary>
        /// Get the current service
        /// </summary>
        public static OrangeService Current => _instance ?? (_instance = new OrangeService());

        /// <summary>
        /// Disconnects account
        /// </summary>
        public void LogoutService()
        {
            // Get the password vault
            var vault = new PasswordVault();
            // Remove the token
            _Token = null;
            // Remove everything in the vault
            vault.FindAllByResource("pshy").ToList().ForEach(x => vault.Remove(x));
        }

        /// <summary>
        /// Checks to see if the users soundcloud account is connected
        /// </summary>
        public bool IsAccountConnected
        {
            get
            {
                // Get the password vault
                var vault = new PasswordVault();

                if (vault.RetrieveAll().FirstOrDefault(x => x.Resource == "pshy") == null)
                    return false;

                try
                {
                    // Get recources
                    var resourece = vault.FindAllByResource("pshy").FirstOrDefault();

                    // If this resource does not exist, return false
                    if (resourece == null)
                        return false;

                    // Get the token
                    var tokenValue = vault.Retrieve("pshy", "Token").Password;

                    // There are logined user
                    var cookie = new Windows.Web.Http.HttpCookie("Authorization", Domain, "/");
                    cookie.Value = "Token " + tokenValue;

                    var filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
                    Windows.Web.Http.HttpCookieManager cookieManager = filter.CookieManager;
                    cookieManager.SetCookie(cookie, false);

                    // return true if the token exists
                    return !string.IsNullOrEmpty(tokenValue);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Get the token needed to access soundcloud resources
        /// </summary>
        public Token Token
        {
            get
            {
                // If we already have the token, return it
                if (_Token != null)
                    return _Token;

                // Perform a check to see if we are logged in
                if (!IsAccountConnected)
                    return null;

                // Get the password vault
                var vault = new PasswordVault();

                try
                {
                    // Get recources
                    var resource = vault.FindAllByResource("pshy").FirstOrDefault();

                    // If this resource does not exist, return false
                    if (resource == null)
                        return null;

                    // Get the vault items
                    var token = vault.Retrieve("pshy", "Token").Password;

                    // Create a new token class
                    var tokenHolder = new Token
                    {
                        key = token,
                    };

                    // Set the private token
                    _Token = tokenHolder;

                    // Return the newly created token
                    return tokenHolder;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        private NameValueCollection _credentials;
        /// <summary>
        /// Get the token needed to access soundcloud resources
        /// </summary>
        public NameValueCollection Credentials
        {
            get
            {
                // If we already have the token, return it
                if (_credentials != null)
                    return _credentials;

                // Perform a check to see if we are logged in
                if (!IsAccountConnected)
                    return null;

                // Get the password vault
                var vault = new PasswordVault();

                try
                {
                    // Get recources
                    var resource = vault.FindAllByResource("pshy").FirstOrDefault();

                    // If this resource does not exist, return false
                    if (resource == null)
                        return null;

                    // Get the vault items
                    var username = vault.Retrieve("pshy", "username").Password;
                    var password = vault.Retrieve("pshy", "password").Password;

                    // Create a new token class
                    var credentialsHolder = new NameValueCollection
                    {
                        { "username", username},
                        { "password", password}
                    };

                    // Set the private token
                    _credentials = credentialsHolder;

                    // Return the newly created token
                    return credentialsHolder;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// The current logged in user.
        /// </summary>
        public User CurrentUser
        {
            get
            {
                // Handle account disconnect
                if (!IsAccountConnected)
                {
                    _currentUser = null;
                    return null;
                }

                // If we have a user object, return it
                //if (_currentUser != null)
                //    return _currentUser;

                var request = new HttpRequestMessage(HttpMethod.Get, "/api/users/me/");

                var result = AsyncHelper.RunSync(async () => await SendRequestAsync<User>(request));
                if (result.Success)
                {
                    _currentUser = result.Data;

                    return _currentUser;
                }
                else
                {
                    return null;
                }

            }
        }

        #region restclient version

        //var client = new RestClient()
        //{
        //    Proxy = new WebProxy("127.0.0.1", 8888),
        //    //Proxy = WebRequest.GetSystemWebProxy(),
        //    BaseUrl = new Uri(BaseUrl)
        //};

        //request.AddHeader("Authorization", "Token " + Token.key);
        //var response = await client.ExecuteTaskAsync<T>(request);
        //var result = new RequestResult<T>();

        //if (response.ErrorException != null)
        //{
        //    result.Success = false;
        //    string message = "Error retrieving response.".GetLocalized();
        //    result.Message = response.ErrorMessage;
        //    result.StatusCode = (int)response.StatusCode;
        //    return result;
        //}
        //Debug.WriteLine(response.Data);
        //result.Data = response.Data;
        //return result;

        #endregion

        public async Task<RequestResult<T>> SendRequestAsync<T>(HttpRequestMessage request, bool ignoreFeedBack = false)
        {

            var client = new HttpClient();
            var result = new RequestResult<T>();

            if (request.RequestUri.IsAbsoluteUri)
            {
                client.BaseAddress = null;
            }
            else
            {
                client.BaseAddress = new Uri(BaseHost);
            }

            if (request == null)
            {
                result.Success = false;
                result.Message = "Request can't be null".GetLocalized();
                return result;
            }

            if (Token != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Token", Token.key);

            try
            {
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    result.Success = false;
                    result.Message = response.ReasonPhrase + ": " + response.Content?.ReadAsStringAsync().Result;
                    result.StatusCode = (int)response.StatusCode;
                }
                else
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(json);

                    if (!ignoreFeedBack)
                    {
                        var data = await Json.ToObjectAsync<T>(json);
                        result.Data = data;
                    }
                }

            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;
            }

            return result;
        }

        public async Task<RequestResult<string>> SendRequestAsync(HttpRequestMessage request)
        {
            return await SendRequestAsync<string>(request, true);

            //var client = new HttpClient();
            //var result = new RequestResult<string>();

            //if (request.RequestUri.IsAbsoluteUri)
            //{
            //    client.BaseAddress = null;
            //}
            //else
            //{
            //    client.BaseAddress = new Uri(BaseHost);
            //}

            //if (request == null)
            //{
            //    result.Success = false;
            //    result.Message = "Request can't be null".GetLocalized();
            //    return result;
            //}

            //if (Token != null)
            //    request.Headers.Authorization = new AuthenticationHeaderValue("Token", Token.key);

            //Debug.WriteLine(request.Content.ReadAsStringAsync());
            //try
            //{
            //    var response = await client.SendAsync(request);

            //    if (!response.IsSuccessStatusCode)
            //    {
            //        result.Success = false;
            //        result.Message = response.ReasonPhrase + ": " + response.Content?.ReadAsStringAsync();
            //        result.StatusCode = (int)response.StatusCode;
            //    }
            //}
            //catch (Exception e)
            //{
            //    result.Success = false;
            //    result.Message = e.Message;
            //}
            //return result;
        }

        public async Task<RequestResult<Token>> LoginAsync(string username, string password)
        {
            //var client = new RestClient
            //{
            //    Proxy = WebRequest.GetSystemWebProxy(),
            //    BaseUrl = new Uri(BaseUrl),
            //    Authenticator = new SimpleAuthenticator("username", username, "password", password)
            //};

            //var request = new RestRequest("auth/login/", Method.POST);
            //var response = await client.ExecuteTaskAsync<Token>(request);
            //var result = new RequestResult<string>();

            //var client = new HttpClient();
            //var result = new RequestResult<string>();
            //client.BaseAddress = new Uri(BaseHost);

            //var byteArray = Encoding.ASCII.GetBytes(username + ":" + password);
            //var h = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            //client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(byteArray));

            //HttpResponseMessage response = await client.PostAsync("api/auth/login/", null);


            var form = new MultipartFormDataContent();

            //Judge if its an email address.
            Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");

            if (r.IsMatch(username))
            {
                form.Add(new StringContent(username), "email");
            }
            else
            {
                form.Add(new StringContent(username), "username");
            }

            form.Add(new StringContent(password), "password");
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/login/");
            request.Content = form;

            var result = await SendRequestAsync<Token>(request);

            if (result.Success)
            {
                Debug.WriteLine(result.Data.key);

                // Create the password vault
                var vault = new PasswordVault();

                // Store the values in the vault
                vault.Add(new PasswordCredential("pshy", "username", username));
                vault.Add(new PasswordCredential("pshy", "password", password));

                vault.Add(new PasswordCredential("pshy", "Token", result.Data.key));
            }

            return result;
        }

        public class RequestResult<T>
        {
            public bool Success { get; set; }
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }

            public RequestResult(bool result = true, int statusCode = 200)
            {
                Success = result;
                StatusCode = statusCode;
            }
        }

    }
}
