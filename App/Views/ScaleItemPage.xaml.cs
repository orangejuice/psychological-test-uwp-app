using System;
using System.Threading.Tasks;
using App.Services;
using App.ViewModels;
using CommonServiceLocator;
using Windows.UI.Xaml.Controls;

namespace App.Views
{
    public sealed partial class ScaleItemPage : Page
    {
        private NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        private ScaleItemViewModel ViewModel
        {
            get { return DataContext as ScaleItemViewModel; }
        }

        public ScaleItemPage()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(ScaleTestViewModel).FullName, ViewModel.CurrentScale);
        }

    }
}
