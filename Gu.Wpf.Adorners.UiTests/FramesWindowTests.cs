namespace Gu.Wpf.Adorners.UiTests;

using Gu.Wpf.UiAutomation;
using NUnit.Framework;

public class FramesWindowTests
{
    private const string ExeFileName = "Gu.Wpf.Adorners.Demo.exe";
    private const string WindowName = "NotShowingWhenInFrames/FramesWindow";

    [Test]
    public void ClickAllTabs()
    {
        // Just a smoke test so we don't crash.
        using var app = Application.Launch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var tab = window.FindTabControl();
        ImageAssert.AreEqual("Images\\FramesWindow\\Tab1.png", tab.FindTextBox("Tab1"), TestImage.OnFail);

        tab.SelectedIndex = 1;
        ImageAssert.AreEqual("Images\\FramesWindow\\Tab2.png", tab.FindTextBox("Tab2"), TestImage.OnFail);

        tab.SelectedIndex = 0;
        ImageAssert.AreEqual("Images\\FramesWindow\\Tab1.png", tab.FindTextBox("Tab1"), TestImage.OnFail);

        tab.SelectedIndex = 1;
        ImageAssert.AreEqual("Images\\FramesWindow\\Tab2.png", tab.FindTextBox("Tab2"), TestImage.OnFail);
    }
}
