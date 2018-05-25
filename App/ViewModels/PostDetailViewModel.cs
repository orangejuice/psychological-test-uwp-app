using System;
using System.Diagnostics;
using System.Net.Http;
using System.Windows.Input;
using App.Helpers;
using App.Models;
using App.Services;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace App.ViewModels
{
    public class PostDetailViewModel : ViewModelBase
    {
        protected NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        private Post _currentPost;
        public Post CurrentPost { get => _currentPost; set => Set(ref (_currentPost), value); }

        public PostDetailViewModel()
        {
            //isloading = true;
            //source = new uri(defaulturl);
            NavigationService.Navigated += Navigated;
        }

        private void Navigated(object sender, NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back && e.Parameter is Post post)
            {
                CurrentPost = AsyncHelper.RunSync(async () => await ObjectCloneHelper.CloneAsync(post));
                var request = new HttpRequestMessage(HttpMethod.Get, "/api/posts/" + post.id + "/");

                var result = AsyncHelper.RunSync(async () => await OrangeService.Current.SendRequestAsync<Post>(request));
                if (result.Success)
                    CurrentPost = result.Data;

                var converter = new DateTimeConverter();

                var tobeAdded = "#" + post.title + "  \n>" + post.author.username + ",  " + post.cate_name + ",  " + converter.Convert(post.created, null, null, null) +
                    (post.created != post.updated ? "  (" + "edited".GetLocalized() + ": " + converter.Convert(post.updated, null, null, null) + ")" : "") +
                      "  \n\n&nbsp;\n\n" + "*****\n\n&nbsp;\n\n";

                if (!CurrentPost.content.StartsWith(tobeAdded))
                {
                    CurrentPost.content = tobeAdded + post.content;
                }

                // favorite
                if (CurrentPost.favor)
                {
                    ToggleFavorite(true);
                }

                // load comments.
                LoadComments();
            }
        }
        
        private Uri _webViewURISource;
        public Uri WebViewURIsource
        {
            get => _webViewURISource;
            set {
                Set(ref (_webViewURISource), value);
            }
        }

        private void LoadComments()
        {
            WebViewURIsource = new Uri(OrangeService.BaseHost + "api/post/" + CurrentPost.id + "/comments");
        }

        public Visibility AddFavoriteVisable { get; set; } = Visibility.Visible;

        public Visibility CalFavoriteVisable { get; set; } = Visibility.Collapsed;

        public void ToggleFavorite(bool calfavorite = false)
        {
            if (calfavorite)
            {
                AddFavoriteVisable = Visibility.Collapsed;
                CalFavoriteVisable = Visibility.Visible;
            }
            else
            {
                AddFavoriteVisable = AddFavoriteVisable == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                CalFavoriteVisable = CalFavoriteVisable == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }

            ViewModelConnHelper.BroadCast("favorite_update");

            RaisePropertyChanged("CalFavoriteVisable");
            RaisePropertyChanged("AddFavoriteVisable");
        }
    }
}
