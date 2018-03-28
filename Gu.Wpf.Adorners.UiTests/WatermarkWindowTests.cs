namespace Gu.Wpf.Adorners.UiTests
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class WatermarkWindowTests
    {
        private const string ExeFileName = "Gu.Wpf.Adorners.Demo.exe";
        private const string WindowName = "WatermarkWindow";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ImageAssert.OnFail = OnFail.SaveImageToTemp;
        }

        [Test]
        public void TextBoxWithDefaultWaterMarkWhenNotFocused()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("TextBoxWithDefaultWaterMark");
                ImageAssert.AreEqual(".\\Images\\TextBoxWithDefaultWaterMark_not_focused.png", textBox);
            }
        }

        [Test]
        public void TextBoxWithDefaultWaterMarkWhenFocused()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("TextBoxWithDefaultWaterMark");
                textBox.Focus();
                ImageAssert.AreEqual(".\\Images\\TextBoxWithDefaultWaterMark_focused.png", textBox);
            }
        }

        [Test]
        public void TextBoxWithDefaultWaterMarkWhenNotEmpty()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("TextBoxWithDefaultWaterMark");
                textBox.Text = "abc";
                window.FindButton("Lose focus").Click();
                ImageAssert.AreEqual(".\\Images\\TextBoxWithDefaultWaterMark_not_empty.png", textBox);
            }
        }

        [Test]
        public void TextBoxWithWaterMarkWithBoundText()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("TextBoxWithWaterMarkWithBoundText");
                ImageAssert.AreEqual(".\\Images\\TextBoxWithWaterMarkWithBoundText_AAA.png", textBox);
                window.FindTextBox("AdornerText").Text = "abc";
                window.FindButton("Lose focus").Invoke();
                Wait.For(TimeSpan.FromMilliseconds(50));
                ImageAssert.AreEqual(".\\Images\\TextBoxWithWaterMarkWithBoundText_abc.png", textBox);
            }
        }

        [Test]
        public void WithWithInheritedFontSize()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("WithInheritedFontSize");
                ImageAssert.AreEqual(".\\Images\\WithInheritedFontSize.png", textBox);
            }
        }

        [Test]
        public void WithExplicitTextStyle()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("WithExplicitTextStyle");
                ImageAssert.AreEqual(".\\Images\\WithExplicitTextStyle.png", textBox);
            }
        }

        [Test]
        public void WithInheritedTextStyle()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var groupBox = window.FindGroupBox("Inherited style");
                ImageAssert.AreEqual(".\\Images\\WithInheritedTextStyle.png", groupBox);
            }
        }

        [Test]
        public void WhenVisibleWhenEmpty()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("VisibleWhenEmpty");
                ImageAssert.AreEqual(".\\Images\\VisibleWhenEmpty_not_focused.png", textBox);
            }
        }

        [Test]
        public void VisibleWhenEmptyWhenFocused()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("VisibleWhenEmpty");
                textBox.Focus();
                ImageAssert.AreEqual(".\\Images\\VisibleWhenEmpty_focused.png", textBox);
            }
        }

        [Test]
        public void VisibleWhenEmptyWhenNotEmpty()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("VisibleWhenEmpty");
                textBox.Text = "abc";
                window.FindButton("Lose focus").Click();
                ImageAssert.AreEqual(".\\Images\\VisibleWhenEmpty_not_empty.png", textBox);
            }
        }

        [Test]
        public void WhenVisibleWhenEmptyAndNotFocused()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("VisibleWhenEmptyAndNotFocused");
                ImageAssert.AreEqual(".\\Images\\VisibleWhenEmptyAndNotFocused_not_focused.png", textBox);
            }
        }

        [Test]
        public void VisibleWhenEmptyAndNotFocusedWhenFocused()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("VisibleWhenEmptyAndNotFocused");
                textBox.Focus();
                ImageAssert.AreEqual(".\\Images\\VisibleWhenEmptyAndNotFocused_focused.png", textBox);
            }
        }

        [Test]
        public void VisibleWhenEmptyAndNotFocusedWhenNotEmpty()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("VisibleWhenEmptyAndNotFocused");
                textBox.Text = "abc";
                window.FindButton("Lose focus").Click();
                ImageAssert.AreEqual(".\\Images\\VisibleWhenEmptyAndNotFocused_not_empty.png", textBox);
            }
        }
    }
}
