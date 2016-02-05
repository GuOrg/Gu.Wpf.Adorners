namespace Gu.Wpf.Adorners.Demo
{
    using System.Windows;
    using System.Windows.Controls;

    public static class Info
    {
        public static readonly DependencyProperty InfoTemplateProperty = DependencyProperty.RegisterAttached(
            "InfoTemplate",
            typeof(ControlTemplate),
            typeof(Info),
            new PropertyMetadata(default(ControlTemplate)));

        public static readonly DependencyProperty InfoVisibleProperty = DependencyProperty.RegisterAttached(
            "InfoVisible",
            typeof(bool),
            typeof(Info),
            new PropertyMetadata(
                default(bool),
                OnIsVisibleChanged));

        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner",
            typeof(TemplatedAdorner),
            typeof(Info),
            new PropertyMetadata(default(TemplatedAdorner)));

        public static void SetInfoTemplate(DependencyObject element, ControlTemplate value)
        {
            element.SetValue(InfoTemplateProperty, value);
        }

        public static ControlTemplate GetInfoTemplate(DependencyObject element)
        {
            return (ControlTemplate)element.GetValue(InfoTemplateProperty);
        }

        public static void SetInfoVisible(DependencyObject element, bool value)
        {
            element.SetValue(InfoVisibleProperty, value);
        }

        public static bool GetInfoVisible(DependencyObject element)
        {
            return (bool)element.GetValue(InfoVisibleProperty);
        }

        private static void SetAdorner(this DependencyObject element, TemplatedAdorner value)
        {
            element.SetValue(AdornerProperty, value);
        }

        private static TemplatedAdorner GetAdorner(this DependencyObject element)
        {
            return (TemplatedAdorner)element.GetValue(AdornerProperty);
        }

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adorner = d.GetAdorner();
            if (Equals(e.NewValue, true))
            {
                if (adorner == null)
                {
                    adorner = new TemplatedAdorner((UIElement)d, GetInfoTemplate(d));
                }

                AdornerService.Show(adorner);
            }
            else
            {
                if (adorner != null)
                {
                    AdornerService.Remove(adorner);
                }
            }
        }
    }
}