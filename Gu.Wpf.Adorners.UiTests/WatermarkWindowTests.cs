namespace Gu.Wpf.Adorners.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class WatermarkWindowTests
    {
        private const string WindowName = "WatermarkWindow";

        [Test]
        public void DefaultAdornerWhenNotFocused()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                AutomationElement textBox = window.FindTextBox("WithDefaultAdorner");
                ImageAssert.AreEqual(".\\Images\\WithDefaultAdorner_not_focused.png", textBox);
            }
        }
    }
}