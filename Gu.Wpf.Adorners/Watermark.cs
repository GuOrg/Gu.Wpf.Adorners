namespace Gu.Wpf.Adorners
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// Attached properties for showing watermarks.
    /// </summary>
    public static class Watermark
    {
#pragma warning disable SA1202 // Elements must be ordered by access

        /// <summary>
        /// The watermark text.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            "Text",
            typeof(string),
            typeof(Watermark),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits,
                OnTextChanged));

        /// <summary>
        /// Controls visibility of the adorner, default is WatermarkVisibleWhen.EmptyAndNotKeyboardFocused
        /// </summary>
        public static readonly DependencyProperty VisibleWhenProperty = DependencyProperty.RegisterAttached(
            "VisibleWhen",
            typeof(WatermarkVisibleWhen),
            typeof(Watermark),
            new FrameworkPropertyMetadata(
                WatermarkVisibleWhen.EmptyAndNotKeyboardFocused,
                FrameworkPropertyMetadataOptions.Inherits,
                OnVisibleWhenChanged));

        /// <summary>
        /// The style for the <see cref="TextBlock"/> rendering <see cref="TextProperty"/>
        /// </summary>
        public static readonly DependencyProperty TextStyleProperty = DependencyProperty.RegisterAttached(
            "TextStyle",
            typeof(Style),
            typeof(Watermark),
            new FrameworkPropertyMetadata(
                default(Style),
                FrameworkPropertyMetadataOptions.Inherits,
                OnTextStyleChanged),
            ValidateTextStyle);

        private static readonly DependencyPropertyKey IsShowingPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsShowing",
            typeof(bool),
            typeof(Watermark),
            new PropertyMetadata(
                default(bool),
                OnIsShowingChanged));

        /// <summary>
        /// Gets or sets if the adorner is currently visible
        /// </summary>
        public static readonly DependencyProperty IsShowingProperty = IsShowingPropertyKey.DependencyProperty;

        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner",
            typeof(WatermarkAdorner),
            typeof(Watermark),
            new PropertyMetadata(
                default(WatermarkAdorner),
                OnAdornerChanged));

        private static readonly DependencyProperty HandlerProperty = DependencyProperty.RegisterAttached(
            "Handler",
            typeof(TextBoxHandler),
            typeof(Watermark),
            new PropertyMetadata(default(TextBoxHandler)));

        /// <summary>
        /// Helper for setting Text property on a UIElement.
        /// </summary>
        /// <param name="element">UIElement to set Text property on.</param>
        /// <param name="value">Text property value.</param>
        public static void SetText(this UIElement element, string value)
        {
            element.SetValue(TextProperty, value);
        }

        /// <summary>
        /// Helper for reading Text property from a UIElement.
        /// </summary>
        /// <param name="element">UIElement to read Text property from.</param>
        /// <returns>Text property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static string GetText(this UIElement element)
        {
            return (string)element.GetValue(TextProperty);
        }

        /// <summary>
        /// Helper for setting VisibleWhen property on a UIElement.
        /// </summary>
        /// <param name="element">UIElement to set VisibleWhen property on.</param>
        /// <param name="value">VisibleWhen property value.</param>
        public static void SetVisibleWhen(this UIElement element, WatermarkVisibleWhen value)
        {
            element.SetValue(VisibleWhenProperty, value);
        }

        /// <summary>
        /// Helper for reading VisibleWhen property from a UIElement.
        /// </summary>
        /// <param name="element">UIElement to read VisibleWhen property from.</param>
        /// <returns>VisibleWhen property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static WatermarkVisibleWhen GetVisibleWhen(this UIElement element)
        {
            return (WatermarkVisibleWhen)element.GetValue(VisibleWhenProperty);
        }

        /// <summary>
        /// Helper for setting TextStyle property on a UIElement.
        /// </summary>
        /// <param name="element">UIElement to set TextStyle property on.</param>
        /// <param name="value">TextStyle property value.</param>
        public static void SetTextStyle(this UIElement element, Style value)
        {
            element.SetValue(TextStyleProperty, value);
        }

        /// <summary>
        /// Helper for reading TextStyle property from a UIElement.
        /// </summary>
        /// <param name="element">UIElement to read TextStyle property from.</param>
        /// <returns>TextStyle property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Style GetTextStyle(this UIElement element)
        {
            return (Style)element.GetValue(TextStyleProperty);
        }

        private static void SetIsShowing(this Control element, bool value)
        {
            element.SetValue(IsShowingPropertyKey, value);
        }

        /// <summary>
        /// Helper for reading IsShowing property from a TextBox.
        /// </summary>
        /// <param name="element">TextBox to read IsShowing property from.</param>
        /// <returns>IsShowing property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetIsShowing(this Control element)
        {
            return (bool)element.GetValue(IsShowingProperty);
        }

        private static void SetAdorner(this DependencyObject element, WatermarkAdorner value)
        {
            element.SetValue(AdornerProperty, value);
        }

        private static WatermarkAdorner GetAdorner(this DependencyObject element)
        {
            return (WatermarkAdorner)element.GetValue(AdornerProperty);
        }

