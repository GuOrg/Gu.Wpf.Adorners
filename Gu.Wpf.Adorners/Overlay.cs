namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    /// <summary>
    /// Attached properties for creating overlays.
    /// </summary>
    public static class Overlay
    {
#pragma warning disable SA1202 // Elements must be ordered by access
        /// <summary>
        /// Gets or sets the content is the data used to generate the child elements of this control.
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached(
            "Content",
            typeof(object),
            typeof(Overlay),
            new PropertyMetadata(
                default(object),
                OnContentChanged));

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> used to display the content of the control.
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.RegisterAttached(
            "ContentTemplate",
            typeof(DataTemplate),
            typeof(Overlay),
            new FrameworkPropertyMetadata(
                default(DataTemplate),
                FrameworkPropertyMetadataOptions.Inherits,
                OnContentTemplateChanged));

        /// <summary>
        /// Gets or sets the <see cref="DataTemplateSelector"/> that allows the application writer to provide custom logic
        /// for choosing the template used to display the content of the control.
        /// </summary>
        /// <remarks>
        /// This property is ignored if <seealso cref="ContentTemplateProperty"/> is set.
        /// </remarks>
        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.RegisterAttached(
            "ContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Overlay),
            new FrameworkPropertyMetadata(
                default(DataTemplateSelector),
                 FrameworkPropertyMetadataOptions.Inherits,
                OnContentTemplateSelectorChanged));

        /// <summary>
        /// Gets or sets the <see cref="Style"/> for rendering <see cref="ContentProperty"/>
        /// </summary>
        public static readonly DependencyProperty ContentPresenterStyleProperty = DependencyProperty.RegisterAttached(
            "ContentPresenterStyle",
            typeof(Style),
            typeof(Overlay),
            new FrameworkPropertyMetadata(
                default(Style),
                FrameworkPropertyMetadataOptions.Inherits,
                OnContentPresenterStyleChanged));

        /// <summary>
        /// Gets or sets visibility of the adorner. Note that setting it to visible does not need to trigger a show.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached(
            "IsVisible",
            typeof(bool?),
            typeof(Overlay),
            new PropertyMetadata(
                default(bool?),
                (d, e) => UpdateIsShowing(d)));

        private static readonly DependencyPropertyKey IsShowingPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsShowing",
            typeof(bool),
            typeof(Overlay),
            new PropertyMetadata(
                default(bool),
                OnIsShowingChanged));

        /// <summary>
        /// Gets or sets if the adorner is currently visible
        /// </summary>
        public static readonly DependencyProperty IsShowingProperty = IsShowingPropertyKey.DependencyProperty;

        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner",
            typeof(ContentAdorner),
            typeof(Overlay),
            new PropertyMetadata(
                default(ContentAdorner),
                OnAdornerChanged));

        /// <summary>Helper for setting <see cref="ContentProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="ContentProperty"/> on.</param>
        /// <param name="value">Content property value.</param>
        public static void SetContent(DependencyObject element, object value)
        {
            element.SetValue(ContentProperty, value);
        }

        /// <summary>Helper for getting <see cref="ContentProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="ContentProperty"/> from.</param>
        /// <returns>Content property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static object GetContent(DependencyObject element)
        {
            return element.GetValue(ContentProperty);
        }

        /// <summary>Helper for setting <see cref="ContentTemplateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="ContentTemplateProperty"/> on.</param>
        /// <param name="value">ContentTemplate property value.</param>
        public static void SetContentTemplate(DependencyObject element, DataTemplate value)
        {
            element.SetValue(ContentTemplateProperty, value);
        }

        /// <summary>Helper for getting <see cref="ContentTemplateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="ContentTemplateProperty"/> from.</param>
        /// <returns>ContentTemplate property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplate GetContentTemplate(DependencyObject element)
        {
            return (DataTemplate)element.GetValue(ContentTemplateProperty);
        }

        /// <summary>Helper for setting <see cref="ContentTemplateSelectorProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="ContentTemplateSelectorProperty"/> on.</param>
        /// <param name="value">ContentTemplateSelector property value.</param>
        public static void SetContentTemplateSelector(DependencyObject element, DataTemplateSelector value)
        {
            element.SetValue(ContentTemplateSelectorProperty, value);
        }

        /// <summary>Helper for getting <see cref="ContentTemplateSelectorProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="ContentTemplateSelectorProperty"/> from.</param>
        /// <returns>ContentTemplateSelector property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static DataTemplateSelector GetContentTemplateSelector(DependencyObject element)
        {
            return (DataTemplateSelector)element.GetValue(ContentTemplateSelectorProperty);
        }

        /// <summary>Helper for setting <see cref="ContentPresenterStyleProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="ContentPresenterStyleProperty"/> on.</param>
        /// <param name="value">ContentPresenterStyle property value.</param>
        public static void SetContentPresenterStyle(DependencyObject element, Style value)
        {
            element.SetValue(ContentPresenterStyleProperty, value);
        }

        /// <summary>Helper for getting <see cref="ContentPresenterStyleProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="ContentPresenterStyleProperty"/> from.</param>
        /// <returns>ContentPresenterStyle property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Style GetContentPresenterStyle(DependencyObject element)
        {
            return (Style)element.GetValue(ContentPresenterStyleProperty);
        }

        /// <summary>Helper for setting <see cref="IsVisibleProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="IsVisibleProperty"/> on.</param>
        /// <param name="value">IsVisible property value.</param>
        public static void SetIsVisible(DependencyObject element, bool? value)
        {
            element.SetValue(IsVisibleProperty, value);
        }

        /// <summary>Helper for getting <see cref="IsVisibleProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="IsVisibleProperty"/> from.</param>
        /// <returns>IsVisible property value.</returns>
        public static bool? GetIsVisible(DependencyObject element)
        {
            return (bool?)element.GetValue(IsVisibleProperty);
        }

        private static void SetIsShowing(this DependencyObject element, bool value)
        {
            element.SetValue(IsShowingPropertyKey, value);
        }

        /// <summary>Helper for getting <see cref="IsShowingProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="IsShowingProperty"/> from.</param>
        /// <returns>IsShowing property value.</returns>
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

        private static void OnAdornedElementChanged(object sender, EventArgs e) => UpdateIsShowing(sender as DependencyObject);

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateHandlers(d);
            UpdateIsShowing(d);
        }

        private static void OnContentTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateHandlers(d);
            var adorner = d?.GetAdorner();
            adorner?.SetCurrentValue(ContentAdorner.ContentTemplateProperty, GetContentTemplate(d));
            UpdateIsShowing(d);
        }

        private static void OnContentTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateHandlers(d);
            var adorner = d?.GetAdorner();
            adorner?.SetCurrentValue(ContentAdorner.ContentTemplateProperty, GetContentTemplate(d));
            UpdateIsShowing(d);
        }

        private static void OnContentPresenterStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateHandlers(d);
            var adorner = d?.GetAdorner();
            adorner?.SetCurrentValue(ContentAdorner.ContentPresenterStyleProperty, GetContentPresenterStyle(d));
        }

        private static void UpdateHandlers(DependencyObject d)
        {
            IsVisibleChangedEventManager.UpdateHandler((UIElement)d, OnAdornedElementChanged);
            LoadedEventManager.UpdateHandler((UIElement)d, OnAdornedElementChanged);
            UnloadedEventManager.UpdateHandler((UIElement)d, OnAdornedElementChanged);
            SizeChangedEventManager.UpdateHandler((FrameworkElement)d, OnSizeChanged);
        }

        private static void OnIsShowingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (UIElement)d;
            if (Equals(e.NewValue, true))
            {
                var adorner = element.GetAdorner();
                if (adorner == null)
                {
                    var adornedElement = element is Window
                        ? element.FirstOrDefaultRecursiveVisualChild<AdornerDecorator>()?.Child
                        : element;

                    adorner = new ContentAdorner(adornedElement);
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

        private static void UpdateIsShowing(DependencyObject o)
        {
            if (o is UIElement element)
            {
                if (!element.IsVisible ||
                    !element.IsLoaded())
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
