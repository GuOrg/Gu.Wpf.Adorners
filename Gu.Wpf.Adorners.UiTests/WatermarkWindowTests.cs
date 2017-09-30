namespace Gu.Wpf.Adorners.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class WatermarkWindowTests
    {
        private const string WindowName = "WatermarkWindow";

        [Test]
        public void DefaultAdornerWhenNotFocused()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("WithDefaultAdorner");
                ImageAssert.AreEqual(".\\Images\\WithDefaultAdorner_not_focused.png", textBox);
            }
        }

        [Test]
        public void DefaultAdornerWhenFocused()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("WithDefaultAdorner");
                textBox.Focus();
                ImageAssert.AreEqual(".\\Images\\WithDefaultAdorner_focused.png", textBox);
            }
        }

        [Test]
        public void DefaultAdornerWhenNotEmpty()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("WithDefaultAdorner");
                textBox.Text = "abc";
                window.FindButton("Lose focus").Click();
                ImageAssert.AreEqual(".\\Images\\WithDefaultAdorner_not_empty.png", textBox);
            }
        }

        [Test]
        public void WithBoundText()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("WithBoundAdornerText");
                ImageAssert.AreEqual(".\\Images\\WithBoundAdornerText_AAA.png", textBox);
                window.FindTextBox("AdornerText").Text = "abc";
                window.FindButton("Lose focus").Click();
                ImageAssert.AreEqual(".\\Images\\WithBoundAdornerText_abc.png", textBox);
            }
        }

        [Test]
        public void WithWithInheritedFontSize()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("WithInheritedFontSize");
                ImageAssert.AreEqual(".\\Images\\WithInheritedFontSize.png", textBox);
            }
        }

        [Test]
        public void WithExplicitTextStyle()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("WithExplicitTextStyle");
                ImageAssert.AreEqual(".\\Images\\WithExplicitTextStyle.png", textBox);
            }
        }

        [Test]
        public void WithInheritedTextStyle()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var groupBox = window.FindGroupBox("Inherited style");
                ImageAssert.AreEqual(".\\Images\\WithInheritedTextStyle.png", groupBox);
            }
        }

        [Test]
        public void WhenVisibleWhenEmpty()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("VisibleWhenEmpty");
                ImageAssert.AreEqual(".\\Images\\VisibleWhenEmpty_not_focused.png", textBox);
            }
        }

        [Test]
        public void VisibleWhenEmptyWhenFocused()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
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
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
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
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox("VisibleWhenEmptyAndNotFocused");
                ImageAssert.AreEqual(".\\Images\\VisibleWhenEmptyAndNotFocused_not_focused.png", textBox);
            }
        }

        [Test]
        public void VisibleWhenEmptyAndNotFocusedWhenFocused()
        {
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
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
            using (var app = Application.Launch(Info.ExeFileName, WindowName))
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