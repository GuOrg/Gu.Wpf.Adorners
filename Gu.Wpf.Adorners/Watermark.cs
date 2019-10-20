namespace Gu.Wpf.Adorners
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;

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
        /// Controls visibility of the adorner, default is WatermarkVisibleWhen.EmptyAndNotKeyboardFocused.
        /// </summary>
        public static readonly DependencyProperty VisibleWhenProperty = DependencyProperty.RegisterAttached(
            "VisibleWhen",
            typeof(WatermarkVisibleWhen),
            typeof(Watermark),
            new FrameworkPropertyMetadata(
                WatermarkVisibleWhen.EmptyAndNotKeyboardFocused,
                FrameworkPropertyMetadataOptions.Inherits,
                (d, e) => UpdateIsShowing(d as Control)));

        /// <summary>
        /// The style for the <see cref="TextBlock"/> rendering <see cref="TextProperty"/>.
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
        /// Gets or sets if the adorner is currently visible.
        /// </summary>
        public static readonly DependencyProperty IsShowingProperty = IsShowingPropertyKey.DependencyProperty;

        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner",
            typeof(WatermarkAdorner),
            typeof(Watermark),
            new PropertyMetadata(
                default(WatermarkAdorner),
                (d, e) => ((WatermarkAdorner)e.OldValue)?.ClearChild()));

        private static readonly DependencyProperty ListenerProperty = DependencyProperty.RegisterAttached(
            "Listener",
            typeof(IListener),
            typeof(Watermark),
            new PropertyMetadata(default(IListener)));

        private interface IListener
        {
            string Text { get; }
        }

        /// <summary>Helper for setting <see cref="TextProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="TextProperty"/> on.</param>
        /// <param name="value">Text property value.</param>
        public static void SetText(this UIElement element, string value)
        {
            element.SetValue(TextProperty, value);
        }

        /// <summary>Helper for getting <see cref="TextProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="TextProperty"/> from.</param>
        /// <returns>Text property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static string GetText(this UIElement element)
        {
            return (string)element.GetValue(TextProperty);
        }

        /// <summary>Helper for setting <see cref="VisibleWhenProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="VisibleWhenProperty"/> on.</param>
        /// <param name="value">VisibleWhen property value.</param>
        public static void SetVisibleWhen(this UIElement element, WatermarkVisibleWhen value)
        {
            element.SetValue(VisibleWhenProperty, value);
        }

        /// <summary>Helper for getting <see cref="VisibleWhenProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="VisibleWhenProperty"/> from.</param>
        /// <returns>VisibleWhen property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static WatermarkVisibleWhen GetVisibleWhen(this UIElement element)
        {
            return (WatermarkVisibleWhen)element.GetValue(VisibleWhenProperty);
        }

        /// <summary>Helper for setting <see cref="TextStyleProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="TextStyleProperty"/> on.</param>
        /// <param name="value">TextStyle property value.</param>
        public static void SetTextStyle(this UIElement element, Style value)
        {
            element.SetValue(TextStyleProperty, value);
        }

        /// <summary>Helper for getting <see cref="TextStyleProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="TextStyleProperty"/> from.</param>
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

        /// <summary>Helper for getting <see cref="IsShowingProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="Control"/> to read <see cref="IsShowingProperty"/> from.</param>
        /// <returns>IsShowing property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(Control))]
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
            switch (o)
            {
                case ComboBox comboBox:
                    UpdateListener(comboBox);
                    UpdateIsShowing(comboBox);
                    break;
                case PasswordBox passwordBox:
                    UpdateListener(passwordBox);
                    UpdateIsShowing(passwordBox);
                    break;
                case TextBox textBox:
                    UpdateListener(textBox);
                    UpdateIsShowing(textBox);
                    break;
            }

            void UpdateListener(Control adornedElement)
            {
                if (adornedElement.GetValue(ListenerProperty) == null)
                {
                    switch (adornedElement)
                    {
                        case TextBox textBox:
                            adornedElement.SetCurrentValue(ListenerProperty, new TextBoxListener(textBox));
                            break;
                        case PasswordBox passwordBox:
                            adornedElement.SetCurrentValue(ListenerProperty, new PasswordBoxListener(passwordBox));
                            break;
                        case ComboBox comboBox:
                            adornedElement.SetCurrentValue(ListenerProperty, new ComboBoxListener(comboBox));
                            break;
                    }
                }
            }
        }

        private static void OnTextStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Control adornedElement)
            {
                var adorner = adornedElement.GetAdorner();
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

        private static void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            element?.GetAdorner()?.InvalidateMeasure();
            UpdateIsShowing(sender as Control);
        }

        private static void OnAdornedElementChanged(object sender, EventArgs e)
        {
            UpdateIsShowing(sender as Control);
        }

        private static void UpdateIsShowing(Control adornedElement)
        {
            var listener = (IListener)adornedElement?.GetValue(ListenerProperty);
            if (listener == null)
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
                        adornedElement.SetIsShowing(string.IsNullOrEmpty(listener.Text));
                        break;
                    case WatermarkVisibleWhen.EmptyAndNotKeyboardFocused:
                        adornedElement.SetIsShowing(string.IsNullOrEmpty(listener.Text) && !adornedElement.IsKeyboardFocused);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(adornedElement), "Should never get here, bug in Gu.Wpf.Adorners.");
                }
            }
        }

        private class TextBoxListener : IListener
        {
            private readonly TextBox textBox;

            internal TextBoxListener(TextBox textBox)
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

        private class PasswordBoxListener : IListener
        {
            private readonly PasswordBox passwordBox;

            internal PasswordBoxListener(PasswordBox passwordBox)
            {
                this.passwordBox = passwordBox;
                IsVisibleChangedEventManager.UpdateHandler(passwordBox, OnAdornedElementChanged);
                LoadedEventManager.UpdateHandler(passwordBox, OnAdornedElementChanged);
                UnloadedEventManager.UpdateHandler(passwordBox, OnAdornedElementChanged);
                GotKeyboardFocusEventManager.UpdateHandler(passwordBox, OnAdornedElementChanged);
                LostKeyboardFocusEventManager.UpdateHandler(passwordBox, OnAdornedElementChanged);
                PasswordChangedEventManager.UpdateHandler(passwordBox, OnAdornedElementChanged);
                SizeChangedEventManager.UpdateHandler(passwordBox, OnSizeChanged);
            }

            public string Text => this.passwordBox.Password;
        }

        private class ComboBoxListener : IListener
        {
            private readonly ComboBox comboBox;

            internal ComboBoxListener(ComboBox comboBox)
            {
                this.comboBox = comboBox;
                IsVisibleChangedEventManager.UpdateHandler(comboBox, OnAdornedElementChanged);
                LoadedEventManager.UpdateHandler(comboBox, OnAdornedElementChanged);
                UnloadedEventManager.UpdateHandler(comboBox, OnAdornedElementChanged);
                GotKeyboardFocusEventManager.UpdateHandler(comboBox, OnAdornedElementChanged);
                LostKeyboardFocusEventManager.UpdateHandler(comboBox, OnAdornedElementChanged);
                SelectionChangedEventManager.UpdateHandler(comboBox, OnAdornedElementChanged);
                SizeChangedEventManager.UpdateHandler(comboBox, OnSizeChanged);
            }

            public string Text => this.comboBox.Text;
        }
    }
}
