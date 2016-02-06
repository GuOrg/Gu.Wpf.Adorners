namespace Gu.Wpf.Adorners
{
    using System.Windows;
    using System.Windows.Data;

    internal static class Visible
    {
        internal static readonly RoutedEvent IsVisibleChangedEvent = EventManager.RegisterRoutedEvent(
            "IsVisibleChanged",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(Visible));

        private static readonly DependencyProperty IsVisibleProxyProperty = DependencyProperty.RegisterAttached(
            "IsVisibleProxy",
            typeof(bool?),
            typeof(Visible),
            new PropertyMetadata(default(bool?), OnIsVisibleChanged));

        private static readonly RoutedEventArgs IsVisibleChangedEventArgs = new RoutedEventArgs(Visible.IsVisibleChangedEvent);

        internal static void Track(UIElement e)
        {
            if (e == null)
            {
                return;
            }

            if (BindingOperations.GetBindingExpression(e, IsVisibleProxyProperty) == null)
            {
                e.Bind(IsVisibleProxyProperty)
                 .OneWayTo(e, UIElement.IsVisibleProperty);
            }
        }

        internal static bool IsVisible(DependencyObject element)
        {
            var fe = element as UIElement;
            if (fe != null)
            {
                return fe.IsVisible;
            }

            return false;
        }

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((UIElement)d).RaiseEvent(IsVisibleChangedEventArgs);
        }
    }
}
