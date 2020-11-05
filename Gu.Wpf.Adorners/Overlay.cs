namespace Gu.Wpf.Adorners
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    /// <summary>
    /// Attached properties for creating overlays.
    /// </summary>
    [StyleTypedProperty(Property = "ContentPresenterStyle", StyleTargetType = typeof(ContentPresenter))]
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
                (d, e) =>
                {
                    UpdateIsVisible(d);
                    UpdateAdorner(d, ContentAdorner.ContentProperty, e.NewValue);
                }));

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> used to display the content of the control.
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.RegisterAttached(
            "ContentTemplate",
            typeof(DataTemplate),
            typeof(Overlay),
            new PropertyMetadata(
                default(DataTemplate),
                (d, e) =>
                {
                    UpdateIsVisible(d);
                    UpdateAdorner(d, ContentAdorner.ContentTemplateProperty, e.NewValue);
                }));

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
            new PropertyMetadata(
                default(DataTemplateSelector),
                (d, e) =>
                {
                    UpdateIsVisible(d);
                    UpdateAdorner(d, ContentAdorner.ContentTemplateSelectorProperty, e.NewValue);
                }));

        /// <summary>
        /// Gets or sets the <see cref="Style"/> for rendering <see cref="ContentProperty"/>.
        /// </summary>
        public static readonly DependencyProperty ContentPresenterStyleProperty = DependencyProperty.RegisterAttached(
            "ContentPresenterStyle",
            typeof(Style),
            typeof(Overlay),
            new PropertyMetadata(
                default(Style),
                (d, e) =>
                {
                    UpdateIsVisible(d);
                    UpdateAdorner(d, ContentAdorner.ContentPresenterStyleProperty, e.NewValue);
                }));

        /// <summary>
        /// Gets or sets visibility of the adorner. Note that setting it to visible does not need to trigger a show.
        /// </summary>
        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.RegisterAttached(
            "Visibility",
            typeof(Visibility),
            typeof(Overlay),
            new PropertyMetadata(
                Visibility.Visible,
                (d, _) => UpdateIsVisible(d)));

        private static readonly DependencyPropertyKey IsVisiblePropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsVisible",
            typeof(bool),
            typeof(Overlay),
            new PropertyMetadata(
                default(bool),
                OnIsVisibleChanged));

        /// <summary>
        /// Gets or sets if the adorner is currently visible.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty = IsVisiblePropertyKey.DependencyProperty;

        private static readonly DependencyProperty ListenerProperty = DependencyProperty.RegisterAttached(
            "Listener",
            typeof(EventListener),
            typeof(Overlay),
            new PropertyMetadata(default(EventListener)));

        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner",
            typeof(ContentAdorner),
            typeof(Overlay),
            new PropertyMetadata(
                default(ContentAdorner),
                (d, e) => ((ContentAdorner?)e.OldValue)?.ClearChild()));

        /// <summary>Helper for setting <see cref="ContentProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="FrameworkElement"/> to set <see cref="ContentProperty"/> on.</param>
        /// <param name="value">Content property value.</param>
        public static void SetContent(FrameworkElement element, object? value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(ContentProperty, value);
        }

        /// <summary>Helper for getting <see cref="ContentProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="FrameworkElement"/> to read <see cref="ContentProperty"/> from.</param>
        /// <returns>Content property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static object? GetContent(FrameworkElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return element.GetValue(ContentProperty);
        }

        /// <summary>Helper for setting <see cref="ContentTemplateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="FrameworkElement"/> to set <see cref="ContentTemplateProperty"/> on.</param>
        /// <param name="value">ContentTemplate property value.</param>
        public static void SetContentTemplate(FrameworkElement element, DataTemplate? value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(ContentTemplateProperty, value);
        }

        /// <summary>Helper for getting <see cref="ContentTemplateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="FrameworkElement"/> to read <see cref="ContentTemplateProperty"/> from.</param>
        /// <returns>ContentTemplate property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static DataTemplate? GetContentTemplate(FrameworkElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (DataTemplate?)element.GetValue(ContentTemplateProperty);
        }

        /// <summary>Helper for setting <see cref="ContentTemplateSelectorProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="FrameworkElement"/> to set <see cref="ContentTemplateSelectorProperty"/> on.</param>
        /// <param name="value">ContentTemplateSelector property value.</param>
        public static void SetContentTemplateSelector(FrameworkElement element, DataTemplateSelector? value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(ContentTemplateSelectorProperty, value);
        }

        /// <summary>Helper for getting <see cref="ContentTemplateSelectorProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="FrameworkElement"/> to read <see cref="ContentTemplateSelectorProperty"/> from.</param>
        /// <returns>ContentTemplateSelector property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static DataTemplateSelector? GetContentTemplateSelector(FrameworkElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (DataTemplateSelector?)element.GetValue(ContentTemplateSelectorProperty);
        }

        /// <summary>Helper for setting <see cref="ContentPresenterStyleProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="FrameworkElement"/> to set <see cref="ContentPresenterStyleProperty"/> on.</param>
        /// <param name="value">ContentPresenterStyle property value.</param>
        public static void SetContentPresenterStyle(FrameworkElement element, Style? value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(ContentPresenterStyleProperty, value);
        }

        /// <summary>Helper for getting <see cref="ContentPresenterStyleProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="FrameworkElement"/> to read <see cref="ContentPresenterStyleProperty"/> from.</param>
        /// <returns>ContentPresenterStyle property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static Style? GetContentPresenterStyle(FrameworkElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (Style?)element.GetValue(ContentPresenterStyleProperty);
        }

        /// <summary>Helper for setting <see cref="VisibilityProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="FrameworkElement"/> to set <see cref="VisibilityProperty"/> on.</param>
        /// <param name="value">Visibility property value.</param>
        public static void SetVisibility(FrameworkElement element, Visibility value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(VisibilityProperty, value);
        }

        /// <summary>Helper for getting <see cref="VisibilityProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="FrameworkElement"/> to read <see cref="VisibilityProperty"/> from.</param>
        /// <returns>Visibility property value.</returns>
        public static Visibility GetVisibility(FrameworkElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (Visibility)element.GetValue(VisibilityProperty);
        }

        private static void SetIsVisible(this FrameworkElement element, bool value)
        {
            element.SetValue(IsVisiblePropertyKey, value);
        }

        /// <summary>Helper for getting <see cref="IsVisibleProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="FrameworkElement"/> to read <see cref="IsVisibleProperty"/> from.</param>
        /// <returns>IsVisible property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static bool GetIsVisible(this FrameworkElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (bool)element.GetValue(IsVisibleProperty);
        }

