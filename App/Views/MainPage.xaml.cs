using System;
using App.Models;
using App.Services;
using App.ViewModels;
using CommonServiceLocator;
using Windows.UI.Xaml.Controls;

namespace App.Views
{
    public sealed partial class MainPage : Page
    {
        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }

        private void PostsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedPost = e.ClickedItem as Post;
            NavigationService.Navigate(typeof(PostDetailViewModel).FullName, selectedPost);
        }

        private void MenuFlyout_Opening(object sender, object e)
        {

        }
    }
}
