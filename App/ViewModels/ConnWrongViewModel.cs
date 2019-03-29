using System;

using GalaSoft.MvvmLight;

namespace App.ViewModels
{
    public class ConnWrongViewModel : ViewModelBase
    {
        public Uri WebViewURIsource = new Uri("ms-appx-web:///Assets/Game/index.html");
        public ConnWrongViewModel()
        {
        }
    }
}
