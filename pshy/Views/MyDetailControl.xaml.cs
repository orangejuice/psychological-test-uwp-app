using System;

using pshy.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace pshy.Views
{
    public sealed partial class MyDetailControl : UserControl
    {
        public SampleOrder MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(MyDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public MyDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MyDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
