namespace Gu.Wpf.Adorners.UiTests
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static partial class WatermarkWindowTests
    {
        public class TextBox
        {
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

            [Test]
            public void WithDefaultWatermarkWhenNotFocused()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithDefaultWatermark");
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\Default_not_focused.png", textBox);
                }
            }

            [Test]
            public void WithDefaultWatermarkWhenFocused()
            {
                using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithDefaultWatermark");
                    textBox.Focus();
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\Default_focused.png", textBox);
                }
            }

            [Test]
            public void WithDefaultWatermarkWhenNotEmpty()
            {
                using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithDefaultWatermark");
                    textBox.Text = "abc";
                    window.FindButton("Lose focus").Click();
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\Default_not_empty.png", textBox);
                }
            }

            [Test]
            public void WithWatermarkWithBoundText()
            {
                using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkWithBoundText");
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\WithWatermarkWithBoundText_AAA.png", textBox);
                    window.FindTextBox("AdornerText").Text = "abc";
                    window.FindButton("Lose focus").Invoke();
                    Wait.For(TimeSpan.FromMilliseconds(50));
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\WithWatermarkWithBoundText_abc.png", textBox);
                }
            }

            [Test]
            public void WithWatermarkWithInheritedFontSize()
            {
                using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkWithInheritedFontSize");
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\WithWatermarkWithInheritedFontSize.png", textBox);
                }
            }

            [Test]
            public void WithWatermarkWithExplicitTextStyle()
            {
                using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkWithExplicitTextStyle");
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\WithWatermarkWithExplicitTextStyle.png", textBox);
                }
            }

            [Test]
            public void WithInheritedTextStyle()
            {
                using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var groupBox = window.FindGroupBox("Inherited style");
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\WithInheritedTextStyle.png", groupBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmpty()
            {
                using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkVisibleWhenEmpty");
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\Visible_when_empty_not_focused.png", textBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyWhenFocused()
            {
                using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkVisibleWhenEmpty");
                    textBox.Focus();
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\Visible_when_empty_focused.png", textBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyWhenNotEmpty()
            {
                using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkVisibleWhenEmpty");
                    textBox.Text = "abc";
                    window.FindButton("Lose focus").Click();
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\Visible_when_empty_not_empty.png", textBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocused()
            {
                using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\WithWatermarkVisibleWhenEmptyAndNotFocused_not_focused.png", textBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocusedWhenFocused()
            {
                using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                    textBox.Focus();
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\WithWatermarkVisibleWhenEmptyAndNotFocused_focused.png", textBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocusedWhenNotEmpty()
            {
                using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                    textBox.Text = "abc";
                    window.FindButton("Lose focus").Click();
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\WithWatermarkVisibleWhenEmptyAndNotFocused_not_empty.png", textBox);
                }
            }

            [TestCase("Collapsed")]
            [TestCase("Hidden")]
            public void WhenAdornedElementIs(string visibility)
            {
                using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    Wait.For(TimeSpan.FromMilliseconds(200));
                    var button = window.FindTextBox("TextBoxWithDefaultWatermark");
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\Default_not_focused.png", button);

                    var comboBox = window.FindComboBox("VisibilityCbx");
                    comboBox.Select(visibility);
                    Wait.For(TimeSpan.FromMilliseconds(200));

                    // Checking that we don't crash here. See issue #24
                    comboBox.Select("Visible");
                    Wait.For(TimeSpan.FromMilliseconds(200));
                    ImageAssert.AreEqual(".\\Images\\Watermark\\TextBox\\Default_not_focused.png", button);
                }
            }
        }
    }
}
