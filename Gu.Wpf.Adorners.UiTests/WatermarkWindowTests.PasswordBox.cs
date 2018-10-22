namespace Gu.Wpf.Adorners.UiTests
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static partial class WatermarkWindowTests
    {
        public class PasswordBox
        {
            [OneTimeSetUp]
            public void OneTimeSetUp()
            {
                ImageAssert.OnFail = OnFail.SaveImageToTemp;
            }

            [Test]
            public void WithDefaultWatermarkWhenNotFocused()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindPasswordBox("PasswordBoxWithDefaultWatermark");
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxWithDefaultWatermark_not_focused.png", passwordBox);
                }
            }

            [Test]
            public void WithDefaultWatermarkWhenFocused()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindPasswordBox("PasswordBoxWithDefaultWatermark");
                    passwordBox.Focus();
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxWithDefaultWatermark_focused.png", passwordBox);
                }
            }

            [Test]
            public void WithDefaultWatermarkWhenNotEmpty()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindPasswordBox("PasswordBoxWithDefaultWatermark");
                    passwordBox.SetValue("abc");
                    window.FindButton("Lose focus").Click();
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxWithDefaultWatermark_not_empty.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkWithBoundText()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkWithBoundText");
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxWithWatermarkWithBoundText_AAA.png", passwordBox);
                    window.FindTextBox("AdornerText").Text = "abc";
                    window.FindButton("Lose focus").Invoke();
                    Wait.For(TimeSpan.FromMilliseconds(50));
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxWithWatermarkWithBoundText_abc.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkWithInheritedFontSize()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkWithInheritedFontSize");
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxWithWatermarkWithInheritedFontSize.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkWithExplicitTextStyle()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkWithExplicitTextStyle");
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxWithWatermarkWithExplicitTextStyle.png", passwordBox);
                }
            }

            [Test]
            public void WithInheritedTextStyle()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var groupBox = window.FindGroupBox("Inherited style");
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxesWithInheritedTextStyle.png", groupBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmpty()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkVisibleWhenEmpty");
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxWithWatermarkVisibleWhenEmpty_not_focused.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyWhenFocused()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkVisibleWhenEmpty");
                    passwordBox.Focus();
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxWithWatermarkVisibleWhenEmpty_focused.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyWhenNotEmpty()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkVisibleWhenEmpty");
                    passwordBox.SetValue("abc");
                    window.FindButton("Lose focus").Click();
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxWithWatermarkVisibleWhenEmpty_not_empty.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocused()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxWithWatermarkVisibleWhenEmptyAndNotFocused_not_focused.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocusedWhenFocused()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                    passwordBox.Focus();
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxWithWatermarkVisibleWhenEmptyAndNotFocused_focused.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocusedWhenNotEmpty()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindPasswordBox("PasswordBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                    passwordBox.SetValue("abc");
                    window.FindButton("Lose focus").Click();
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxWithWatermarkVisibleWhenEmptyAndNotFocused_not_empty.png", passwordBox);
                }
            }

            [TestCase("Collapsed")]
            [TestCase("Hidden")]
            public void WhenAdornedElementIs(string visibility)
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    Wait.For(TimeSpan.FromMilliseconds(200));
                    var button = window.FindPasswordBox("PasswordBoxWithDefaultWatermark");
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxWithDefaultWatermark_not_focused.png", button);

                    var comboBox = window.FindComboBox("VisibilityCbx");
                    comboBox.Select(visibility);
                    Wait.For(TimeSpan.FromMilliseconds(200));

                    // Checking that we don't crash here. See issue #24
                    comboBox.Select("Visible");
                    Wait.For(TimeSpan.FromMilliseconds(200));
                    ImageAssert.AreEqual(".\\Images\\PasswordBoxWithDefaultWatermark_not_focused.png", button);
                }
            }
        }
    }
}
