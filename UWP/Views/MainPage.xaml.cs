using System;

using pshy.ViewModels;

using Windows.UI.Xaml.Controls;

namespace pshy.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
