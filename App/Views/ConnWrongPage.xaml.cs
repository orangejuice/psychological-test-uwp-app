using System;

using App.ViewModels;

using Windows.UI.Xaml.Controls;

namespace App.Views
{
    public sealed partial class ConnWrongPage : Page
    {
        private ConnWrongViewModel ViewModel
        {
            get { return DataContext as ConnWrongViewModel; }
        }

        public ConnWrongPage()
        {
            InitializeComponent();
        }
    }
}
