namespace Gu.Wpf.Adorners
{
    using System.Windows;

    public static class Loaded
    {
        /// <summary>
        /// This is a hack to use dp inheritance to trickle down so that we can add empty loaded handlers.
        /// Inspired by: https://gist.github.com/mwisnicki/3104963
        /// </summary>
        private static readonly DependencyProperty FlagProperty = DependencyProperty.RegisterAttached(
            "Flag",
            typeof(bool),
            typeof(Loaded),
            new FrameworkPropertyMetadata(
                default(bool),
                FrameworkPropertyMetadataOptions.Inherits,
                OnFlagChanged));
        private static readonly RoutedEventHandler EmptyRoutedEventHandler = delegate { };

        public static void Track()
        {
            EventManager.RegisterClassHandler(typeof(Window), FrameworkElement.LoadedEvent, new RoutedEventHandler(OnWindowLoaded));
        }

        private static void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            ((Window)sender).SetValue(FlagProperty, true);
        }

        private static void OnFlagChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FrameworkElement)?.AddHandler(FrameworkElement.LoadedEvent, EmptyRoutedEventHandler);
            (d as FrameworkContentElement)?.AddHandler(FrameworkContentElement.LoadedEvent, EmptyRoutedEventHandler);
        }
    }
}
