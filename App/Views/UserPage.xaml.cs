using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using App.Helpers;
using App.Models;
using App.Services;
using App.ViewModels;
using CommonServiceLocator;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace App.Views
{
    public sealed partial class UserPage : Page
    {
        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        private UserViewModel ViewModel
        {
            get { return DataContext as UserViewModel; }
        }

        public UserPage()
        {
            InitializeComponent();
        }

        private async void AvatarChange_ClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            //openPicker.ViewMode = PickerViewMode.List;

            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            var file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    var form = new MultipartFormDataContent();
                    form.Add(new StreamContent(stream.AsStream()), "avatar", file.Name);
                    var request = new HttpRequestMessage(HttpMethod.Put, "/api/users/avatar/");
                    request.Content = form;

                    var result = AsyncHelper.RunSync(async () => await OrangeService.Current.SendRequestAsync(request));

                    if (result.Success)
                    {
                        // TODO update the avatar among the app.
                        Debug.WriteLine("log- success!");
                        ViewModelConnHelper.BroadCast("avatar_update");
                    }
                    else
                    {
                        Notification.Show(result.Message, 5000);
                    }
                }
            }
        }

        private async void PasswordChange_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var form = new MultipartFormDataContent();
            form.Add(new StringContent(NPassword1.Password), "new_password1");
            form.Add(new StringContent(NPassword2.Password), "new_password2");
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/password/change/");
            request.Content = form;

            var result = await OrangeService.Current.SendRequestAsync(request);

            if (result.Success)
            {
                Notification.Show("New Password has been effective, please re-login!", 3000);
                Logout_Click(null, null);
            }
            else
            {
                Notification.Show(result.Message, 3000);
            }
        }

        private void Logout_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.OrangeService.LogoutService();
            NavigationService.ClearBackStack();
            NavigationService.Navigate(typeof(MainViewModel).FullName);
            ViewModelConnHelper.BroadCast("logout");
        }

        private void PostsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var postfavor = e.ClickedItem as PostFavor;
         
            NavigationService.Navigate(typeof(PostDetailViewModel).FullName, postfavor.post);
        }

        private void OnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (e.IsIntermediate)
            {
                var ScrollViewer = PostsListView.ChildrenBreadthFirst().OfType<ScrollViewer>().First();

                var verticalOffset = ScrollViewer.VerticalOffset;
                var maxVerticalOffset = ScrollViewer.ScrollableHeight; //sv.ExtentHeight - sv.ViewportHeight;

                if (maxVerticalOffset < 0 || verticalOffset == maxVerticalOffset)
                {
                    Debug.WriteLine("Scrolled to bottom");
                    Task t = ViewModel.LoadAsync(false);
                }
            }
        }
    }
}
