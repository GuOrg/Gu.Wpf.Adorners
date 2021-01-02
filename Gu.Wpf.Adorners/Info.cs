namespace Gu.Wpf.Adorners
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    /// <summary>
    /// For showing adorners similar to validation errors.
    /// </summary>
    public static class Info
    {
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
        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.RegisterAttached(
            "Visibility",
            typeof(Visibility),
            typeof(Info),
            new PropertyMetadata(
                Visibility.Visible,
                (d, e) => OnAdornedElementChanged(d, e)));

        private static readonly DependencyPropertyKey IsVisiblePropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsVisible",
            typeof(bool),
            typeof(Info),
            new PropertyMetadata(
                default(bool),
                OnIsVisibleChanged));

        /// <summary>
        /// Gets or sets if the adorner is currently visible.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty = IsVisiblePropertyKey.DependencyProperty;

        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner",
            typeof(Adorner),
            typeof(Info),
            new PropertyMetadata(
                default(Adorner),
                (d, e) => ((Adorner?)e.OldValue)?.ClearTemplatedAdornerChild()));

        /// <summary>Helper for setting <see cref="TemplateProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="TemplateProperty"/> on.</param>
        /// <param name="value">Template property value.</param>
        public static void SetTemplate(DependencyObject element, ControlTemplate? value)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            element.SetValue(TemplateProperty, value);
        }

        /// <summary>Helper for getting <see cref="TemplateProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="TemplateProperty"/> from.</param>
        /// <returns>Template property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static ControlTemplate? GetTemplate(DependencyObject element)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            return (ControlTemplate?)element.GetValue(TemplateProperty);
        }

        /// <summary>Helper for setting <see cref="VisibilityProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="VisibilityProperty"/> on.</param>
        /// <param name="value">Visibility property value.</param>
        public static void SetVisibility(DependencyObject element, Visibility value)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            element.SetValue(VisibilityProperty, value);
        }

        /// <summary>Helper for getting <see cref="VisibilityProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="VisibilityProperty"/> from.</param>
        /// <returns>Visibility property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static Visibility GetVisibility(DependencyObject element)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            return (Visibility)element.GetValue(VisibilityProperty);
        }

        private static void SetIsVisible(this DependencyObject element, bool value)
        {
            element.SetValue(IsVisiblePropertyKey, value);
        }

        /// <summary>Helper for getting <see cref="IsVisibleProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="IsVisibleProperty"/> from.</param>
        /// <returns>IsVisible property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetIsVisible(this DependencyObject element)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            return (bool)element.GetValue(IsVisibleProperty);
        }

        private static void OnSizeChanged(object? sender, RoutedEventArgs e)
        {
            if (sender is UIElement adornedElement)
            {
                (adornedElement.GetValue(AdornerProperty) as Adorner)?.InvalidateMeasure();
                UpdateIsVisible(adornedElement);
            }
        }

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        private static void OnAdornedElementChanged(object? sender, object _)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
        {
            if (sender is UIElement adornedElement)
            {
                UpdateIsVisible(adornedElement);
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
                UpdateIsVisible(adornedElement);
            }
        }

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

        private static void UpdateIsVisible(UIElement element)
        {
            if (element.IsVisible &&
                element.IsLoaded() &&
                GetTemplate(element) is { } &&
                GetVisibility(element) == Visibility.Visible)
            {
                element.SetIsVisible(true);
                return;
            }
            else
            {
                element.SetIsVisible(false);
            }
        }
    }
}
