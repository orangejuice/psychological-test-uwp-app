using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using App.Helpers;
using App.Models;
using App.Services;
using GalaSoft.MvvmLight;
using Windows.UI.Xaml;

namespace App.ViewModels
{
    public class PostViewModel : ViewModelBase
    {

        private Visibility _loading = Visibility.Collapsed;
        public Visibility Loading { get => _loading; set => Set(ref (_loading), value); }

        public PostViewModel()
        {
            Task x = LoadAsync();
        }

        public async Task LoadAsync(bool initialize = true)
        {
            Loading = Visibility.Visible;
            if (initialize)
            {
                await Task.Delay(800);
            }
            else
            {
                await Task.Delay(2000);
            }

            if (initialize || Next != null)
            {
                var request = Next == null ?
                new HttpRequestMessage(HttpMethod.Get, "/api/posts/") :
                new HttpRequestMessage(HttpMethod.Get, Next);

                var result = await OrangeService.Current.SendRequestAsync<Pagination<Post>>(request);
                if (result.Success)
                {

                    var data = result.Data;
                    Next = data?.next;
                    foreach (var item in data?.results)
                    {
                        Items.Add(item);
                    }
                }
            }

            Loading = Visibility.Collapsed;
        }

        private Uri _next = null;

        public Uri Next
        {
            get { return _next; }

            set { Set(ref _next, value); }
        }

        public ObservableCollection<Post> _items = new ObservableCollection<Post>();

        public ObservableCollection<Post> Items
        {
            get { return _items; }
            set { Set(ref _items, value); }
        }
    }
}
