namespace Gu.Wpf.Adorners.Demo
{
    using System.Windows;
    using System.Windows.Controls;

    public partial class DialogOverlayView : UserControl
    {
        public DialogOverlayView()
        {
            this.InitializeComponent();
        }

        private void OnShowMessageBoxClick(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window?.SetCurrentValue(Overlay.IsVisibleProperty, true);
            MessageBox.Show("Text", "Caption");
            window?.SetCurrentValue(Overlay.IsVisibleProperty, false);
        }
    }
}
