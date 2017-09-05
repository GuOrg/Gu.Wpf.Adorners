namespace Gu.Wpf.Adorners.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class MainWindowTests
    {
        [Test]
        public void ClickAllTabs()
        {
            // Just a smoke test so that everything builds.
            using (var app = Application.Launch(Info.ExeFileName))
            {
                var window = app.MainWindow;
                var tab = window.FindTabControl();
                foreach (var tabItem in tab.Items)
                {
                    tabItem.Click();
                }
            }
        }
    }
}