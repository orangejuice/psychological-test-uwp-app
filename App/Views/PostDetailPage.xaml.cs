using System;

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
            //ViewModel.Initialize(webView);
        }
    }
}
