using System;
using App.Helpers;
using App.Services;
using GalaSoft.MvvmLight;

namespace App.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        public OrangeService OrangeService => OrangeService.Current;

        public UserViewModel()
        {
            ViewModelConnHelper.OnMessageTransmitted += OnMessageTransmitted;
        }

        private void OnMessageTransmitted(string message)
        {
            if (message == "avatar_update")
                RaisePropertyChanged("OrangeService");
        }
    }
}
