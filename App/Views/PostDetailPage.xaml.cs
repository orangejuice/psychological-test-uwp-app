using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using App.Helpers;
using App.Services;
using App.ViewModels;

using Windows.UI.Xaml.Controls;

namespace App.Views
{
    public sealed partial class PostDetailPage : Page
    {
        private PostDetailViewModel ViewModel
        {
            get { return DataContext as PostDetailViewModel; }
        }
        
        public PostDetailPage()
        {
            InitializeComponent();

            //CommentView.NavigationStarting += CommentView_NavigationStarting;
            CommentView.NavigationCompleted += CommentView_NavigationCompletedAsync;
        }

        private async void CommentView_NavigationCompletedAsync(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            var webView = sender as WebView;
            // get the total width and height
            var heightString = await webView.InvokeScriptAsync("eval", new[] { "document.body.scrollHeight.toString()" });
            if (!int.TryParse(heightString, out int height))
            {
                throw new Exception("Unable to get page height");
            }
            // resize the webview to the content
            webView.Height = height;

            // cancel the webview scrollbar.
            var x = await webView.InvokeScriptAsync("eval", new[] { "document.body.style.overflow = 'hidden';" });
        }

        private void CommentView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            CommentView.NavigationStarting -= CommentView_NavigationStarting;
            args.Cancel = true;//cancel navigation in a handler for this event by setting the WebViewNavigationStartingEventArgs.Cancel property to true

            var method = Windows.Web.Http.HttpMethod.Get;

            var request = new Windows.Web.Http.HttpRequestMessage(method, args.Uri);
            if (OrangeService.Current.Token != null)
            {
                request.Headers.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("Token", OrangeService.Current.Token.key);
            }

            Debug.WriteLine("log- navigate to new page");
            CommentView.NavigateWithHttpRequestMessage(request);
            CommentView.NavigationStarting += CommentView_NavigationStarting;
        }

        private async void Favorite_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            AddFavoriteButton.IsEnabled = false;
            CalFavoriteButton.IsEnabled = false;

            var form = new StringContent(await Json.StringifyAsync(new Dictionary<string, int>()
            {
                {"post", ViewModel.CurrentPost.id }
            }), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/posts-favor/");

            request.Content = form;

            var result = await OrangeService.Current.SendRequestAsync(request);
            if (result.Success)
            {
                ViewModel.ToggleFavorite();
            }
            else
            {
                Notification.Show(result.Message, 3000);
            }

            AddFavoriteButton.IsEnabled = true;
            CalFavoriteButton.IsEnabled = true;
        }
    }
}
