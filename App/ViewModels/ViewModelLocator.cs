using System;

using App.Services;
using App.Views;

using CommonServiceLocator;

using GalaSoft.MvvmLight.Ioc;

namespace App.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            SimpleIoc.Default.Register<ShellViewModel>();
            Register<MainViewModel, MainPage>();
            Register<PostDetailViewModel, PostDetailPage>();
            Register<SettingsViewModel, SettingsPage>();
            Register<LoginViewModel, LoginPage>();
            Register<UserViewModel, UserPage>();
            Register<PostViewModel, PostPage>();
            Register<ScaleViewModel, ScalePage>();
            Register<ScaleItemViewModel, ScaleItemPage>();
            Register<ScaleTestViewModel, ScaleTestPage>();
        }

        public ScaleTestViewModel ScaleTestViewModel => ServiceLocator.Current.GetInstance<ScaleTestViewModel>();

        public ScaleItemViewModel ScaleItemViewModel => ServiceLocator.Current.GetInstance<ScaleItemViewModel>();

        public ScaleViewModel ScaleViewModel => ServiceLocator.Current.GetInstance<ScaleViewModel>();

        public PostViewModel PostViewModel => ServiceLocator.Current.GetInstance<PostViewModel>();

        public UserViewModel UserViewModel => ServiceLocator.Current.GetInstance<UserViewModel>();

        public LoginViewModel LoginViewModel => ServiceLocator.Current.GetInstance<LoginViewModel>();

        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();
        
        public PostDetailViewModel PostDetailViewModel => ServiceLocator.Current.GetInstance<PostDetailViewModel>();

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>() where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
