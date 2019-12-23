namespace Gu.Wpf.Adorners.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class ComboBoxWindowTests
    {
        private const string ExeFileName = "Gu.Wpf.Adorners.Demo.exe";
        private const string WindowName = "ComboBoxWindow";

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Close the shared window after the last test.
            Application.KillLaunched(ExeFileName, WindowName);
        }

        [Test]
        public void Default()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var comboBox = window.FindComboBox("Default");
            ImageAssert.AreEqual($"Images\\ComboBoxWindow\\{TestImage.CurrentFolder}\\Default.png", comboBox, TestImage.OnFail);
        }

        [Test]
        public void DefaultEditable()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var comboBox = window.FindComboBox("DefaultEditable");
            ImageAssert.AreEqual($"Images\\ComboBoxWindow\\{TestImage.CurrentFolder}\\Default_editable.png", comboBox, TestImage.OnFail);
        }
    }
}
