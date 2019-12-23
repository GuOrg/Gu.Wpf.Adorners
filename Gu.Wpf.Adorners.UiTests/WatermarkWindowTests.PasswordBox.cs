namespace Gu.Wpf.Adorners.UiTests
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static partial class WatermarkWindowTests
    {
        public class PasswordBox
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
                var passwordBox = window.FindPasswordBox("PasswordBoxWithDefaultWatermark");
                window.FindButton("Lose focus").Click();
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Default_not_focused.png", passwordBox, TestImage.OnFail);
            }

            [Test]
            public void WithDefaultWatermarkWhenFocused()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var passwordBox = window.FindPasswordBox("PasswordBoxWithDefaultWatermark");
                passwordBox.Focus();
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Default_focused.png", passwordBox, TestImage.OnFail);
            }

            [Test]
            public void WithDefaultWatermarkWhenNotEmpty()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var passwordBox = window.FindPasswordBox("PasswordBoxWithDefaultWatermark");
                passwordBox.SetValue("abc");
                window.FindButton("Lose focus").Click();
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Default_not_empty.png", passwordBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkWithBoundText()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkWithBoundText");
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Bound_text_AAA.png", passwordBox, TestImage.OnFail);
                window.FindTextBox("AdornerText").Text = "abc";
                window.FindButton("Lose focus").Invoke();
                Wait.For(TimeSpan.FromMilliseconds(50));
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Bound_text_abc.png", passwordBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkWithInheritedFontSize()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkWithInheritedFontSize");
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Inherited_font_size.png", passwordBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkWithExplicitTextStyle()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkWithExplicitTextStyle");
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Explicit_text_style.png", passwordBox, TestImage.OnFail);
            }

            [Test]
            public void WithInheritedTextStyle()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var groupBox = window.FindGroupBox("Inherited style");
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Inherited_text_style.png", groupBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkVisibleWhenEmpty()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkVisibleWhenEmpty");
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Visible_when_empty_not_focused.png", passwordBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyWhenFocused()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkVisibleWhenEmpty");
                passwordBox.Focus();
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Visible_when_empty_focused.png", passwordBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyWhenNotEmpty()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkVisibleWhenEmpty");
                passwordBox.SetValue("abc");
                window.FindButton("Lose focus").Click();
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Visible_when_empty_not_empty.png", passwordBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocused()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Visible_when_empty_and_not_focused_not_focused.png", passwordBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocusedWhenFocused()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                passwordBox.Focus();
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Visible_when_empty_and_not_focused_focused.png", passwordBox, TestImage.OnFail);
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocusedWhenNotEmpty()
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                passwordBox.SetValue("abc");
                window.FindButton("Lose focus").Click();
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Visible_when_empty_and_not_focused_not_empty.png", passwordBox, TestImage.OnFail);
            }

            [TestCase("Collapsed")]
            [TestCase("Hidden")]
            public void WhenAdornedElementIs(string visibility)
            {
                using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
                var window = app.MainWindow;
                Wait.For(TimeSpan.FromMilliseconds(200));
                var button = window.FindPasswordBox("PasswordBoxWithDefaultWatermark");
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Default_not_focused.png", button, TestImage.OnFail);

                var comboBox = window.FindComboBox("VisibilityCbx");
                comboBox.Select(visibility);
                Wait.For(TimeSpan.FromMilliseconds(200));

                // Checking that we don't crash here. See issue #24
                comboBox.Select("Visible");
                Wait.For(TimeSpan.FromMilliseconds(200));
                ImageAssert.AreEqual("Images\\WatermarkWindow\\PasswordBox\\Default_not_focused.png", button, TestImage.OnFail);
            }
        }
    }
}
