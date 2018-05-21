using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using App.Models;
using App.ViewModels;

using Windows.UI.Xaml.Controls;

namespace App.Views
{
    public sealed partial class ScaleTestPage : Page
    {
        private ScaleTestViewModel ViewModel
        {
            get { return DataContext as ScaleTestViewModel; }
        }

        public ScaleTestPage()
        {
            InitializeComponent();
        }

        private void PreviousButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.curr -= 1;
            ViewModel.RenderItem(ViewModel.curr);
        }

        private void NextButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.curr += 1;
            ViewModel.RenderItem(ViewModel.curr);
        }

        private void StopButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Task x = ViewModel.SubmitOpts();
        }

        private void OptionsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0) {
                ViewModel.chooseOpt(((ScaleOpt)e.AddedItems[0]).key);
            }
        }
    }
}
