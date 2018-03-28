namespace Gu.Wpf.Adorners.Demo.Windows
{
    using System.Windows;

    public partial class OverlayDialogWindow : Window
    {
        public OverlayDialogWindow()
        {
            this.InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.SetCurrentValue(Adorners.Overlay.IsVisibleProperty, true);
            MessageBox.Show("Text", "Caption");
            this.SetCurrentValue(Adorners.Overlay.IsVisibleProperty, false);
        }
    }
}
