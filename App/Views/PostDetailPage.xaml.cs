using System;
using System.Collections.Generic;
using System.Net.Http;
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
