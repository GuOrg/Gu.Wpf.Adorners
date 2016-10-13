namespace Gu.Wpf.Adorners
{
    using System.Windows;
    using System.Windows.Controls;

    public static class Overlay
    {
#pragma warning disable SA1202 // Elements must be ordered by access

        public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached(
            "Content",
            typeof(object),
            typeof(Overlay),
            new PropertyMetadata(
                default(object),
                OnContentChanged));

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.RegisterAttached(
            "ContentTemplate",
            typeof(DataTemplate),
            typeof(Overlay),
            new FrameworkPropertyMetadata(
                default(DataTemplate),
                FrameworkPropertyMetadataOptions.Inherits,
                OnContentTemplateChanged));

        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.RegisterAttached(
            "ContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Overlay),
            new FrameworkPropertyMetadata(
                default(DataTemplateSelector),
                 FrameworkPropertyMetadataOptions.Inherits,
                OnContentTemplateSelectorChanged));

        public static readonly DependencyProperty ContentPresenterStyleProperty = DependencyProperty.RegisterAttached(
            "ContentPresenterStyle",
            typeof(Style),
            typeof(Overlay),
            new FrameworkPropertyMetadata(
                default(Style),
                FrameworkPropertyMetadataOptions.Inherits,
                OnContentPresenterStyleChanged));

        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached(
            "IsVisible",
            typeof(bool?),
            typeof(Overlay),
            new PropertyMetadata(
                default(bool?),
                OnIsVisibleChanged));

        private static readonly DependencyPropertyKey IsShowingPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsShowing",
            typeof(bool),
            typeof(Overlay),
            new PropertyMetadata(
                default(bool),
                OnIsShowingChanged));

        public static readonly DependencyProperty IsShowingProperty = IsShowingPropertyKey.DependencyProperty;

        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner",
            typeof(ContentAdorner),
            typeof(Overlay),
            new PropertyMetadata(
                default(ContentAdorner),
                OnAdornerChanged));

        static Overlay()
        {
            EventManager.RegisterClassHandler(typeof(UIElement), FrameworkElement.SizeChangedEvent, new RoutedEventHandler(OnSizeChanged));
            Loaded.Track();
            EventManager.RegisterClassHandler(typeof(UIElement), FrameworkElement.LoadedEvent, new RoutedEventHandler(OnLoaded));
            EventManager.RegisterClassHandler(typeof(UIElement), FrameworkElement.UnloadedEvent, new RoutedEventHandler(OnUnLoaded));
            EventManager.RegisterClassHandler(typeof(UIElement), Visible.IsVisibleChangedEvent, new RoutedEventHandler(OnIsVisibleChanged));
        }

        public static void SetContent(DependencyObject element, object value)
        {
            element.SetValue(ContentProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static object GetContent(DependencyObject element)
        {
            return element.GetValue(ContentProperty);
        }

        public static void SetContentTemplate(DependencyObject element, DataTemplate value)
        {
            element.SetValue(ContentTemplateProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplate GetContentTemplate(DependencyObject element)
        {
            return (DataTemplate)element.GetValue(ContentTemplateProperty);
        }

        public static void SetContentTemplateSelector(DependencyObject element, DataTemplateSelector value)
        {
            element.SetValue(ContentTemplateSelectorProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplateSelector GetContentTemplateSelector(DependencyObject element)
        {
            return (DataTemplateSelector)element.GetValue(ContentTemplateSelectorProperty);
        }

        public static void SetContentPresenterStyle(DependencyObject element, Style value)
        {
            element.SetValue(ContentPresenterStyleProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Style GetContentPresenterStyle(DependencyObject element)
        {
            return (Style)element.GetValue(ContentPresenterStyleProperty);
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

        private static void SetAdorner(this DependencyObject element, ContentAdorner value)
        {
            element.SetValue(AdornerProperty, value);
        }

        private static ContentAdorner GetAdorner(this DependencyObject element)
        {
            return (ContentAdorner)element.GetValue(AdornerProperty);
        }

#pragma warning restore SA1202 // Elements must be ordered by access

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

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Visible.Track(d as UIElement);
            UpdateIsShowing(d);
        }

        private static void OnContentTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adorner = d?.GetAdorner();
            adorner?.SetCurrentValue(ContentAdorner.ContentTemplateProperty, GetContentTemplate(d));
            UpdateIsShowing(d);
        }

        private static void OnContentTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adorner = d?.GetAdorner();
            adorner?.SetCurrentValue(ContentAdorner.ContentTemplateProperty, GetContentTemplate(d));
            UpdateIsShowing(d);
        }

        private static void OnContentPresenterStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adorner = d?.GetAdorner();
            adorner?.SetCurrentValue(ContentAdorner.ContentPresenterStyleProperty, GetContentPresenterStyle(d));
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
                    adorner = new ContentAdorner(element);
                    element.SetAdorner(adorner);
                }

                SetIfNotNull(d, ContentProperty, adorner, ContentAdorner.ContentProperty);
                SetIfNotNull(d, ContentTemplateProperty, adorner, ContentAdorner.ContentTemplateProperty);
                SetIfNotNull(d, ContentTemplateSelectorProperty, adorner, ContentAdorner.ContentTemplateSelectorProperty);
                SetIfNotNull(d, ContentPresenterStyleProperty, adorner, ContentAdorner.ContentPresenterStyleProperty);
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
            ((ContentAdorner)e.OldValue)?.ClearChild();
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

            var isVisible = GetIsVisible(element);
            if (isVisible != null)
            {
                element.SetIsShowing(isVisible.Value);
                return;
            }

            var content = GetContent(element);
            element.SetIsShowing(content != null);
        }

        private static void SetIfNotNull(
            DependencyObject source,
            DependencyProperty sourceProperty,
            ContentAdorner adorner,
            DependencyProperty adornerProperty)
        {
            var value = source.GetValue(sourceProperty);
            if (value != null)
            {
                adorner.SetValue(adornerProperty, value);
            }
        }
    }
}
