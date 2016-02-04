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
                OnTextStyleChanged));

        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner",
            typeof(WatermarkAdorner),
            typeof(Watermark),
            new PropertyMetadata(default(WatermarkAdorner)));

        static Watermark()
        {
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

            var text = (string)e.NewValue;
            if (string.IsNullOrEmpty(text))
            {
                var adorner = textBox.GetAdorner();
                if (adorner != null)
                {
                    AdornerService.Remove(adorner);
                }

                textBox.ClearValue(AdornerProperty);
            }
            else
            {
                var adorner = textBox.GetAdorner();
                if (adorner == null)
                {
                    adorner = new WatermarkAdorner(textBox);
                    var textStyle = textBox.GetTextStyle();
                    if (textStyle != null)
                    {
                        adorner.TextStyle = textStyle;
                    }

                    textBox.SetAdorner(adorner);
                    UpdateWatermarkVisibility(textBox);
                }

                adorner.Text = (string)e.NewValue;
            }
        }

        private static void OnVisibleWhenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private static void OnTextStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBoxBase = d as TextBoxBase;
            var adorner = textBoxBase?.GetAdorner();
            if (adorner != null)
            {
                adorner.TextStyle = (Style)e.NewValue;
            }
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
            var adorner = textBox.GetAdorner();
            if (adorner == null)
            {
                return;
            }

            var visibleWhen = textBox.GetVisibleWhen();
            Debug.WriteLine(visibleWhen);
            switch (visibleWhen)
            {
                case WatermarkVisibleWhen.Empty:
                    if (string.IsNullOrEmpty(textBox.Text))
                    {
                        AdornerService.Show(adorner);
                    }
                    else
                    {
                        AdornerService.Remove(adorner);
                    }
                    break;
                case WatermarkVisibleWhen.EmptyAndNotKeyboardFocused:
                    if (string.IsNullOrEmpty(textBox.Text) && !textBox.IsKeyboardFocused)
                    {
                        AdornerService.Show(adorner);
                    }
                    else
                    {
                        AdornerService.Remove(adorner);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}