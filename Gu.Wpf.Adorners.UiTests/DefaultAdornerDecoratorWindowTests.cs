namespace Gu.Wpf.Adorners.UiTests;

using Gu.Wpf.UiAutomation;
using NUnit.Framework;

public class DefaultAdornerDecoratorWindowTests
{
    private const string ExeFileName = "Gu.Wpf.Adorners.Demo.exe";
    private const string WindowName = "DefaultAdornerDecoratorWindow";

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        // Close the shared window after the last test.
        Application.KillLaunched(ExeFileName, WindowName);
    }

    [Test]
    public void DefaultTextBoxWaterMark()
    {
        using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var textBox = window.FindTextBox("Default");
        ImageAssert.AreEqual($"Images\\TextBoxWindow\\{TestImage.CurrentFolder}\\Default.png", textBox, TestImage.OnFail);
    }

    [Test]
    public void DefaultWithZeroBorderTextBoxWaterMark()
    {
        using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var textBox = window.FindTextBox("DefaultWithZeroBorder");
        ImageAssert.AreEqual($"Images\\TextBoxWindow\\{TestImage.CurrentFolder}\\Default_zero_border.png", textBox, TestImage.OnFail);
    }

    [Test]
    public void DefaultComboBoxWatermark()
    {
        using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var comboBox = window.FindComboBox("Default");
        ImageAssert.AreEqual($"Images\\ComboBoxWindow\\{TestImage.CurrentFolder}\\Default.png", comboBox, TestImage.OnFail);
    }

    [Test]
    public void DefaultEditableComboBoxWatermark()
    {
        using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var comboBox = window.FindComboBox("DefaultEditable");
        ImageAssert.AreEqual($"Images\\ComboBoxWindow\\{TestImage.CurrentFolder}\\Default_editable.png", comboBox, TestImage.OnFail);
    }

    [Test]
    public void OverlaidButton()
    {
        using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var comboBox = window.FindButton("Overlaid button");
        ImageAssert.AreEqual($"Images\\DefaultAdornerDecoratorWindow\\{TestImage.CurrentFolder}\\Overlaid_button.png", comboBox, TestImage.OnFail);
    }

    [Test]
    public void InfoButton()
    {
        using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var comboBox = window.FindButton("Info button");
        ImageAssert.AreEqual($"Images\\DefaultAdornerDecoratorWindow\\{TestImage.CurrentFolder}\\Info_button.png", comboBox, TestImage.OnFail);
    }
}
