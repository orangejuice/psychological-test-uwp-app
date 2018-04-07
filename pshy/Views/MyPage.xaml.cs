using System;

using pshy.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace pshy.Views
{
    public sealed partial class MyPage : Page
    {
        private MyViewModel ViewModel
        {
            get { return DataContext as MyViewModel; }
        }

        public MyPage()
        {
            InitializeComponent();
            Loaded += MyPage_Loaded;
        }

        private async void MyPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }
    }
}
