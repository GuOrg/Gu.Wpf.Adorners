namespace Gu.Wpf.Adorners.UiTests;

using Gu.Wpf.UiAutomation;
using NUnit.Framework;

public class TextBoxWindowTests
{
    private const string ExeFileName = "Gu.Wpf.Adorners.Demo.exe";
    private const string WindowName = "TextBoxWindow";

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        // Close the shared window after the last test.
        Application.KillLaunched(ExeFileName, WindowName);
    }

    [Test]
    public void Default()
    {
        using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var textBox = window.FindTextBox("Default");
        ImageAssert.AreEqual($"Images\\TextBoxWindow\\{TestImage.CurrentFolder}\\Default.png", textBox, TestImage.OnFail);
    }

    [Test]
    public void DefaultWithZeroBorder()
    {
        using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var textBox = window.FindTextBox("DefaultWithZeroBorder");
        ImageAssert.AreEqual($"Images\\TextBoxWindow\\{TestImage.CurrentFolder}\\Default_zero_border.png", textBox, TestImage.OnFail);
    }
}
