namespace Gu.Wpf.Adorners
{
    using System;
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
        /// Gets or sets visibility of the adorner. Note that setting it to visible does not need to trigger a show.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached(
            "IsVisible",
            typeof(bool?),
            typeof(Info),
            new PropertyMetadata(
                default(bool?),
                (d, e) => UpdateIsShowing(d)));

        private static readonly DependencyPropertyKey IsShowingPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsShowing",
            typeof(bool),
            typeof(Info),
            new PropertyMetadata(
                default(bool),
                OnIsShowingChanged));

        /// <summary>
        /// Gets or sets if the adorner is currently visible
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
            var element = sender as DependencyObject;
            element?.GetAdorner()?.InvalidateMeasure();
            UpdateIsShowing(element);
        }

        private static void OnAdornedElementChanged(object sender, EventArgs e)
        {
            UpdateIsShowing(sender as DependencyObject);
        }

        private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IsVisibleChangedEventManager.UpdateHandler((UIElement)d, OnAdornedElementChanged);
            LoadedEventManager.UpdateHandler((UIElement)d, OnAdornedElementChanged);
            UnloadedEventManager.UpdateHandler((UIElement)d, OnAdornedElementChanged);
            SizeChangedEventManager.UpdateHandler((FrameworkElement)d, OnSizeChanged);
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
                    adorner = TemplatedAdorner.Create(element, (ControlTemplate)element.GetValue(TemplateProperty));
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
}
