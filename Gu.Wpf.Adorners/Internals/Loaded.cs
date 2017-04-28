namespace Gu.Wpf.Adorners
{
    using System.Windows;

    internal static class Loaded
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
                OnIsTrackingChanged));

        internal static void Track()
        {
            EventManager.RegisterClassHandler(
                typeof(Window),
                FrameworkElement.LoadedEvent,
                new RoutedEventHandler(OnWindowLoaded));
        }

        internal static bool IsLoaded(DependencyObject element)
        {
            if (element is FrameworkElement fe)
            {
                return fe.IsLoaded;
            }

            if (element is FrameworkContentElement fce)
            {
                return fce.IsLoaded;
            }

            return false;
        }

        private static void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            ((Window)sender).SetCurrentValue(IsTrackingProperty, true);
        }

        private static void OnIsTrackingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement fe)
            {
                fe.Loaded += (sender, args) => { };
                if (fe.IsLoaded)
                {
                    fe.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
                }
            }

            if (d is FrameworkContentElement fce)
            {
                fce.Loaded += (sender, args) => { };
                if (fce.IsLoaded)
                {
                    fce.RaiseEvent(new RoutedEventArgs(FrameworkContentElement.LoadedEvent));
                }
            }
        }
    }
}
