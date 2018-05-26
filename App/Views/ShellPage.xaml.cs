using System;
using System.Diagnostics;
using App.Helpers;
using App.Services;
using App.ViewModels;
using APP.Helpers;
using CommonServiceLocator;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Controls;

namespace App.Views
{
    public sealed partial class ShellPage : Page
    {
        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public TitleBarHelper TitleHelper => TitleBarHelper.Instance;

        private ShellViewModel ViewModel
        {
            get { return DataContext as ShellViewModel; }
        }

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, navigationView);
            NetworkHelper.Current.NetworkStatusChanged += Current_NetworkStatusChanged;
            Current_NetworkStatusChanged(null);
        }

        private void Current_NetworkStatusChanged(object sender)
        {
            if (NetworkHelper.Current.Network == 4)
            {
                var x = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    NavigationService.Navigate(typeof(ConnWrongViewModel).FullName);
                    NavigationService.ClearBackStack();
                    navigationView.IsEnabled = false;
                });
            }
            else
            {
                var x = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    if(navigationView.IsEnabled == false)
                    {
                        NavigationService.Navigate(typeof(MainViewModel).FullName);
                        navigationView.IsEnabled = true;
                    }
                });
            }
        }

        private void ShellLogin_Selected(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(LoginViewModel).FullName);
        }

        private void ShellUser_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(UserViewModel).FullName);
        }
    }
}
