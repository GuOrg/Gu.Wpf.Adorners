namespace Gu.Wpf.Adorners
{
    using System.Windows;

    internal static class Visible
    {
        internal static readonly RoutedEvent IsVisibleChangedEvent = EventManager.RegisterRoutedEvent(
            "IsVisibleChanged",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(Visible));

        private static readonly DependencyProperty TrackerProperty = DependencyProperty.RegisterAttached(
            "Tracker",
            typeof(VisibilityTracker),
            typeof(Visible),
            new PropertyMetadata(default(VisibilityTracker)));

        internal static readonly RoutedEventArgs IsVisibleChangedEventArgs = new RoutedEventArgs(Visible.IsVisibleChangedEvent);

        internal static void Track(FrameworkElement e)
        {
            if (e.GetValue(TrackerProperty) == null)
            {
                e.SetValue(TrackerProperty, new VisibilityTracker(e));
            }
        }

        private class VisibilityTracker
        {
            public VisibilityTracker(FrameworkElement e)
            {
                e.IsVisibleChanged += OnIsVisibleChanged;
            }

            private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
            {
                ((FrameworkElement)sender).RaiseEvent(IsVisibleChangedEventArgs);
            }
        }
    }
}
