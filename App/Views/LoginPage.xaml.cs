using System;
using App.Services;
using App.ViewModels;
using CommonServiceLocator;
using Microsoft.Toolkit.Uwp.UI.Controls;
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
                Notification.Show(result.Message, 5000);
                //ViewModel.StatusText = result.Message;
                //ViewModel.StatusVisable = Visibility.Visible;
            }
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
