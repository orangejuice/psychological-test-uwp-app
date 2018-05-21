using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using App.Helpers;
using App.Models;
using App.Services;
using App.ViewModels;
using CommonServiceLocator;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace App.Views
{
    public sealed partial class ScalePage : Page
    {
        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        private ScaleViewModel ViewModel
        {
            get { return DataContext as ScaleViewModel; }
        }

        private static int _persistedItemIndex = -1;

        public ScalePage()
        {
            InitializeComponent();
        }

        private void ItemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedScale = e.ClickedItem as Scale;
            _persistedItemIndex = ItemGridView.Items.IndexOf(e.ClickedItem);
            NavigationService.Navigate(typeof(ScaleItemViewModel).FullName, selectedScale);
        }

        private void OnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (e.IsIntermediate)
            {
                var ScrollViewer = ItemGridView.ChildrenBreadthFirst().OfType<ScrollViewer>().First();

                var verticalOffset = ScrollViewer.VerticalOffset;
                var maxVerticalOffset = ScrollViewer.ScrollableHeight; //sv.ExtentHeight - sv.ViewportHeight;

                if (maxVerticalOffset < 0 || verticalOffset == maxVerticalOffset)
                {
                    Debug.WriteLine("Scrolled to bottom");
                    Task t = ViewModel.LoadAsync(false);
                }
            }
        }

        private void ItemGridView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var scrollViewer = ItemGridView.ChildrenBreadthFirst().OfType<ScrollViewer>().First();
            scrollViewer.ViewChanged += OnViewChanged;
        }


        #region staggering

        private void ItemGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            args.ItemContainer.Loaded += ItemContainer_Loaded;

        }

        private void ItemContainer_Loaded(object sender, RoutedEventArgs e)
        {
            var itemsPanel = (ItemsWrapGrid)ItemGridView.ItemsPanelRoot;
            var itemContainer = (GridViewItem)sender;

            var itemIndex = ItemGridView.IndexFromContainer(itemContainer);

            var relativeIndex = itemIndex - itemsPanel.FirstVisibleIndex;

            var uc = itemContainer.ContentTemplateRoot as Grid;

            if (itemIndex != _persistedItemIndex && itemIndex >= 0 && itemIndex >= itemsPanel.FirstVisibleIndex && itemIndex <= itemsPanel.LastVisibleIndex)
            {
                var itemVisual = ElementCompositionPreview.GetElementVisual(uc);
                ElementCompositionPreview.SetIsTranslationEnabled(uc, true);

                var easingFunction = Window.Current.Compositor.CreateCubicBezierEasingFunction(new Vector2(0.1f, 0.9f), new Vector2(0.2f, 1f));

                // Create KeyFrameAnimations
                var offsetAnimation = Window.Current.Compositor.CreateScalarKeyFrameAnimation();
                offsetAnimation.InsertKeyFrame(0f, 100);
                offsetAnimation.InsertKeyFrame(1f, 0, easingFunction);
                offsetAnimation.Target = "Translation.X";
                offsetAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;
                offsetAnimation.Duration = TimeSpan.FromMilliseconds(700);
                offsetAnimation.DelayTime = TimeSpan.FromMilliseconds(relativeIndex * 100);

                var fadeAnimation = Window.Current.Compositor.CreateScalarKeyFrameAnimation();
                fadeAnimation.InsertExpressionKeyFrame(0f, "0");
                fadeAnimation.InsertExpressionKeyFrame(1f, "1");
                fadeAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;
                fadeAnimation.Duration = TimeSpan.FromMilliseconds(700);
                fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(relativeIndex * 100);

                // Start animations
                itemVisual.StartAnimation("Translation.X", offsetAnimation);
                itemVisual.StartAnimation("Opacity", fadeAnimation);
            }
            else
            {
                //Debug.WriteLine("Skipping");
            }

            itemContainer.Loaded -= ItemContainer_Loaded;
        }

        #endregion

    }
}
