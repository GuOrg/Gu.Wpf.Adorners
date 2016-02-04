namespace Gu.Wpf.Adorners
{
    using System.Windows;
    using System.Windows.Documents;

    public static class Overlay
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached(
            "Content",
            typeof(object),
            typeof(Overlay),
            new PropertyMetadata(default(object), OnContentChanged));

        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached(
            "IsVisible", 
            typeof (bool), 
            typeof (Overlay), 
            new PropertyMetadata(default(bool), OnIsVisibleChanged));

        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner",
            typeof (Adorner),
            typeof (Overlay), 
            new PropertyMetadata(default(Adorner)));

        public static void SetContent(DependencyObject element, object value)
        {
            element.SetValue(ContentProperty, value);
        }

        public static object GetContent(UIElement element)
        {
            return (object)element.GetValue(ContentProperty);
        }

        public static void SetIsVisible(UIElement element, bool value)
        {
            element.SetValue(IsVisibleProperty, value);
        }

        public static bool GetIsVisible(UIElement element)
        {
            return (bool)element.GetValue(IsVisibleProperty);
        }

        private static void SetAdorner(this UIElement element, Adorner value)
        {
            element.SetValue(AdornerProperty, value);
        }

        private static Adorner GetAdorner(this UIElement element)
        {
            return (Adorner)element.GetValue(AdornerProperty);
        }

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
