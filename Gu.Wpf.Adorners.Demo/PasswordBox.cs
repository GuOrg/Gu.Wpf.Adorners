namespace Gu.Wpf.Adorners.Demo
{
    using System.Windows;

    public static class PasswordBox
    {
        private static readonly DependencyPropertyKey IsEmptyPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsEmpty",
            typeof(bool),
            typeof(PasswordBox),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsEmptyProperty = IsEmptyPropertyKey.DependencyProperty;

        static PasswordBox()
        {
            EventManager.RegisterClassHandler(typeof(System.Windows.Controls.PasswordBox), System.Windows.Controls.PasswordBox.PasswordChangedEvent, new RoutedEventHandler(OnPasswordChanged));
        }

        private static void SetIsEmpty(DependencyObject element, bool value)
        {
            element.SetValue(IsEmptyPropertyKey, value);
        }

        public static bool GetIsEmpty(DependencyObject element)
        {
            return (bool)element.GetValue(IsEmptyProperty);
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (System.Windows.Controls.PasswordBox)sender;
            SetIsEmpty(passwordBox, string.IsNullOrEmpty(passwordBox.Password));
        }
    }
}
