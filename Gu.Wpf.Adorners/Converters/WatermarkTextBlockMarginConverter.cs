namespace Gu.Wpf.Adorners
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal class WatermarkTextBlockMarginConverter : IValueConverter
    {
        internal static readonly WatermarkTextBlockMarginConverter Default = new WatermarkTextBlockMarginConverter();

        private WatermarkTextBlockMarginConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var presenter = (ScrollContentPresenter)parameter;
            var presenterMargin = presenter.Margin;
            var textMargin = ((FrameworkElement)presenter.Content).Margin;
            var result = new Thickness(
                presenterMargin.Left + textMargin.Left,
                presenterMargin.Top + textMargin.Top,
                presenterMargin.Right + textMargin.Right,
                presenterMargin.Bottom + textMargin.Bottom);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Only for one way bindings");
        }
    }
}
