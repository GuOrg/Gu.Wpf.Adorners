namespace Gu.Wpf.Adorners
{
    using System.Windows;

    internal class Loaded
    {
        /// <summary>
        /// This is a hack to use dp inheritance to trickle down so that we can add empty loaded handlers.
        /// Inspired by: https://gist.github.com/mwisnicki/3104963
        /// </summary>
        private static readonly DependencyProperty IsTrackingProperty = DependencyProperty.RegisterAttached(
            "IsTracking",
            typeof(bool),
            typeof(Loaded),
            new FrameworkPropertyMetadata(
                default(bool),
                FrameworkPropertyMetadataOptions.Inherits,
                OnFlagChanged));

        public static void Track()
        {
            EventManager.RegisterClassHandler(
                typeof(Window),
                FrameworkElement.LoadedEvent, 
                new RoutedEventHandler(OnWindowLoaded));
        }

        private static void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            ((Window)sender).SetValue(IsTrackingProperty, true);
        }

        private static void OnFlagChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe != null)
            {
                fe.Loaded += delegate { };
                if (fe.IsLoaded)
                {
                    fe.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
                }
            }

            var fce = d as FrameworkContentElement;
            if (fce != null)
            {
                fce.Loaded += delegate { };
                if (fce.IsLoaded)
                {
                    fce.RaiseEvent(new RoutedEventArgs(FrameworkContentElement.LoadedEvent));
                }
            }
        }
    }
}
