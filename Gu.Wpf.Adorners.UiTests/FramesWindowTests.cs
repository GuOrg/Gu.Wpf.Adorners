namespace Gu.Wpf.Adorners.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class FramesWindowTests
    {
        private const string WindowName = "NotShowingWhenInFrames/FramesWindow";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ImageAssert.OnFail = OnFail.SaveImageToTemp;
        }

        [Test]
        public void ClickAllTabs()
        {
            // Just a smoke test so we don't crash.
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var tab = window.FindTabControl();
                ImageAssert.AreEqual(".\\Images\\Tab1.png", tab.FindTextBox("Tab1"));

                tab.SelectedIndex = 1;
                ImageAssert.AreEqual(".\\Images\\Tab2.png", tab.FindTextBox("Tab2"));

                tab.SelectedIndex = 0;
                ImageAssert.AreEqual(".\\Images\\Tab1.png", tab.FindTextBox("Tab1"));

                tab.SelectedIndex = 1;
                ImageAssert.AreEqual(".\\Images\\Tab2.png", tab.FindTextBox("Tab2"));
            }
        }
    }
}