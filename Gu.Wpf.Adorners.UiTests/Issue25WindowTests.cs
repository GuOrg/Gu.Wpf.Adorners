namespace Gu.Wpf.Adorners.UiTests
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class Issue25WindowTests
    {
        private const string ExeFileName = "Gu.Wpf.Adorners.Demo.exe";
        private const string WindowName = "Issue25/Issue25Window";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ImageAssert.OnFail = OnFail.SaveImageToTemp;
        }

        [Test]
        public void ClickAllTabs()
        {
            // Just a smoke test so we don't crash.
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                Wait.For(TimeSpan.FromMilliseconds(200));
                var window = app.MainWindow;
            }
        }
    }
}
