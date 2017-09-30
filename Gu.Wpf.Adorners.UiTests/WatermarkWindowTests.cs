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
                var textBox = window.FindTextBox("WithDefaultAdorner");
                ImageAssert.AreEqual(".\\Images\\WithDefaultAdorner_not_focused.png", textBox);
            }
        }

        [Test]
        public void DefaultAdornerWhenFocused()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("WithDefaultAdorner");
                textBox.Focus();
                ImageAssert.AreEqual(".\\Images\\WithDefaultAdorner_focused.png", textBox);
            }
        }

        [Test]
        public void DefaultAdornerWhenNotEmpty()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("WithDefaultAdorner");
                textBox.Text = "abc";
                window.FindButton("Lose focus").Click();
                ImageAssert.AreEqual(".\\Images\\WithDefaultAdorner_not_empty.png", textBox);
            }
        }
    }
}