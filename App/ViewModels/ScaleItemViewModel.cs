using System;
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
    public class ScaleItemViewModel : ViewModelBase
    {
        public ScaleItemViewModel()
        {
            NavigationService.Navigated += Navigated;
        }

        private void Navigated(object sender, NavigationEventArgs e)
        {

            if (e.NavigationMode != NavigationMode.Back && e.Parameter is Scale scale)
            {
                CurrentScale = AsyncHelper.RunSync(async () => await ObjectCloneHelper.CloneAsync(scale));

                var tobeAdded = "#" + scale.title + "  \n\n>";
                if (!scale.introduction.StartsWith(tobeAdded))
                {
                    CurrentScale.introduction = tobeAdded + scale.introduction;
                }
                if (scale.done != null)
                {
                    AsyncHelper.RunSync(async () => await LoadResultAsync(scale.done));
                    CurrentScale.introduction += "\n\n\n\n******  \n\n";
                    foreach (var item in ScaleResult.conclusion)
                    {
                        CurrentScale.introduction += item.Value.description + "  \n";
                    }
                    // TODO if item  2
                }
            }
        }

        public async Task LoadResultAsync(Uri uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var result = await OrangeService.Current.SendRequestAsync<ScaleResult>(request);
            if (result.Success)
            {
                var data = result.Data;
                ScaleResult = data;
            }
        }

        protected NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();
        
        private Scale _currentScale;
        public Scale CurrentScale { get => _currentScale; set => Set(ref (_currentScale), value); }

        private ScaleResult _scaleResult;
        public ScaleResult ScaleResult { get => _scaleResult; set => Set(ref (_scaleResult), value); }
        
    }
}