#pragma warning restore SA1202 // Elements must be ordered by access

        private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is TextBox textBox)
            {
                UpdateHandlers(textBox);
                UpdateIsShowing(textBox);
            }
        }

        private static void OnVisibleWhenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateIsShowing(d as TextBox);
        }

        private static void OnTextStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBoxBase textBox)
            {
                UpdateHandlers(textBox);
                var adorner = textBox.GetAdorner();
                if (adorner == null)
                {
                    return;
                }

                adorner.SetCurrentValue(WatermarkAdorner.TextStyleProperty, e.NewValue);
                if (e.NewValue == null)
                {
                    adorner.UpdateDefaultStyle();
                }
            }
        }

        private static void UpdateHandlers(Control adornedElement)
        {
            if (adornedElement.GetValue(HandlerProperty) == null)
            {
                if (adornedElement is TextBox textBox)
                {
                    adornedElement.SetCurrentValue(HandlerProperty, new TextBoxHandler(textBox));
                }
            }
        }

        private static bool ValidateTextStyle(object value)
        {
            var style = (Style)value;
            return style?.TargetType == null ||
                   typeof(TextBlock).IsAssignableFrom(style.TargetType);
        }

        private static void OnIsShowingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adornedElement = (Control)d;
            if (Equals(e.NewValue, true))
            {
                var adorner = adornedElement.GetAdorner();
                if (adorner == null)
                {
                    adorner = new WatermarkAdorner(adornedElement);
                    adornedElement.SetAdorner(adorner);
                    var textStyle = adornedElement.GetTextStyle();
                    if (textStyle != null)
                    {
                        adorner.SetCurrentValue(WatermarkAdorner.TextStyleProperty, textStyle);
                    }

                    AdornerService.Show(adorner);
                    adornedElement.SetCurrentValue(AdornerProperty, adorner);
                }
                else
                {
                    Debug.Assert(condition: false, message: "Already visible");
                }
            }
            else
            {
                var adorner = adornedElement.GetAdorner();
                if (adorner != null)
                {
                    AdornerService.Remove(adorner);
                    adornedElement.ClearValue(AdornerProperty);
                }
            }
        }

        private static void OnAdornerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WatermarkAdorner)e.OldValue)?.ClearChild();
        }

        private static void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            element?.GetAdorner()?.InvalidateMeasure();
            UpdateIsShowing(sender as Control);
        }

        private static void OnAdornedElementChanged(object sender, EventArgs e)
        {
            UpdateIsShowing((TextBox)sender);
        }

        private static void UpdateIsShowing(Control adornedElement)
        {
            var handler = (TextBoxHandler)adornedElement?.GetValue(HandlerProperty);
            if (handler == null)
            {
                return;
            }

            if (!adornedElement.IsVisible ||
                string.IsNullOrEmpty(GetText(adornedElement)))
            {
                adornedElement.SetIsShowing(false);
            }
            else
            {
                switch (adornedElement.GetVisibleWhen())
                {
                    case WatermarkVisibleWhen.Empty:
                        adornedElement.SetIsShowing(string.IsNullOrEmpty(handler.Text));
                        break;
                    case WatermarkVisibleWhen.EmptyAndNotKeyboardFocused:
                        adornedElement.SetIsShowing(string.IsNullOrEmpty(handler.Text) && !adornedElement.IsKeyboardFocused);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private class TextBoxHandler
        {
            private readonly TextBox textBox;

            public TextBoxHandler(TextBox textBox)
            {
                this.textBox = textBox;
                IsVisibleChangedEventManager.UpdateHandler(textBox, OnAdornedElementChanged);
                LoadedEventManager.UpdateHandler(textBox, OnAdornedElementChanged);
                UnloadedEventManager.UpdateHandler(textBox, OnAdornedElementChanged);
                GotKeyboardFocusEventManager.UpdateHandler(textBox, OnAdornedElementChanged);
                LostKeyboardFocusEventManager.UpdateHandler(textBox, OnAdornedElementChanged);
                TextChangedEventManager.UpdateHandler(textBox, OnAdornedElementChanged);
                SizeChangedEventManager.UpdateHandler(textBox, OnSizeChanged);
            }

            public string Text => this.textBox.Text;
        }
    }
}
