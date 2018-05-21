using System;

using GalaSoft.MvvmLight;
using Windows.UI.Xaml;

namespace App.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
        }

        private string _username;

        public string Username
        {
            get { return _username; }

            set
            {
                Set(ref _username, value);
                RaisePropertyChanged(() => CanSignIn);
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }

            set
            {
                Set(ref _password, value);
                RaisePropertyChanged(() => CanSignIn);
            }
        }

        public bool CanSignIn => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);

        private Visibility _statusVisable = Visibility.Collapsed;

        public Visibility StatusVisable
        {
            get { return _statusVisable; }

            set { Set(ref _statusVisable, value); }
        }

        private string _statusText = "";

        public string StatusText
        {
            get { return _statusText; }

            set { Set(ref _statusText, value); }
        }

    }
}
