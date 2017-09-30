namespace Gu.Wpf.Adorners.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class OverlayWindowTests
    {
        private const string WindowName = "OverlayWindow";

        [Test]
        public void NoOverlay()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var button = window.FindButton("No overlay");
                ImageAssert.AreEqual(".\\Images\\No overlay.png", button);
            }
        }

        [Test]
        public void DefaultVisibility()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var button = window.FindButton("Default visibility");
                ImageAssert.AreEqual(".\\Images\\Default visibility.png", button);
            }
        }

        [Test]
        public void BoundVisibility()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var button = window.FindButton("Bound visibility");
                ImageAssert.AreEqual(".\\Images\\Bound visibility_visible.png", button);

                window.FindButton("IsVisibleButton").Click();
                ImageAssert.AreEqual(".\\Images\\Bound visibility_not_visible.png", button);
            }
        }

        [Test]
        public void WithContentTemplate()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var button = window.FindButton("With content template");
                ImageAssert.AreEqual(".\\Images\\With content template.png", button);
            }
        }

        [Test]
        public void WithInheritedContentTemplate()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var groupBox = window.FindGroupBox("Inherits");
                ImageAssert.AreEqual(".\\Images\\WithInheritedContentTemplate.png", groupBox);
            }
        }
    }
}