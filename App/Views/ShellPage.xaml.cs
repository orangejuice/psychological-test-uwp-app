using App.Services;
using App.ViewModels;
using APP.Helpers;
using CommonServiceLocator;
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
