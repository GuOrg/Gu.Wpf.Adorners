namespace Gu.Wpf.Adorners.UiTests
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public partial class WatermarkWindowTests
    {
        public class ComboBox
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
                    var passwordBox = window.FindComboBox("ComboBoxWithDefaultWatermark");
                    ImageAssert.AreEqual(".\\Images\\ComboBoxWithDefaultWatermark_not_focused.png", passwordBox);
                }
            }

            [Test]
            public void WithDefaultWatermarkWhenFocused()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindComboBox("ComboBoxWithDefaultWatermark");
                    passwordBox.Focus();
                    ImageAssert.AreEqual(".\\Images\\ComboBoxWithDefaultWatermark_focused.png", passwordBox);
                }
            }

            [Test]
            public void WithDefaultWatermarkWhenNotEmpty()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindComboBox("ComboBoxWithDefaultWatermark");
                    passwordBox.Select("abc");
                    window.FindButton("Lose focus").Click();
                    ImageAssert.AreEqual(".\\Images\\ComboBoxWithDefaultWatermark_not_empty.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkWithBoundText()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindComboBox("ComboBoxWithWatermarkWithBoundText");
                    ImageAssert.AreEqual(".\\Images\\ComboBoxWithWatermarkWithBoundText_AAA.png", passwordBox);
                    window.FindTextBox("AdornerText").Text = "abc";
                    window.FindButton("Lose focus").Invoke();
                    Wait.For(TimeSpan.FromMilliseconds(50));
                    ImageAssert.AreEqual(".\\Images\\ComboBoxWithWatermarkWithBoundText_abc.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkWithInheritedFontSize()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindComboBox("ComboBoxWithWatermarkWithInheritedFontSize");
                    ImageAssert.AreEqual(".\\Images\\ComboBoxWithWatermarkWithInheritedFontSize.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkWithExplicitTextStyle()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindComboBox("ComboBoxWithWatermarkWithExplicitTextStyle");
                    ImageAssert.AreEqual(".\\Images\\ComboBoxWithWatermarkWithExplicitTextStyle.png", passwordBox);
                }
            }

            [Test]
            public void WithInheritedTextStyle()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var groupBox = window.FindGroupBox("Inherited style");
                    ImageAssert.AreEqual(".\\Images\\ComboBoxesWithInheritedTextStyle.png", groupBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmpty()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindComboBox("ComboBoxWithWatermarkVisibleWhenEmpty");
                    ImageAssert.AreEqual(".\\Images\\ComboBoxWithWatermarkVisibleWhenEmpty_not_focused.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyWhenFocused()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindComboBox("ComboBoxWithWatermarkVisibleWhenEmpty");
                    passwordBox.Focus();
                    ImageAssert.AreEqual(".\\Images\\ComboBoxWithWatermarkVisibleWhenEmpty_focused.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyWhenNotEmpty()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindComboBox("ComboBoxWithWatermarkVisibleWhenEmpty");
                    passwordBox.Select("abc");
                    window.FindButton("Lose focus").Click();
                    ImageAssert.AreEqual(".\\Images\\ComboBoxWithWatermarkVisibleWhenEmpty_not_empty.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocused()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindComboBox("ComboBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                    ImageAssert.AreEqual(".\\Images\\ComboBoxWithWatermarkVisibleWhenEmptyAndNotFocused_not_focused.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocusedWhenFocused()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindComboBox("ComboBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                    passwordBox.Focus();
                    ImageAssert.AreEqual(".\\Images\\ComboBoxWithWatermarkVisibleWhenEmptyAndNotFocused_focused.png", passwordBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocusedWhenNotEmpty()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var passwordBox = window.FindComboBox("ComboBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                    passwordBox.Select("abc");
                    window.FindButton("Lose focus").Click();
                    ImageAssert.AreEqual(".\\Images\\ComboBoxWithWatermarkVisibleWhenEmptyAndNotFocused_not_empty.png", passwordBox);
                }
            }
        }
    }
}
