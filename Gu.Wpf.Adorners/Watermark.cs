namespace Gu.Wpf.Adorners
{
    using System.Windows;
    using System.Windows.Controls;

    public static class Watermark
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            "Text",
            typeof(string),
            typeof(Watermark),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OnWatermarkTextChanged));

        public static readonly DependencyProperty VisibleWhenProperty = DependencyProperty.RegisterAttached(
            "VisibleWhen",
            typeof(WatermarkVisibleWhen),
            typeof(Watermark),
            new PropertyMetadata(WatermarkVisibleWhen.Empty));

        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner",
            typeof(WatermarkAdorner),
            typeof(Watermark),
            new PropertyMetadata(default(WatermarkAdorner)));

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
                    var watermarkAdorner = new WatermarkAdorner(textBox);
                    textBox.SetAdorner(watermarkAdorner);
                    UpdateWatermarkVisibility(textBox);
                }
            }
        }

        private static void UpdateWatermarkVisibility(TextBox textBox)
        {
            var adorner = textBox.GetAdorner();
            AdornerService.Show(adorner);
        }
    }
}