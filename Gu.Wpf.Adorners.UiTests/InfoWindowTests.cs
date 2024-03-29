namespace Gu.Wpf.Adorners.UiTests;

using System;
using Gu.Wpf.UiAutomation;
using NUnit.Framework;

public class InfoWindowTests
{
    private const string ExeFileName = "Gu.Wpf.Adorners.Demo.exe";
    private const string WindowName = "InfoWindow";

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        // Close the shared window after the last test.
        Application.KillLaunched(ExeFileName, WindowName);
    }

    [TestCase("red border bound visibility", "Images\\InfoWindow\\Red_border_bound_visibility_visible.png")]
    [TestCase("red border default visibility", "Images\\InfoWindow\\Red_border_default_visibility.png")]
    public void Overlay(string name, string imageFileName)
    {
        using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var button = window.FindButton(name);
        ImageAssert.AreEqual(imageFileName, button, TestImage.OnFail);
    }

    [Test]
    public void BoundVisibility()
    {
        using var app = Application.Launch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var button = window.FindButton("red border bound visibility");
        ImageAssert.AreEqual("Images\\InfoWindow\\Red_border_bound_visibility_visible.png", button, TestImage.OnFail);

        window.FindToggleButton("IsVisibleToggleButton").IsChecked = false;
        ImageAssert.AreEqual("Images\\InfoWindow\\Red_border_bound_visibility_not_visible.png", button, TestImage.OnFail);
    }

    [Test]
    public void WhenSizeChanges()
    {
        using var app = Application.Launch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var button = window.FindButton("red border default visibility");
        ImageAssert.AreEqual("Images\\InfoWindow\\Red_border_default_visibility.png", button, TestImage.OnFail);

        window.FindSlider("WidthSlider").Value = 100;
        ImageAssert.AreEqual("Images\\InfoWindow\\Red_border_default_visibility_width_100.png", button, TestImage.OnFail);
    }

    [Test]
    public void WhenDrawsOutside()
    {
        using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var groupBox = window.FindGroupBox("Draws outside");
        ImageAssert.AreEqual("Images\\InfoWindow\\Draws_outside.png", groupBox, TestImage.OnFail);
    }

    [TestCase("Collapsed")]
    [TestCase("Hidden")]
    public void WhenAdornedElementIs(string visibility)
    {
        using var app = Application.Launch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var button = window.FindButton("red border default visibility");
        ImageAssert.AreEqual("Images\\InfoWindow\\Red_border_default_visibility.png", button, TestImage.OnFail);

        var comboBox = window.FindComboBox("VisibilityCbx");
        comboBox.Select(visibility);
        Wait.For(TimeSpan.FromMilliseconds(200));

        // Checking that we don't crash here. See issue #24
        comboBox.Select("Visible");
        Wait.For(TimeSpan.FromMilliseconds(200));
        ImageAssert.AreEqual("Images\\InfoWindow\\Red_border_default_visibility.png", button, TestImage.OnFail);
    }
}
