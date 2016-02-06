namespace Gu.Wpf.Adorners
{
    using System.Windows;
    using System.Windows.Controls;

    public static class Overlay
    {
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
            new PropertyMetadata(
                default(DataTemplate),
                OnContentTemplateChanged));

        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.RegisterAttached(
            "ContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Overlay),
            new PropertyMetadata(
                default(DataTemplateSelector),
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
            typeof(bool),
            typeof(Overlay),
            new PropertyMetadata(
                default(bool), 
                OnIsVisibleChanged));

        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner",
            typeof(ContentAdorner),
            typeof(Overlay),
            new PropertyMetadata(default(ContentAdorner), OnAdornerChanged));

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

        public static object GetContent(UIElement element)
        {
            return (object)element.GetValue(ContentProperty);
        }

        public static void SetContentTemplate(DependencyObject element, DataTemplate value)
        {
            element.SetValue(ContentTemplateProperty, value);
        }

        public static DataTemplate GetContentTemplate(DependencyObject element)
        {
            return (DataTemplate)element.GetValue(ContentTemplateProperty);
        }

        public static void SetContentTemplateSelector(DependencyObject element, DataTemplateSelector value)
        {
            element.SetValue(ContentTemplateSelectorProperty, value);
        }

        public static DataTemplateSelector GetContentTemplateSelector(DependencyObject element)
        {
            return (DataTemplateSelector)element.GetValue(ContentTemplateSelectorProperty);
        }

        public static void SetContentPresenterStyle(this UIElement element, Style value)
        {
            element.SetValue(ContentPresenterStyleProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Style GetContentPresenterStyle(this UIElement element)
        {
            return (Style)element.GetValue(ContentPresenterStyleProperty);
        }

        public static void SetIsVisible(UIElement element, bool value)
        {
            element.SetValue(IsVisibleProperty, value);
        }

        public static bool GetIsVisible(UIElement element)
        {
            return (bool)element.GetValue(IsVisibleProperty);
        }

        private static void SetAdorner(this UIElement element, ContentAdorner value)
        {
            element.SetValue(AdornerProperty, value);
        }

        private static ContentAdorner GetAdorner(this UIElement element)
        {
            return (ContentAdorner)element.GetValue(AdornerProperty);
        }

        private static void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            UpdateOverlayVisibility(sender as UIElement);
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateOverlayVisibility(sender as UIElement);
        }

        private static void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateOverlayVisibility(sender as UIElement);
        }

        private static void OnIsVisibleChanged(object sender, RoutedEventArgs e)
        {
            UpdateOverlayVisibility(sender as UIElement);
        }

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (UIElement)d;
            Visible.Track(element);
            if (e.NewValue == null)
            {
                var adorner = element.GetAdorner();
                if (adorner != null)
                {
                    AdornerService.Remove(adorner);
                }

                element.ClearValue(AdornerProperty);
            }
            else
            {
                var adorner = element.GetAdorner();
                if (adorner == null)
                {
                    adorner = new ContentAdorner(element);
                    Visible.Track(element);

                    element.SetAdorner(adorner);
                    UpdateOverlayVisibility(element);
                }

                adorner.Content = e.NewValue;
                adorner.ContentTemplate = GetContentTemplate(element);
                adorner.ContentTemplateSelector = GetContentTemplateSelector(element);
                adorner.ContentPresenterStyle = GetContentPresenterStyle(element);
            }
        }

        private static void OnContentTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adorner = (ContentAdorner)(d as UIElement)?.GetAdorner();
            if (adorner != null)
            {
                adorner.ContentTemplate = GetContentTemplate(d);
            }
        }

        private static void OnContentTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adorner = (ContentAdorner)(d as UIElement)?.GetAdorner();
            if (adorner != null)
            {
                adorner.ContentTemplate = GetContentTemplate(d);
            }
        }

        private static void OnContentPresenterStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;
            var adorner = element?.GetAdorner();
            if (adorner != null)
            {
                adorner.ContentPresenterStyle = GetContentPresenterStyle(element);
            }
        }

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateOverlayVisibility(d as UIElement);
        }

        private static void OnAdornerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WatermarkAdorner)e.OldValue)?.ClearChild();
        }

        private static void UpdateOverlayVisibility(UIElement element)
        {
            var adorner = element?.GetAdorner();
            if (adorner == null)
            {
                return;
            }

            if (!element.IsVisible)
            {
                AdornerService.Remove(adorner);
                return;
            }

            if (GetIsVisible(element))
            {
                AdornerService.Show(adorner);
            }
            else
            {
                AdornerService.Remove(adorner);
            }
        }
    }
}
