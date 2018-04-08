using System;

using pshy.Services;
using pshy.Views;
using Windows.ApplicationModel.Activation;
using Windows.Networking.Connectivity;
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace pshy
{
    public sealed partial class App : Application
    {
        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public App()
        {
            InitializeComponent();

            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(ViewModels.MainViewModel), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            return new Views.ShellPage();
        }
        #region Static App Helpers


        /// <summary>
        /// Stops the back event from being called, allowing for manual overiding
        /// </summary>
        public static bool OverrideBackEvent { get; set; }

        /// <summary>
        ///     Is the app currently in the background.
        /// </summary>
        public static bool IsBackground { get; set; }

        /// <summary>
        ///     Is the app running on xbox
        /// </summary>
        public static bool IsXbox => AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox";

        /// <summary>
        ///     Is the app runnning on a phone
        /// </summary>
        public static bool IsMobile => AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile";

        /// <summary>
        ///     Is the app running on desktop
        /// </summary>
        public static bool IsDesktop => AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop";

        /// <summary>
        ///     Is the application fullscreen.
        /// </summary>
        public static bool IsFullScreen => ApplicationView.GetForCurrentView().IsFullScreenMode;

        /// <summary>
        ///     Does the application currently have access to the internet.
        /// </summary>
        public static bool HasInternet
        {
            get
            {
                try
                {
                    var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                    return connectionProfile != null &&
                           connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        #endregion
    }
}
