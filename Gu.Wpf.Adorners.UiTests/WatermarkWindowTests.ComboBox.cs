namespace Gu.Wpf.Adorners.UiTests
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static partial class WatermarkWindowTests
    {
        public class ComboBox
        {
            [OneTimeTearDown]
            public void OneTimeTearDown()
            {
                // Close the shared window after the last test.
                Application.KillLaunched(ExeFileName, WindowName);
            }

            [Test]
            public void WithDefaultWatermarkWhenNotFocused()
            {
                using var app = Application.Launch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var comboBox = window.FindComboBox("ComboBoxWithDefaultWatermark");
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Default_not_focused.png", comboBox, TestImage.OnFail);
            }

            [Test]
            public void WithDefaultWatermarkWhenFocused()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var comboBox = window.FindComboBox("ComboBoxWithDefaultWatermark");
                comboBox.Focus();
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Default_focused.png", comboBox, TestImage.OnFail);
            }

            [Test]
            public void WithDefaultWatermarkWhenNotEmpty()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var comboBox = window.FindComboBox("ComboBoxWithDefaultWatermark");
                comboBox.Select("abc");
                window.FindButton("Lose focus").Click();
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Default_not_empty.png", comboBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkWithBoundText()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var comboBox = window.FindComboBox("ComboBoxWithWatermarkWithBoundText");
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Bound_text_AAA.png", comboBox, TestImage.OnFail);
                window.FindTextBox("AdornerText").Text = "abc";
                window.FindButton("Lose focus").Invoke();
                Wait.For(TimeSpan.FromMilliseconds(50));
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Bound_text_abc.png", comboBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkWithInheritedFontSize()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var comboBox = window.FindComboBox("ComboBoxWithWatermarkWithInheritedFontSize");
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Inherited_font_size.png", comboBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkWithExplicitTextStyle()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var comboBox = window.FindComboBox("ComboBoxWithWatermarkWithExplicitTextStyle");
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Explicit_text_style.png", comboBox, TestImage.OnFail);
            }

            [Test]
            public void WithInheritedTextStyle()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var groupBox = window.FindGroupBox("Inherited style");
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Inherited_text_style.png", groupBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkVisibleWhenEmpty()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var comboBox = window.FindComboBox("ComboBoxWithWatermarkVisibleWhenEmpty");
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Visible_when_empty_not_focused.png", comboBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyWhenFocused()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var comboBox = window.FindComboBox("ComboBoxWithWatermarkVisibleWhenEmpty");
                comboBox.Focus();
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Visible_when_empty_focused.png", comboBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyWhenNotEmpty()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var comboBox = window.FindComboBox("ComboBoxWithWatermarkVisibleWhenEmpty");
                comboBox.Select("abc");
                window.FindButton("Lose focus").Click();
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Visible_when_empty_not_empty.png", comboBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocused()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var comboBox = window.FindComboBox("ComboBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Visible_when_empty_and_not_focused_not_focused.png", comboBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocusedWhenFocused()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var comboBox = window.FindComboBox("ComboBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                comboBox.Focus();
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Visible_when_empty_and_not_focused_focused.png", comboBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocusedWhenNotEmpty()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var comboBox = window.FindComboBox("ComboBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                comboBox.Select("abc");
                window.FindButton("Lose focus").Click();
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Visible_when_empty_and_not_focused_not_empty.png", comboBox, TestImage.OnFail);
            }

            [TestCase("Collapsed")]
            [TestCase("Hidden")]
            public void WhenAdornedElementIs(string visibility)
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                Wait.For(TimeSpan.FromMilliseconds(200));
                var button = window.FindComboBox("ComboBoxWithDefaultWatermark");
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Default_not_focused.png", button, TestImage.OnFail);

                var comboBox = window.FindComboBox("VisibilityCbx");
                comboBox.Select(visibility);
                Wait.For(TimeSpan.FromMilliseconds(200));

                // Checking that we don't crash here. See issue #24
                comboBox.Select("Visible");
                Wait.For(TimeSpan.FromMilliseconds(200));
                ImageAssert.AreEqual("Images\\WatermarkWindow\\ComboBox\\Default_not_focused.png", button, TestImage.OnFail);
            }
        }
    }
}
