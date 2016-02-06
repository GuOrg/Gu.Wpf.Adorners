namespace Gu.Wpf.Adorners
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Markup.Primitives;

    // Failed attempt at making a templated adorner
    // would be easy to just write it but it would be nice if AdornedElementPlaceHolder could be used like for validation error template.
    public static class Info
    {
        public static readonly DependencyProperty TemplateProperty = DependencyProperty.RegisterAttached(
            "Template",
            typeof(ControlTemplate),
            typeof(Info),
            new PropertyMetadata(
                default(ControlTemplate), 
                OnTemplateChanged));

        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached(
            "IsVisible",
            typeof(bool?),
            typeof(Info),
            new PropertyMetadata(
                default(bool?),
                OnIsVisibleChanged));

        private static readonly DependencyPropertyKey IsShowingPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsShowing",
            typeof(bool),
            typeof(Info),
            new PropertyMetadata(
                default(bool),
                OnIsShowingChanged));

        public static readonly DependencyProperty IsShowingProperty = IsShowingPropertyKey.DependencyProperty;

        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner",
            typeof(Adorner),
            typeof(Info),
            new PropertyMetadata(
                default(Adorner),
                OnAdornerChanged));

        static Info()
        {
            EventManager.RegisterClassHandler(typeof(UIElement), FrameworkElement.SizeChangedEvent, new RoutedEventHandler(OnSizeChanged));
            Loaded.Track();
            EventManager.RegisterClassHandler(typeof(UIElement), FrameworkElement.LoadedEvent, new RoutedEventHandler(OnLoaded));
            EventManager.RegisterClassHandler(typeof(UIElement), FrameworkElement.UnloadedEvent, new RoutedEventHandler(OnUnLoaded));
            EventManager.RegisterClassHandler(typeof(UIElement), Visible.IsVisibleChangedEvent, new RoutedEventHandler(OnIsVisibleChanged));
        }

        public static void SetTemplate(DependencyObject element, ControlTemplate value)
        {
            element.SetValue(TemplateProperty, value);
        }

        public static ControlTemplate GetTemplate(DependencyObject element)
        {
            return (ControlTemplate)element.GetValue(TemplateProperty);
        }

        public static void SetIsVisible(DependencyObject element, bool? value)
        {
            element.SetValue(IsVisibleProperty, value);
        }

        public static bool? GetIsVisible(DependencyObject element)
        {
            return (bool?)element.GetValue(IsVisibleProperty);
        }

        private static void SetIsShowing(this DependencyObject element, bool value)
        {
            element.SetValue(IsShowingPropertyKey, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetIsShowing(this DependencyObject element)
        {
            return (bool)element.GetValue(IsShowingProperty);
        }

        private static void SetAdorner(this DependencyObject element, Adorner value)
        {
            element.SetValue(AdornerProperty, value);
        }

        private static Adorner GetAdorner(this DependencyObject element)
        {
            return (Adorner)element.GetValue(AdornerProperty);
        }

        private static void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            var element = sender as DependencyObject;
            element?.GetAdorner()?.InvalidateMeasure();
            UpdateIsShowing(element);
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateIsShowing(sender as DependencyObject);
        }

        private static void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateIsShowing(sender as DependencyObject);
        }

        private static void OnIsVisibleChanged(object sender, RoutedEventArgs e)
        {
            UpdateIsShowing(sender as DependencyObject);
        }

        private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Visible.Track(d as UIElement);
            UpdateIsShowing(d);
        }

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateIsShowing(d);
        }

        private static void OnIsShowingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (UIElement)d;
            if (Equals(e.NewValue, true))
            {
                var adorner = element.GetAdorner();
                if (adorner == null)
                {
                    adorner = new TemplatedAdorner(element, (ControlTemplate)element.GetValue(TemplateProperty));
                    element.SetAdorner(adorner);
                }

                AdornerService.Show(adorner);
            }
            else
            {
                var adorner = element.GetAdorner();
                if (adorner != null)
                {
                    AdornerService.Remove(adorner);
                }

                element.ClearValue(AdornerProperty);
            }
        }

        private static void OnAdornerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TemplatedAdorner)e.OldValue)?.ClearChild();
        }

        private static void UpdateIsShowing(DependencyObject element)
        {
            if (element == null)
            {
                return;
            }

            if (!Visible.IsVisible(element) ||
                !Loaded.IsLoaded(element))
            {
                element.SetIsShowing(false);
                return;
            }

            var template = GetTemplate(element);
            var isVisible = GetIsVisible(element);
            if (template != null && isVisible != null)
            {
                element.SetIsShowing(isVisible.Value);
                return;
            }

            element.SetIsShowing(template != null);
        }
    }
}