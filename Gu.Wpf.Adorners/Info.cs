namespace Gu.Wpf.Adorners
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    /// <summary>
    /// For showing adorners similar to validation errors.
    /// </summary>
    public static class Info
    {
#pragma warning disable SA1202 // Elements must be ordered by access
        /// <summary>
        /// Template used to generate info feedback on the AdornerLayer.
        /// </summary>
        public static readonly DependencyProperty TemplateProperty = DependencyProperty.RegisterAttached(
            "Template",
            typeof(ControlTemplate),
            typeof(Info),
            new PropertyMetadata(
                default(ControlTemplate),
                OnTemplateChanged));

        /// <summary>
        /// Gets or sets visibility of the adorner.
        /// Note that setting it to visible does not need to mean it will be rendered. This can happen if the adorned element is collapsed for example.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached(
            "IsVisible",
            typeof(bool?),
            typeof(Info),
            new PropertyMetadata(
                default(bool?),
                (d, e) => OnAdornedElementChanged(d, e)));

        private static readonly DependencyPropertyKey IsShowingPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsShowing",
            typeof(bool),
            typeof(Info),
            new PropertyMetadata(
                default(bool),
                OnIsShowingChanged));

        /// <summary>
        /// Gets or sets if the adorner is currently visible.
        /// </summary>
        public static readonly DependencyProperty IsShowingProperty = IsShowingPropertyKey.DependencyProperty;

        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner",
            typeof(Adorner),
            typeof(Info),
            new PropertyMetadata(
                default(Adorner),
                (d, e) => ((Adorner)e.OldValue)?.ClearTemplatedAdornerChild()));

        /// <summary>Helper for setting <see cref="TemplateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="TemplateProperty"/> on.</param>
        /// <param name="value">Template property value.</param>
        public static void SetTemplate(DependencyObject element, ControlTemplate value)
        {
            element.SetValue(TemplateProperty, value);
        }

        /// <summary>Helper for getting <see cref="TemplateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="TemplateProperty"/> from.</param>
        /// <returns>Template property value.</returns>
        public static ControlTemplate GetTemplate(DependencyObject element)
        {
            return (ControlTemplate)element.GetValue(TemplateProperty);
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

        private static void SetAdorner(this DependencyObject element, Adorner value)
        {
            element.SetValue(AdornerProperty, value);
        }

        private static Adorner GetAdorner(this DependencyObject element)
        {
            return (Adorner)element.GetValue(AdornerProperty);
        }

#pragma warning restore SA1202 // Elements must be ordered by access

        private static void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            if (sender is UIElement adornedElement)
            {
                adornedElement.GetAdorner()?.InvalidateMeasure();
                UpdateIsShowing(adornedElement);
            }
        }

        private static void OnAdornedElementChanged(object sender, object _)
        {
            if (sender is UIElement adornedElement)
            {
                UpdateIsShowing(adornedElement);
            }
        }

        private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement adornedElement)
            {
                IsVisibleChangedEventManager.UpdateHandler(adornedElement, OnAdornedElementChanged);
                LoadedEventManager.UpdateHandler(adornedElement, OnAdornedElementChanged);
                UnloadedEventManager.UpdateHandler(adornedElement, OnAdornedElementChanged);
                SizeChangedEventManager.UpdateHandler(adornedElement, OnSizeChanged);
                UpdateIsShowing(adornedElement);
            }
        }

        private static void OnIsShowingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (Equals(e.NewValue, true) &&
                d is UIElement element &&
                d.GetValue(TemplateProperty) is ControlTemplate template)
            {
                if (d.GetValue(AdornerProperty) is null)
                {
                    var adorner = TemplatedAdorner.Create(element, template);
                    d.SetCurrentValue(AdornerProperty, adorner);
                    AdornerService.Show(adorner);
                }
                else
                {
                    Debug.Assert(condition: false, message: $"Element {d} already has an info adorner.");
                }
            }
            else if (d.GetValue(AdornerProperty) is Adorner adorner)
            {
                AdornerService.Remove(adorner);
                d.ClearValue(AdornerProperty);
            }
        }

        private static void UpdateIsShowing(UIElement element)
        {
            if (element.IsVisible &&
                element.IsLoaded() &&
                GetTemplate(element) is { } &&
                GetIsVisible(element) == true)
            {
                element.SetIsShowing(true);
                return;
            }
            else
            {
                element.SetIsShowing(false);
            }
        }
    }
}
