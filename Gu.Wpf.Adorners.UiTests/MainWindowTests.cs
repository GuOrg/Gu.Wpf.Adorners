namespace Gu.Wpf.Adorners.UiTests;

using Gu.Wpf.UiAutomation;
using NUnit.Framework;

public class MainWindowTests
{
    [Test]
    public void ClickAllTabs()
    {
        // Just a smoke test so we don't crash.
        using var app = Application.Launch("Gu.Wpf.Adorners.Demo.exe");
        var window = app.MainWindow;
        var tab = window.FindTabControl();
        foreach (var tabItem in tab.Items)
        {
            tabItem.Click();
        }
    }
}
