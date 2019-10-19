namespace Gu.Wpf.Adorners.UiTests
{
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
        public void Resize()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                while (window.ActualHeight > 100)
                {
                    window.Resize((int)window.ActualWidth, (int)(window.ActualHeight - 10));
                }

                while (window.ActualHeight < 600)
                {
                    window.Resize((int)window.ActualWidth, (int)(window.ActualHeight + 10));
                }

                while (window.ActualHeight > 100)
                {
                    window.Resize((int)window.ActualWidth, (int)(window.ActualHeight - 10));
                }

                while (window.ActualHeight < 600)
                {
                    window.Resize((int)window.ActualWidth, (int)(window.ActualHeight + 10));
                }
            }
        }
    }
}
