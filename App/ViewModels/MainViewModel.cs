using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Timers;
using App.Helpers;
using App.Models;
using App.Services;
using GalaSoft.MvvmLight;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public int _carouselSelected;

        public int CarouselSelected
        {
            get { return _carouselSelected; }
            set { Set(ref _carouselSelected, value); }
        }

        public ObservableCollection<Uri> Thumbnail { get; set; }

        public MainViewModel()
        {
            var images = new ObservableCollection<Uri>();
            var folder = AsyncHelper.RunSync(async () => await Package.Current.InstalledLocation.GetFolderAsync("Assets\\Carousel"));
            var files = AsyncHelper.RunSync(async () => await folder.GetFilesAsync());
            foreach (var item in files)
            {
                images.Add(new Uri(item.Path));
            }
            Thumbnail = images;


            CarouselSelected = 1;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(4000);
            timer.Tick += OnTimedEvent;
            timer.Start();
        }

        private void OnTimedEvent(object source, object e)
        {
            if (CarouselSelected < Thumbnail.Count - 1)
            {
                CarouselSelected += 1;
            }
            else
            {
                CarouselSelected = 0;
            }
        }

        public Pagination<Post> Posts
        {
            get
            {
                var posts = new Pagination<Post>();
                var request = new HttpRequestMessage(HttpMethod.Get, "/api/posts/");
                var result = AsyncHelper.RunSync(async () => await OrangeService.Current.SendRequestAsync<Pagination<Post>>(request));
                if (result.Success)
                {
                    posts = result.Data;
                    return posts;
                }
                else
                {
                    return new Pagination<Post>();
                }
            }

        }

    }
}
