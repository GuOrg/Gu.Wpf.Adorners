namespace Gu.Wpf.Adorners
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public static class Watermark
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            "Text",
            typeof(string),
            typeof(Watermark),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits,
                OnWatermarkTextChanged));

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
            OnValidateTextStyle);

        private static readonly DependencyPropertyKey IsVisiblePropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsVisible",
            typeof(bool),
            typeof(Watermark),
            new PropertyMetadata(default(bool), OnIsVisibleChanged));

        public static readonly DependencyProperty IsVisibleProperty = IsVisiblePropertyKey.DependencyProperty;

        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner",
            typeof(WatermarkAdorner),
            typeof(Watermark),
            new PropertyMetadata(default(WatermarkAdorner)));

        static Watermark()
        {
            Loaded.Track();
            UnLoaded.Track();
            EventManager.RegisterClassHandler(typeof(TextBox), FrameworkElement.SizeChangedEvent, new RoutedEventHandler(OnSizeChanged));
            EventManager.RegisterClassHandler(typeof(TextBox), FrameworkElement.LoadedEvent, new RoutedEventHandler(OnLoaded));
            EventManager.RegisterClassHandler(typeof(TextBox), FrameworkElement.UnloadedEvent, new RoutedEventHandler(OnUnLoaded));
            EventManager.RegisterClassHandler(typeof(TextBox), Visible.IsVisibleChangedEvent, new RoutedEventHandler(OnIsVisibleChanged));
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent, new RoutedEventHandler(OnGotKeyboardFocus));
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.LostKeyboardFocusEvent, new RoutedEventHandler(OnLostKeyboardFocus));
            EventManager.RegisterClassHandler(typeof(TextBox), TextBoxBase.TextChangedEvent, new RoutedEventHandler(OnTextChanged));
        }

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

        private static void SetIsVisible(this TextBox element, bool value)
        {
            element.SetValue(IsVisiblePropertyKey, value);
        }

        public static bool GetIsVisible(this TextBox element)
        {
            return (bool)element.GetValue(IsVisibleProperty);
        }

        private static void SetAdorner(this DependencyObject element, WatermarkAdorner value)
        {
            element.SetValue(AdornerProperty, value);
        }

        private static WatermarkAdorner GetAdorner(this DependencyObject element)
        {
            return (WatermarkAdorner)element.GetValue(AdornerProperty);
        }

        private static void OnWatermarkTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var textBox = o as TextBox;
            if (textBox == null)
            {
                return;
            }

            UpdateWatermarkVisibility(textBox);
        }

        private static void OnVisibleWhenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateWatermarkVisibility(d as TextBox);
        }

        private static void OnTextStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adorner = (d as TextBox)?.GetAdorner();
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

        private static bool OnValidateTextStyle(object value)
        {
            var style = (Style)value;
            return style?.TargetType == null ||
                   typeof(TextBlock).IsAssignableFrom(style.TargetType);
        }

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.Print($"Visible changed to: {e.NewValue}");
            var textBox = (TextBox)d;
            if (Equals(e.NewValue, true))
            {
                var adorner = textBox.GetAdorner();
                if (adorner == null)
                {
                    adorner = new WatermarkAdorner(textBox);
                    var textStyle = textBox.GetTextStyle();
                    if (textStyle != null)
                    {
                        adorner.SetCurrentValue(WatermarkAdorner.TextStyleProperty, textStyle);
                    }

                    AdornerService.Show(adorner);
                    textBox.SetValue(AdornerProperty, adorner);
                }
                else
                {
                    Debug.Print("Already visible");
                }
            }
            else
            {
                var adorner = textBox.GetAdorner();
                if (adorner != null)
                {
                    AdornerService.Remove(adorner);
                    adorner.ClearChild();
                    textBox.ClearValue(AdornerProperty);
                }
            }
        }

        private static void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            UpdateWatermarkVisibility((TextBox)sender);
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateWatermarkVisibility((TextBox)sender);
        }

        private static void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateWatermarkVisibility((TextBox)sender);
        }

        private static void OnIsVisibleChanged(object sender, RoutedEventArgs e)
        {
            UpdateWatermarkVisibility((TextBox)sender);
        }

        private static void OnGotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            UpdateWatermarkVisibility((TextBox)sender);
        }

        private static void OnLostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            UpdateWatermarkVisibility((TextBox)sender);
        }

        private static void OnTextChanged(object sender, RoutedEventArgs e)
        {
            UpdateWatermarkVisibility((TextBox)sender);
        }

        private static void UpdateWatermarkVisibility(TextBox textBox)
        {
            if (textBox == null)
            {
                return;
            }

            if (!textBox.IsVisible || !textBox.IsLoaded || string.IsNullOrEmpty(GetText(textBox)))
            {
                textBox.SetIsVisible(false);
            }
            else
            {
                switch (textBox.GetVisibleWhen())
                {
                    case WatermarkVisibleWhen.Empty:
                        textBox.SetIsVisible(string.IsNullOrEmpty(textBox.Text));
                        break;
                    case WatermarkVisibleWhen.EmptyAndNotKeyboardFocused:
                        textBox.SetIsVisible(string.IsNullOrEmpty(textBox.Text) && !textBox.IsKeyboardFocused);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}