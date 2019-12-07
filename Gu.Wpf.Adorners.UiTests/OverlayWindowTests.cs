namespace Gu.Wpf.Adorners.UiTests
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class OverlayWindowTests
    {
        private const string ExeFileName = "Gu.Wpf.Adorners.Demo.exe";
        private const string WindowName = "OverlayWindow";

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

        [TestCase("No overlay", ".\\Images\\OverlayWindow\\No overlay.png")]
        [TestCase("Default visibility", ".\\Images\\OverlayWindow\\Default visibility.png")]
        [TestCase("With content template", ".\\Images\\OverlayWindow\\With content template.png")]
        public void Overlay(string name, string imageFileName)
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var button = window.FindButton(name);
                ImageAssert.AreEqual(imageFileName, button);
            }
        }

        [Test]
        public void BoundVisibility()
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                window.FindToggleButton("IsVisibleButton").IsChecked = true;
                var button = window.FindButton("Bound visibility");
                ImageAssert.AreEqual(".\\Images\\OverlayWindow\\Bound visibility_visible.png", button);

                window.FindToggleButton("IsVisibleButton").IsChecked = false;
                ImageAssert.AreEqual(".\\Images\\OverlayWindow\\Bound visibility_not_visible.png", button);
            }
        }

        [Test]
        public void WithInheritedContentTemplate()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var groupBox = window.FindGroupBox("Inherits");
                ImageAssert.AreEqual(".\\Images\\OverlayWindow\\Inherited_content_template.png", groupBox);
            }
        }

        [Test]
        public void WhenSizeChanges()
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                window.FindSlider("WidthSlider").Value = 200;
                var button = window.FindButton("Default visibility");
                ImageAssert.AreEqual(".\\Images\\OverlayWindow\\Default visibility.png", button);

                window.FindSlider("WidthSlider").Value = 100;
                ImageAssert.AreEqual(".\\Images\\OverlayWindow\\Default visibility_width_100.png", button);
            }
        }

        [TestCase("Collapsed")]
        [TestCase("Hidden")]
        public void WhenAdornedElementIs(string visibility)
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var button = window.FindButton("Default visibility");
                ImageAssert.AreEqual(".\\Images\\OverlayWindow\\Default visibility.png", button);

                var comboBox = window.FindComboBox("VisibilityCbx");
                comboBox.Select(visibility);
                Wait.For(TimeSpan.FromMilliseconds(200));

                // Checking that we don't crash here. See issue #24
                comboBox.Select("Visible");
                Wait.For(TimeSpan.FromMilliseconds(200));
                ImageAssert.AreEqual(".\\Images\\OverlayWindow\\Default visibility.png", button);
            }
        }
    }
}
