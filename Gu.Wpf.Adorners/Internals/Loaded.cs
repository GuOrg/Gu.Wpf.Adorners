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

        internal static void Track()
        {
            EventManager.RegisterClassHandler(
                typeof(Window),
                FrameworkElement.LoadedEvent,
                new RoutedEventHandler(OnWindowLoaded));
        }

        internal static bool IsLoaded(DependencyObject element)
        {
            var fe = element as FrameworkElement;
            if (fe != null)
            {
                return fe.IsLoaded;
            }

            var fce = element as FrameworkContentElement;
            if (fce != null)
            {
                return fce.IsLoaded;
            }

            return false;
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
                fe.Loaded += (sender, args) => { };
                if (fe.IsLoaded)
                {
                    fe.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
                }
            }

            var fce = d as FrameworkContentElement;
            if (fce != null)
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
