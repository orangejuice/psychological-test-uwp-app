using System;
using App.Services;
using App.ViewModels;
using CommonServiceLocator;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace App.Views
{
    public sealed partial class LoginPage : Page
    {

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        private LoginViewModel ViewModel
        {
            get { return DataContext as LoginViewModel; }
        }

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginActionAsync(object sender, RoutedEventArgs e)
        {
            var result = await OrangeService.Current.LoginAsync(ViewModel.Username, ViewModel.Password);
            if (result.Success)
            {
                NavigationService.Navigate("App.ViewModels.MainViewModel");
            }
            else
            {
                ViewModel.StatusText = result.Message;
                ViewModel.StatusVisable = Visibility.Visible;
            }

            //var request = new RestRequest("auth/login/", Method.POST);
            //request.AddParameter("username", ViewModel.Username);
            //request.AddParameter("password", ViewModel.Password);
            //Debug.WriteLine(ViewModel.Username);
            //Debug.WriteLine(ViewModel.Password);
            //Token token = RestApiService.Current.SendRequest<Token>(request);
            //Debug.WriteLine(token.token);
            //Debug.WriteLine(token.detail);

            // Called when the webview is going to navigate to another 
            // Uri.
            //LoginWebView.NavigationStarting += async (view, eventArgs) =>
            //{
            //    // If we are navigating to google, let the user know that google login is not supported
            //    if (eventArgs.Uri.Host == "accounts.google.com")
            //    {
            //        // Cancel the page load and hide the loading panel
            //        eventArgs.Cancel = true;
            //        LoadingSection.Visibility = Visibility.Collapsed;
            //        TelemetryService.Current.TrackEvent("Google Sign in Attempt");
            //        await new MessageDialog("Google Account sign in is not supported. Please instead signin with a Facebook or SoundCloud account.", "Sign in Error").ShowAsync();
            //    }

            //    // We worry about localhost addresses are they are directed towards us.
            //    if (eventArgs.Uri.Host == "localhost")
            //    {
            //        // Cancel the navigation, (as localhost does not exist).
            //        eventArgs.Cancel = true;

            //        // Parse the URL for work
            //        // ReSharper disable once CollectionNeverUpdated.Local
            //        var parser = new QueryParameterCollection(eventArgs.Uri);

            //        // First we just check that the state equals (to make sure the url was not hijacked)
            //        var state = parser.FirstOrDefault(x => x.Key == "state").Value;

            //        // The state does not match
            //        if (string.IsNullOrEmpty(state) || state.TrimEnd('#') != stateVerification)
            //        {
            //            // Display the error to the user
            //            await new MessageDialog("State Verfication Failed. This could be caused by another process intercepting the SoundByte login procedure. Sigin has been canceled to protect your privacy.", "Sign in Error").ShowAsync();
            //            TelemetryService.Current.TrackEvent("State Verfication Failed");
            //            // Close
            //            LoadingSection.Visibility = Visibility.Collapsed;
            //            App.GoBack();
            //            return;
            //        }

            //        // We have an error
            //        if (parser.FirstOrDefault(x => x.Key == "error").Value != null)
            //        {
            //            var type = parser.FirstOrDefault(x => x.Key == "error").Value;
            //            var reason = parser.FirstOrDefault(x => x.Key == "error_description").Value;

            //            // The user denied the request
            //            if (type == "access_denied")
            //            {
            //                LoadingSection.Visibility = Visibility.Collapsed;
            //                App.GoBack();
            //                return;
            //            }

            //            // Display the error to the user
            //            await new MessageDialog(reason, "Sign in Error").ShowAsync();

            //            // Close
            //            LoadingSection.Visibility = Visibility.Collapsed;
            //            App.GoBack();
            //            return;
            //        }

            //        // Get the code from the url
            //        if (parser.FirstOrDefault(x => x.Key == "code").Value != null)
            //        {
            //            var code = parser.FirstOrDefault(x => x.Key == "code").Value;

            //            // Create a http client to get the token
            //            using (var httpClient = new HttpClient())
            //            {
            //                // Set the user agent string
            //                httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("SoundByte", Package.Current.Id.Version.Major + "." + Package.Current.Id.Version.Minor + "." + Package.Current.Id.Version.Build));

            //                // Get all the params
            //                var parameters = new Dictionary<string, string>
            //                {
            //                    {"client_id", accountType == "soundcloud" ? Common.ServiceKeys.SoundCloudClientId : Common.ServiceKeys.FanburstClientId},
            //                    {"client_secret", accountType == "soundcloud" ? Common.ServiceKeys.SoundCloudClientSecret :  Common.ServiceKeys.FanburstClientSecret},
            //                    {"grant_type", "authorization_code"},
            //                    {"redirect_uri", callback.ToString()},
            //                    {"code", code}
            //                };

            //                var encodedContent = new FormUrlEncodedContent(parameters);

            //                // Post to the soundcloud API
            //                using (var postQuery = await httpClient.PostAsync(accountType == "soundcloud" ? "https://api.soundcloud.com/oauth2/token" : "https://fanburst.com/oauth/token", encodedContent))
            //                {
            //                    // Check if the post was successful
            //                    if (postQuery.IsSuccessStatusCode)
            //                    {
            //                        // Get the stream
            //                        using (var stream = await postQuery.Content.ReadAsStreamAsync())
            //                        {
            //                            // Read the stream
            //                            using (var streamReader = new StreamReader(stream))
            //                            {
            //                                // Get the text from the stream
            //                                using (var textReader = new JsonTextReader(streamReader))
            //                                {
            //                                    // Used to get the data from JSON
            //                                    var serializer = new JsonSerializer
            //                                    {
            //                                        NullValueHandling = NullValueHandling.Ignore
            //                                    };

            //                                    // Get the class from the json
            //                                    var response = serializer.Deserialize<SoundByteService.Token>(textReader);

            //                                    // Create the password vault
            //                                    var vault = new PasswordVault();

            //                                    if (accountType == "soundcloud")
            //                                    {
            //                                        // Store the values in the vault
            //                                        vault.Add(new PasswordCredential("SoundByte.SoundCloud", "Token", response.AccessToken.ToString()));
            //                                        vault.Add(new PasswordCredential("SoundByte.SoundCloud", "Scope", response.Scope.ToString()));
            //                                    }
            //                                    else
            //                                    {
            //                                        // Store the values in the vault
            //                                        vault.Add(new PasswordCredential("SoundByte.FanBurst", "Token", response.AccessToken.ToString()));
            //                                    }

            //                                    LoadingSection.Visibility = Visibility.Collapsed;
            //                                    TelemetryService.Current.TrackEvent("Login Successful", new Dictionary<string, string>()
            //                                    {
            //                                        { "service", accountType }
            //                                    });
            //                                    App.NavigateTo(typeof(HomeView));
            //                                }
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        // Display the error to the user
            //                        await new MessageDialog("Token Error. Try again later.", "Sign in Error").ShowAsync();

            //                        // Close
            //                        LoadingSection.Visibility = Visibility.Collapsed;
            //                        App.GoBack();
            //                    }
            //                }
            //            }
            //        }
            //    }
            //};
        }

        private void Password_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter && ViewModel.CanSignIn)
            {
                LoginActionAsync(null, null);
            }
        }

    }
}
