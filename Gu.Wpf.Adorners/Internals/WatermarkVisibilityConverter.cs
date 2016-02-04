namespace Gu.Wpf.Adorners
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class WatermarkVisibilityConverter : IMultiValueConverter
    {
        public static readonly WatermarkVisibilityConverter Default = new WatermarkVisibilityConverter();
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var textBox = (TextBox)values[0];
            if (!textBox.IsVisible)
            {
                return Visibility.Collapsed;
            }
            var visibleWhen = textBox.GetVisibleWhen();
            switch (visibleWhen)
            {
                case WatermarkVisibleWhen.Empty:
                    return textBox.Text == "" ? Visibility.Visible : Visibility.Collapsed;
                case WatermarkVisibleWhen.NotKeyboardFocused:
                    return textBox.IsKeyboardFocused ? Visibility.Collapsed : Visibility.Visible;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}