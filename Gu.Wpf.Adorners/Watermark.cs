namespace Gu.Wpf.Adorners
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public static class Watermark
    {
#pragma warning disable SA1202 // Elements must be ordered by access
        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            "Text",
            typeof(string),
            typeof(Watermark),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits,
                OnTextChanged));

        public static readonly DependencyProperty VisibleWhenProperty = DependencyProperty.RegisterAttached(
            "VisibleWhen",
            typeof(WatermarkVisibleWhen),
            typeof(Watermark),
            new FrameworkPropertyMetadata(
                WatermarkVisibleWhen.EmptyAndNotKeyboardFocused,
                FrameworkPropertyMetadataOptions.Inherits,
                OnVisibleWhenChanged));

        public static readonly DependencyProperty TextStyleProperty = DependencyProperty.RegisterAttached(
            "TextStyle",
            typeof(Style),
            typeof(Watermark),
            new FrameworkPropertyMetadata(
                default(Style),
                FrameworkPropertyMetadataOptions.Inherits,
                OnTextStyleChanged),
            TextStyleValidateValue);

        private static readonly DependencyPropertyKey IsShowingPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsShowing",
            typeof(bool),
            typeof(Watermark),
            new PropertyMetadata(
                default(bool),
                OnIsShowingChanged));

        public static readonly DependencyProperty IsShowingProperty = IsShowingPropertyKey.DependencyProperty;

        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner",
            typeof(WatermarkAdorner),
            typeof(Watermark),
            new PropertyMetadata(
                default(WatermarkAdorner),
                OnAdornerChanged));

        public static void SetText(this UIElement element, string value)
        {
            element.SetValue(TextProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static string GetText(this UIElement element)
        {
            return (string)element.GetValue(TextProperty);
        }

        public static void SetVisibleWhen(this UIElement element, WatermarkVisibleWhen value)
        {
            element.SetValue(VisibleWhenProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static WatermarkVisibleWhen GetVisibleWhen(this UIElement element)
        {
            return (WatermarkVisibleWhen)element.GetValue(VisibleWhenProperty);
        }

        public static void SetTextStyle(this UIElement element, Style value)
        {
            element.SetValue(TextStyleProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Style GetTextStyle(this UIElement element)
        {
            return (Style)element.GetValue(TextStyleProperty);
        }

        private static void SetIsShowing(this TextBox element, bool value)
        {
            element.SetValue(IsShowingPropertyKey, value);
        }

        public static bool GetIsShowing(this TextBox element)
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

        private static void UpdateHandlers(TextBoxBase textBox)
        {
            IsVisibleChangedEventManager.UpdateHandler(textBox, OnAdornedElementChanged);
            LoadedEventManager.UpdateHandler(textBox, OnAdornedElementChanged);
            UnloadedEventManager.UpdateHandler(textBox, OnAdornedElementChanged);
            GotKeyboardFocusEventManager.UpdateHandler(textBox, OnAdornedElementChanged);
            LostKeyboardFocusEventManager.UpdateHandler(textBox, OnAdornedElementChanged);
            TextChangedEventManager.UpdateHandler(textBox, OnAdornedElementChanged);
            SizeChangedEventManager.UpdateHandler(textBox, OnSizeChanged);
        }

        private static bool TextStyleValidateValue(object value)
        {
            var style = (Style)value;
            return style?.TargetType == null ||
                   typeof(TextBlock).IsAssignableFrom(style.TargetType);
        }

        private static void OnIsShowingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBox)d;
            if (Equals(e.NewValue, true))
            {
                var adorner = textBox.GetAdorner();
                if (adorner == null)
                {
                    adorner = new WatermarkAdorner(textBox);
                    textBox.SetAdorner(adorner);
                    var textStyle = textBox.GetTextStyle();
                    if (textStyle != null)
                    {
                        adorner.SetCurrentValue(WatermarkAdorner.TextStyleProperty, textStyle);
                    }

                    AdornerService.Show(adorner);
                    textBox.SetCurrentValue(AdornerProperty, adorner);
                }
                else
                {
                    Debug.Assert(condition: false, message: "Already visible");
                }
            }
            else
            {
                var adorner = textBox.GetAdorner();
                if (adorner != null)
                {
                    AdornerService.Remove(adorner);
                    textBox.ClearValue(AdornerProperty);
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
            UpdateIsShowing((TextBox)sender);
        }

        private static void OnAdornedElementChanged(object sender, EventArgs e)
        {
            UpdateIsShowing((TextBox)sender);
        }

        private static void UpdateIsShowing(TextBox textBox)
        {
            if (textBox == null)
            {
                return;
            }

            if (!textBox.IsVisible || string.IsNullOrEmpty(GetText(textBox)))
            {
                textBox.SetIsShowing(false);
            }
            else
            {
                switch (textBox.GetVisibleWhen())
                {
                    case WatermarkVisibleWhen.Empty:
                        textBox.SetIsShowing(string.IsNullOrEmpty(textBox.Text));
                        break;
                    case WatermarkVisibleWhen.EmptyAndNotKeyboardFocused:
                        textBox.SetIsShowing(string.IsNullOrEmpty(textBox.Text) && !textBox.IsKeyboardFocused);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}