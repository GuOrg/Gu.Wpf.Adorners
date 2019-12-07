namespace Gu.Wpf.Adorners.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class TextBoxWindowTests
    {
        private const string ExeFileName = "Gu.Wpf.Adorners.Demo.exe";
        private const string WindowName = "TextBoxWindow";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ImageAssert.OnFail = OnFail.SaveImageToTemp;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Close the shared window after the last test.
            Application.KillLaunched(ExeFileName, WindowName);
        }

        [Test]
        public void Default()
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("Default");
                if (WindowsVersion.IsWindows10())
                {
                    ImageAssert.AreEqual(".\\Images\\TextBoxWindow\\Default_Win10.png", textBox);
                }
                else
                {
                    Assert.Inconclusive($"No image for current windows version.");
                }
            }
        }

        [Test]
        public void DefaultWithZeroBorder()
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("DefaultWithZeroBorder");
                if (WindowsVersion.IsWindows10())
                {
                    ImageAssert.AreEqual(".\\Images\\TextBoxWindow\\Default_zero_border_Win10.png", textBox);
                }
                else
                {
                    Assert.Inconclusive($"No image for current windows version.");
                }
            }
        }
    }
}
