using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using App.Helpers;
using App.Models;
using App.Services;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace App.ViewModels
{
    public class ScaleTestViewModel : ViewModelBase
    {
        public ScaleTestViewModel()
        {
            NavigationService.Navigated += Navigated;
        }

        public async Task StartTestAsync(Uri uri)
        {
            await Task.Delay(1500);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var result = await OrangeService.Current.SendRequestAsync<Scale>(request);
            if (result.Success)
            {
                var data = result.Data;
                CurrentScale.items = data.items;
                RenderItem(0);
            }
        }
        public void RenderItem(int indicate)
        {
            // question number now.
            curr = indicate;

            // scale item now.
            var item = AsyncHelper.RunSync(async () => await ObjectCloneHelper.CloneAsync(CurrentScale.items[indicate]));
            item.question = "##" + item.question;
            CurrentScaleItem = item;

            // set the head text.
            Header = "#" + CurrentScale.title + "  \n\n" + "**" + (curr + 1) + "**" + " / " +
                CurrentScale.items.Count + "  \n\n";

            // set the prev next button visable.
            if (curr <= 0)
            {
                Previous = false;
                if (CurrentScale.items[curr].isChose)
                    Next = true;
                else
                    Next = false;
            }
            else if (curr >= CurrentScale.items.Count - 1)
            {
                Previous = true;
                Next = false;
            }
            else
            {
                Previous = true;
                if (CurrentScale.items[curr].isChose)
                    Next = true;
                else
                    Next = false;
            }

            // set the selected option.
            if (CurrentScale.items[curr].isChose)
            {
                foreach (var opt in item.opts)
                {
                    if (CurrentScale.items[curr].chose == opt.key)
                    {
                        SelectedOpt = opt;
                        Debug.WriteLine("log- selected before. key equals " + opt.key);
                    }
                }
            }
        }

        public void chooseOpt(int key)
        {
            CurrentScale.items[curr].chose = key;
            CurrentScale.items[curr].isChose = true;
            Debug.WriteLine("log- select. key equals " + key);
            if (curr < CurrentScale.items.Count - 1)
                Next = true;

            // set the comp button.
            bool finished = true;
            foreach (var one in CurrentScale.items)
            {
                if (!one.isChose)
                {
                    finished = false;
                }
            }
            Complete = finished;
        }

        public async Task SubmitOpts()
        {
            // TODO change the method of pop up the tip.
            MessageDialog showDialog = new MessageDialog("Submit_now".GetLocalized());
            showDialog.Commands.Add(new UICommand("Yes".GetLocalized()) { Id = 0 });
            showDialog.Commands.Add(new UICommand("No".GetLocalized()) { Id = 1 });
            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;
            var c = await showDialog.ShowAsync();
            if ((int)c.Id == 0)
            {
                await Task.Delay(1500);
                var opts = new Dictionary<string, object>();
                foreach (var item in CurrentScale.items)
                {
                    opts.Add(item.sn.ToString(), item.chose);
                }
                var dict = new Dictionary<string, object>
                {
                    { "scale", CurrentScale.id },
                    { "opts", opts }
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "/api/eval-record/");
                var send = await Json.StringifyAsync(dict);
                request.Content = new StringContent(send, Encoding.UTF8, "application/json");

                var result = await OrangeService.Current.SendRequestAsync(request);
                if (result.Success)
                {
                    CurrentScale = await LoadScaleAsync();
                    NavigationService.Navigate(typeof(ScaleItemViewModel).FullName, CurrentScale);
                }
            }
        }

        public async Task<Scale> LoadScaleAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, CurrentScale.url);

            var result = await OrangeService.Current.SendRequestAsync<Scale>(request);
            if (result.Success)
            {
                var data = result.Data;
                return data;
            }
            return null;
        }

        private void Navigated(object sender, NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back && e.Parameter is Scale scale)
            {
                CurrentScale = AsyncHelper.RunSync(async () => await ObjectCloneHelper.CloneAsync(scale));

                Task x = StartTestAsync(scale.url);
            }
        }

        public int curr { get; set; }

        protected NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();
        
        private Scale _currentScale;
        public Scale CurrentScale { get => _currentScale; set => Set(ref (_currentScale), value); }

        private string _header;
        public string Header { get => _header; set => Set(ref (_header), value); }

        private ScaleItem _currentScaleItem = new ScaleItem();
        public ScaleItem CurrentScaleItem { get => _currentScaleItem; set => Set(ref (_currentScaleItem), value); }

        private ScaleOpt _selectedOpt;
        public ScaleOpt SelectedOpt { get => _selectedOpt; set => Set(ref (_selectedOpt), value); }

        private bool _previous;
        public bool Previous { get => _previous; set => Set(ref (_previous), value); }

        private bool _next;
        public bool Next { get => _next; set => Set(ref (_next), value); }

        private bool _complete;
        public bool Complete { get => _complete; set => Set(ref (_complete), value); }

    }
}
