namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Threading;

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
            typeof(Adorner),
            typeof(Info),
            new PropertyMetadata(default(Adorner)));

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

        private static void SetAdorner(this DependencyObject element, Adorner value)
        {
            element.SetValue(AdornerProperty, value);
        }

        private static Adorner GetAdorner(this DependencyObject element)
        {
            return (Adorner)element.GetValue(AdornerProperty);
        }

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateVisibility((UIElement) d);
        }

        private static void UpdateVisibility(UIElement adornedElement)
        {
            var adorner = adornedElement.GetAdorner();
            if (adornedElement.IsVisible)
            {
                if (adorner == null)
                {
                    var template = GetInfoTemplate(adornedElement);
                    adorner = TemplatedAdornerFactory.Create(adornedElement, template);
                }

                if (adorner == null)
                {
                    return;
                  //adornedElement.Dispatcher.BeginInvoke(DispatcherPriority.Loaded,new Action<UIElement>(a))
                }

                adornedElement.SetAdorner(adorner);
                AdornerService.Show(adorner);
            }

            else if(adorner != null)
            {
                AdornerService.Remove(adorner);
            }
        }

        //private static Adorner 

        //private static object CreateAdornerOperation(object arg)
        //{
        //    var args = (object[])arg;
        //    var adorner = (Adorner)args[0];
        //    Show(adorner, false);
        //    return null;
        //}
    }
}