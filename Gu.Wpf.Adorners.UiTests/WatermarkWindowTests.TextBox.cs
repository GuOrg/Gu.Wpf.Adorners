namespace Gu.Wpf.Adorners.UiTests
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public partial class WatermarkWindowTests
    {
        public class TextBox
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
                    var textBox = window.FindTextBox("TextBoxWithDefaultWatermark");
                    ImageAssert.AreEqual(".\\Images\\TextBoxWithDefaultWatermark_not_focused.png", textBox);
                }
            }

            [Test]
            public void WithDefaultWatermarkWhenFocused()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithDefaultWatermark");
                    textBox.Focus();
                    ImageAssert.AreEqual(".\\Images\\TextBoxWithDefaultWatermark_focused.png", textBox);
                }
            }

            [Test]
            public void WithDefaultWatermarkWhenNotEmpty()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithDefaultWatermark");
                    textBox.Text = "abc";
                    window.FindButton("Lose focus").Click();
                    ImageAssert.AreEqual(".\\Images\\TextBoxWithDefaultWatermark_not_empty.png", textBox);
                }
            }

            [Test]
            public void WithWatermarkWithBoundText()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkWithBoundText");
                    ImageAssert.AreEqual(".\\Images\\TextBoxWithWatermarkWithBoundText_AAA.png", textBox);
                    window.FindTextBox("AdornerText").Text = "abc";
                    window.FindButton("Lose focus").Invoke();
                    Wait.For(TimeSpan.FromMilliseconds(50));
                    ImageAssert.AreEqual(".\\Images\\TextBoxWithWatermarkWithBoundText_abc.png", textBox);
                }
            }

            [Test]
            public void WithTextBoxWithWatermarkWithInheritedFontSize()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkWithInheritedFontSize");
                    ImageAssert.AreEqual(".\\Images\\TextBoxWithWatermarkWithInheritedFontSize.png", textBox);
                }
            }

            [Test]
            public void WithWatermarkWithExplicitTextStyle()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkWithExplicitTextStyle");
                    ImageAssert.AreEqual(".\\Images\\TextBoxWithWatermarkWithExplicitTextStyle.png", textBox);
                }
            }

            [Test]
            public void WithInheritedTextStyle()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var groupBox = window.FindGroupBox("Inherited style");
                    ImageAssert.AreEqual(".\\Images\\TextBoxesWithInheritedTextStyle.png", groupBox);
                }
            }

            [Test]
            public void WhenTextBoxWithWatermarkVisibleWhenEmpty()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkVisibleWhenEmpty");
                    ImageAssert.AreEqual(".\\Images\\TextBoxWithWatermarkVisibleWhenEmpty_not_focused.png", textBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyWhenFocused()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkVisibleWhenEmpty");
                    textBox.Focus();
                    ImageAssert.AreEqual(".\\Images\\TextBoxWithWatermarkVisibleWhenEmpty_focused.png", textBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyWhenNotEmpty()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkVisibleWhenEmpty");
                    textBox.Text = "abc";
                    window.FindButton("Lose focus").Click();
                    ImageAssert.AreEqual(".\\Images\\TextBoxWithWatermarkVisibleWhenEmpty_not_empty.png", textBox);
                }
            }

            [Test]
            public void WhenTextBoxWithWatermarkVisibleWhenEmptyAndNotFocused()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                    ImageAssert.AreEqual(".\\Images\\TextBoxWithWatermarkVisibleWhenEmptyAndNotFocused_not_focused.png", textBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocusedWhenFocused()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                    textBox.Focus();
                    ImageAssert.AreEqual(".\\Images\\TextBoxWithWatermarkVisibleWhenEmptyAndNotFocused_focused.png", textBox);
                }
            }

            [Test]
            public void WithWatermarkVisibleWhenEmptyAndNotFocusedWhenNotEmpty()
            {
                using (var app = Application.Launch(ExeFileName, WindowName))
                {
                    var window = app.MainWindow;
                    var textBox = window.FindTextBox("TextBoxWithWatermarkVisibleWhenEmptyAndNotFocused");
                    textBox.Text = "abc";
                    window.FindButton("Lose focus").Click();
                    ImageAssert.AreEqual(".\\Images\\TextBoxWithWatermarkVisibleWhenEmptyAndNotFocused_not_empty.png", textBox);
                }
            }
        }
    }
}
