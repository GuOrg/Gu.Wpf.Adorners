namespace Gu.Wpf.Adorners.Demo.Windows
{
    using System.Windows;

    public partial class OverlayDialogWindow : Window
    {
        public OverlayDialogWindow()
        {
            this.InitializeComponent();
        }

        private void OnShowMessageBoxClick(object sender, RoutedEventArgs e)
        {
            this.SetCurrentValue(Overlay.VisibilityProperty, Visibility.Visible);
            MessageBox.Show("Text", "Caption");
            this.SetCurrentValue(Overlay.VisibilityProperty, Visibility.Hidden);
        }
    }
}