#pragma warning restore SA1202 // Elements must be ordered by access

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (Equals(e.NewValue, true))
            {
                if (d.GetValue(AdornerProperty) is null)
                {
                    var adorner = new ContentAdorner(AdornedElement())
                    {
                        Content = d.GetValue(ContentProperty),
                        ContentTemplate = (DataTemplate)d.GetValue(ContentTemplateProperty),
                        ContentTemplateSelector = (DataTemplateSelector)d.GetValue(ContentTemplateSelectorProperty),
                        ContentPresenterStyle = (Style)d.GetValue(ContentPresenterStyleProperty),
                    };
                    d.SetCurrentValue(AdornerProperty, adorner);
                    AdornerService.Show(adorner);
                }
                else
                {
                    Debug.Assert(condition: false, message: $"Element {d} already has an info adorner.");
                }
            }
            else if (d.GetValue(AdornerProperty) is ContentAdorner adorner)
            {
                AdornerService.Remove(adorner);
                d.ClearValue(AdornerProperty);
            }

            UIElement AdornedElement()
            {
                if (d is Window window &&
                    window.FirstRecursiveVisualChild<AdornerDecorator>() is { Child: { } adornerDecoratorChild })
                {
                    return adornerDecoratorChild;
                }

                return (UIElement)d;
            }
        }

        private static void UpdateIsVisible(DependencyObject d)
        {
            if (d is FrameworkElement element)
            {
                if (d.GetValue(ListenerProperty) is null)
                {
                    d.SetCurrentValue(ListenerProperty, new EventListener(element));
                }

                if (element.IsVisible &&
                    element.IsLoaded() &&
                    GetVisibility(element) == Visibility.Visible)
                {
                    element.SetIsVisible(true);
                }
                else
                {
                    d.SetValue(IsVisiblePropertyKey, false);
                }
            }
        }

        private static void UpdateAdorner(DependencyObject d, DependencyProperty property, object value)
        {
            if (d.GetValue(AdornerProperty) is ContentAdorner adorner)
            {
                adorner.SetCurrentValue(property, value);
            }
        }

        private class EventListener
        {
            internal EventListener(FrameworkElement element)
            {
                IsVisibleChangedEventManager.UpdateHandler(element, OnAdornedElementChanged);
                LoadedEventManager.UpdateHandler(element, OnAdornedElementChanged);
                UnloadedEventManager.UpdateHandler(element, OnAdornedElementChanged);
                SizeChangedEventManager.UpdateHandler(element, OnSizeChanged);

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
                static void OnAdornedElementChanged(object? sender, EventArgs _)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
                {
                    if (sender is FrameworkElement e)
                    {
                        UpdateIsVisible(e);
                    }
                }

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
                static void OnSizeChanged(object? sender, RoutedEventArgs _)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
                {
                    if (sender is FrameworkElement e)
                    {
                        (e.GetValue(AdornerProperty) as ContentAdorner)?.InvalidateMeasure();
                    }
                }
            }
        }
    }
}
