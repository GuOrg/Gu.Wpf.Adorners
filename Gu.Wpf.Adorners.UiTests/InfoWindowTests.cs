namespace Gu.Wpf.Adorners.UiTests
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class InfoWindowTests
    {
        private const string WindowName = "InfoWindow";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ImageAssert.OnFail = OnFail.SaveImageToTemp;
        }

        [TestCase("red border bound visibility", ".\\Images\\red border bound visibility_visible.png")]
        [TestCase("red border default visibility", ".\\Images\\red border default visibility.png")]
        public void Overlay(string name, string imageFileName)
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                Wait.For(TimeSpan.FromMilliseconds(200));
                var button = window.FindButton(name);
                ImageAssert.AreEqual(imageFileName, button);
            }
        }

        [Test]
        public void BoundVisibility()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                Wait.For(TimeSpan.FromMilliseconds(200));
                var button = window.FindButton("red border bound visibility");
                ImageAssert.AreEqual(".\\Images\\red border bound visibility_visible.png", button);

                window.FindButton("IsVisibleButton").Click();
                ImageAssert.AreEqual(".\\Images\\red border bound visibility_not_visible.png", button);
            }
        }

        [Test]
        public void WhenSizeChanges()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                Wait.For(TimeSpan.FromMilliseconds(200));
                var button = window.FindButton("red border default visibility");
                ImageAssert.AreEqual(".\\Images\\red border default visibility.png", button);

                window.FindSlider("WidthSlider").Value = 100;
                ImageAssert.AreEqual(".\\Images\\red border default visibility_width_100.png", button);
            }
        }

        [Test]
        public void WhenDrawsOutside()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                Wait.For(TimeSpan.FromMilliseconds(200));
                var groupBox = window.FindGroupBox("Draws outside");
                ImageAssert.AreEqual(".\\Images\\Draws outside.png", groupBox);
            }
        }
    }
}