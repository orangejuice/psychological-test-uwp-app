using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using App.Helpers;
using App.Models;
using App.Services;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace App.ViewModels
{
    public class ScaleViewModel : ViewModelBase
    {
        public ObservableCollection<Scale> _items = new ObservableCollection<Scale>();

        public ObservableCollection<Scale> Items
        {
            get { return _items; }
            set { Set(ref _items, value); }
        }

        private Visibility _loading = Visibility.Collapsed;

        public Visibility Loading { get => _loading; set => Set(ref (_loading), value); }

        private Uri _next = null;

        public Uri Next
        {
            get { return _next; }

            set { Set(ref _next, value); }
        }

        public ScaleViewModel()
        {
            NavigationService.Navigated += Navigated;
        }

        private void Navigated(object sender, NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                Task x = LoadAsync();
            }
        }

        public async Task LoadAsync(bool initialize = true)
        {
            Loading = Visibility.Visible;
            if (initialize)
                await Task.Delay(800);
            else
                await Task.Delay(2000);

            if (initialize || Next != null)
            {
                var request = Next == null ?
                new HttpRequestMessage(HttpMethod.Get, "/api/eval/") :
                new HttpRequestMessage(HttpMethod.Get, Next);

                var result = await OrangeService.Current.SendRequestAsync<Pagination<Scale>>(request);
                if (result.Success)
                {
                    var data = result.Data;
                    Next = data?.next;
                    if (initialize) Items.Clear();
                    foreach (var item in data?.results)
                    {
                        Items.Add(item);
                    }
                }
            }

            Loading = Visibility.Collapsed;
        }

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

    }
}
