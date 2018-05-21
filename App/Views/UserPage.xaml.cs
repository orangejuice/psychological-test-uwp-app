using System;

using App.ViewModels;

using Windows.UI.Xaml.Controls;

namespace App.Views
{
    public sealed partial class UserPage : Page
    {
        private UserViewModel ViewModel
        {
            get { return DataContext as UserViewModel; }
        }

        public UserPage()
        {
            InitializeComponent();
        }
    }
}
