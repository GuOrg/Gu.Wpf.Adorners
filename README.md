# Gu.Wpf.Adorners
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE) [![NuGet](https://img.shields.io/nuget/v/Gu.Wpf.Adorners.svg)](https://www.nuget.org/packages/Gu.Wpf.Adorners/)
[![Build Status](https://dev.azure.com/guorg/Gu.Wpf.Adorners/_apis/build/status/GuOrg.Gu.Wpf.Adorners?branchName=master)](https://dev.azure.com/guorg/Gu.Wpf.Adorners/_build/latest?definitionId=5&branchName=master)
[![Build status](https://ci.appveyor.com/api/projects/status/7jwv4kskke9kraa0?svg=true)](https://ci.appveyor.com/project/JohanLarsson/gu-wpf-adorners)

## A collection of adorners for wpf.

- [About](#about)
  - [How to install](#How-to-install)
- [Watermark](#watermark)
  - [Basic Usage](#basic-usage) 
    - [TextBox](#textbox)
    - [PasswordBox](#passwordbox)
    - [ComboBox](#combobox)
  - [Advanced Usage](#advanced-usage)
    - [Binding](#binding)
    - [Attached Properties](#attached-properties)
    - [Inheriting Style](#inheriting-style)
    - [Explicit Styling](#explicit-styling)
    - [TextStyle](#textstyle)
    - [Visibility](#visibility)
    - [Rendered Example](#rendered-example)
  - [Watermark Properties](#watermark-properties)
    - [Watermark.Text](#watermarktext)
    - [Watermark.VisibleWhen](#watermarkvisiblewhen)
    - [Watermark.TextStyle](#watermarktextstyle)
      - [Default Watermark Style](#default-watermark-style)
- [Overlay](#overlay)
  - [Attached properties](#attached-properties)
- [Info](#info)
- [DragAdorner](#dragadorner)

# About
*An Adorner is a custom FrameworkElement that is bound to a UIElement. Adorners are rendered in an AdornerLayer, which is a rendering surface that is always on top of the adorned element or a collection of adorned elements.*

With Gu.Wpf.Adorners you can Overlay / Watermark multiple controls.

## How to install

  In a WPF application, install from NuGet.

    ```powershell
    PM> install-package Gu.Wpf.Adorners
    ```

  NuGet installs the dll, and adds it as a resource to your project.

# Watermark

## Basic Usage

Add the namespace to your control.
```xaml
<UserControl ...
             xmlns:adorners="http://gu.se/Adorners">
```
### TextBox

```xaml
<TextBox adorners:Watermark.Text="Write something here" />
```

### PasswordBox

```xaml
<PasswordBox adorners:Watermark.Text="Write something here" />
```

### ComboBox

```xaml
<ComboBox adorners:Watermark.Text="Write something here">
    <ComboBoxItem>abc</ComboBoxItem>
    <ComboBoxItem>cde</ComboBoxItem>
    <ComboBoxItem>fgh</ComboBoxItem>
</ComboBox>
```

## Advanced Usage
The below examples apply to `TextBox`, `PasswordBox` and `ComboBox`.

### Binding
Instead of setting a static text as watermark, you can bind its value:
```xaml
<!--Bind to the Text property of a different Element-->
<TextBox adorners:Watermark.Text="{Binding Text, ElementName=ElementNameHere}" />

<!--Bind to a property in your ViewModel/codebehind (make sure to set Datacontext)-->
<TextBox adorners:Watermark.Text="{Binding Path=SomeProperty}" />

<!--Bind to a static resource using the Gu.Wpf.Localization localization plugin-->
<TextBox adorners:Watermark.Text="{l:Static p:Resources.Label_Password}" />
```
*For more info about the localization plugin, have a look at [Gu.Wpf.Localization](https://github.com/GuOrg/Gu.Localization)*

### Attached Properties
All properties are attached properties so you can do:
```xaml
<StackPanel adorners:Watermark.Text="Write something here"
            adorners:Watermark.TextStyle="{StaticResource AdornerTextStyle}"
            adorners:Watermark.VisibleWhen="EmptyAndNotKeyboardFocused">
    <TextBox Text="{Binding SomeProp}"/>
    <TextBox Text="{Binding SomeOtherProp}" />
</StackPanel>
```

### Inheriting Style

The Watermark inherits the following styles from the UIElement:
- `FontFamily`
- `FontStyle`
- `FontWeight`
- `FontStretch`
- `FontSize`
- `Foreground`
- `TextEffects`

The below example will show a font 32 and bold watermark.
```xaml
<TextBox adorners:Watermark.Text="Foo"
         FontSize="32"
         FontWeight="Bold"/>
```

### Explicit Styling

Beside inheriting style, you can explicitly set it.
```xaml
<UserControl.Resources>
    <Style x:Key="AdornerTextStyle" TargetType="{x:Type TextBlock}">
        <Setter Property="Foreground" Value="Green" />
        <Setter Property="Opacity" Value="1" />
    </Style>
</UserControl.Resources>
...
<Grid>
    <!--Style is set explicitly. The Watermark will render with a green Foreground.-->
    <TextBox adorners:Watermark.Text="Explicit style"
             adorners:Watermark.TextStyle="{StaticResource AdornerTextStyle}" />
</Grid>
```

By setting the `adorners:Watermark` to a ContentControl/Panel (Grid, GroupBox, StackPanel, etc.), all TextBox, PasswordBox and ComboBox children inherit the value.
```xaml
<UserControl.Resources>
    <Style x:Key="AdornerTextStyle" TargetType="{x:Type TextBlock}">
        <Setter Property="Foreground" Value="Green" />
        <Setter Property="Opacity" Value="1" />
    </Style>
    <Style TargetType="{x:Type GroupBox}">
        <Setter Property="BorderBrush" Value="Blue" />
        <Setter Property="BorderThickness" Value="1" />
    </Style>
</UserControl.Resources>
...
<Grid>
    <!--By setting the Watermark.TextStyle on the ContentContol/Panel, all children inherit the TextStyle.-->
    <!--Same goes for Text and VisibleWhen (see below)-->
    <GroupBox adorners:Watermark.Text="Inherited style"
              adorners:Watermark.TextStyle="{StaticResource AdornerTextStyle}"
              Header="Inherited style">
        <StackPanel>
            <TextBox />
            <TextBox />
        </StackPanel>
    </GroupBox>

    <!--Both TextBox will render with the same Watermark Text: "Inherited text"-->
    <GroupBox adorners:Watermark.Text="Inherited text" Header="Inherited text">
        <StackPanel>
            <TextBox />
            <TextBox />
        </StackPanel>
    </GroupBox>

    <!--The top 3 elements inherit the same Watermark Text, the last one will use its own.-->
    <GroupBox adorners:Watermark.Text="Inherited text" Header="Inherited text">
        <StackPanel>
            <TextBox />
            <PasswordBox />
            <ComboBox />
            <TextBox adorners:Watermark.Text="This textbox does not inherit" />
        </StackPanel>
    </GroupBox>
</Grid>
```

### TextStyle
TextStyle accepts a style for `TextBlock` the text is drawn where the textbox text is drawn so no margins needed.

```xaml
<PasswordBox adorners:Watermark.Text="PASSWORD">
    <adorners:Watermark.TextStyle>
        <Style TargetType="{x:Type TextBlock}">
            <Setter Property="Opacity" Value="0.5" />
            <Setter Property="FontStyle" Value="Oblique" />
            <Setter Property="VerticalAlignment" Value="Center" />
        </Style>
    </adorners:Watermark.TextStyle>
</PasswordBox>
```

### Visibility
The behaviour of the watermark can be set with the `VisibleWhen` property.

```xaml
<!--The watermark shows as long as the control has no value.-->
<TextBox adorners:Watermark.Text="visible when empty"
         adorners:Watermark.VisibleWhen="Empty" />

<!--The watermark shows as long as the control has no value and is not focused.-->
<TextBox adorners:Watermark.Text="visible when not keyboard focused (default)"
         adorners:Watermark.VisibleWhen="EmptyAndNotKeyboardFocused" />
```


### Rendered Example
The above examples render to the following visualisation:

![watermarked](http://i.imgur.com/CGMrn3S.gif)

## Watermark Properties

### Watermark.Text
The text displayed as watermark in the UIElement.

### Watermark.VisibleWhen
- `Empty`
- `EmptyAndNotKeyboardFocused` *(Default)*

### Watermark.TextStyle
TextStyle accepts a style for `TextBlock` the text is drawn where the textbox text is drawn so no margins needed.

#### Default Watermark Style
```xaml
<Style TargetType="{x:Type local:WatermarkAdorner}">
    <Setter Property="IsHitTestVisible" Value="False" />
    <Setter Property="Focusable" Value="False" />
    <Setter Property="TextElement.FontFamily" Value="{Binding AdornedTextBox.FontFamily, RelativeSource={RelativeSource Self}}" />
    <Setter Property="TextElement.FontStyle" Value="{Binding AdornedTextBox.FontStyle, RelativeSource={RelativeSource Self}}" />
    <Setter Property="TextElement.FontWeight" Value="{Binding AdornedTextBox.FontWeight, RelativeSource={RelativeSource Self}}" />
    <Setter Property="TextElement.FontStretch" Value="{Binding AdornedTextBox.FontStretch, RelativeSource={RelativeSource Self}}" />
    <Setter Property="TextElement.FontSize" Value="{Binding AdornedTextBox.FontSize, RelativeSource={RelativeSource Self}}" />
    <Setter Property="TextElement.Foreground" Value="{Binding AdornedTextBox.Foreground, RelativeSource={RelativeSource Self}}" />
    <Setter Property="TextElement.TextEffects" Value="{Binding Path=(TextElement.TextEffects), RelativeSource ={RelativeSource Self}}" />

    <Setter Property="TextStyle">
        <Setter.Value>
            <Style TargetType="{x:Type TextBlock}">
                <Setter Property="Opacity" Value="0.5" />
                <Setter Property="FontStyle" Value="Oblique" />
            </Style>
        </Setter.Value>
    </Setter>
</Style>
```


# Overlay

For adding an overlay to an element.
The overlay visibility is controlled with adorners:Overlay.IsVisible if set to null the overlay is shown if adorners:Overlay.Content != null

Sample:
```xaml
<Button adorners:Overlay.IsVisible="{Binding IsChecked,
                                             ElementName=IsVisibleButton}">
    <adorners:Overlay.Content>
        <Border BorderBrush="HotPink"
                BorderThickness="3" />
    </adorners:Overlay.Content>
</Button>
```
Renders: ![overlay](http://i.imgur.com/Csrqi6L.png)

## Attached properties
All properties are attached properties so you can do:
Note that this sample makes little sense overspecifying, providing it to give copy-paste friendly xaml.

```xaml
<StackPanel adorners:Overlay.ContentPresenterStyle="{StaticResource OverlayStyle}"
            adorners:Overlay.ContentTemplateSelector="{StaticResource OverlayTemplateSelector}">
    <adorners:Overlay.ContentTemplate>
        <DataTemplate>
            <Border BorderBrush="GreenYellow"
                    BorderThickness="3">
                <TextBlock HorizontalAlignment="Center"
                            VerticalAlignment="Center"
                            Text="{Binding}" />
            </Border>
        </DataTemplate>
    </adorners:Overlay.ContentTemplate>

    <Button Width="100"
            Height="100"
            Margin="5"
            Foreground="Yellow" />

    <Button Width="100"
            Height="100"
            Margin="5"
            Foreground="Blue" />
</StackPanel>
```

# Info

This is very similar to the adorner used for validation in WPF

Sample:

```xaml
<Button adorners:Info.IsVisible="{Binding IsChecked,
                                          ElementName=IsVisibleButton}">
    <adorners:Info.Template>
        <ControlTemplate>
            <Grid>
                <Grid.RowDefinitions>
                    <RowDefinition Height="Auto" />
                    <RowDefinition Height="Auto" />
                </Grid.RowDefinitions>
                <Border HorizontalAlignment="Right"
                        BorderBrush="Blue"
                        BorderThickness="0,0,0,1">
                    <AdornedElementPlaceholder />
                </Border>
                <TextBlock Grid.Row="1"
                            HorizontalAlignment="Right"
                            Text="Some info text"
                            TextAlignment="Right" />
            </Grid>
        </ControlTemplate>
    </adorners:Info.Template>
</Button>
```
Renders: ![info](http://i.imgur.com/9ODbtO9.png)

The DataContext of the adorner is bound to DataContext of AdornedElement.

# DragAdorner

Shows an adorner that follows the mouse while draging in a drag & drop operation.

![dragadorner](https://user-images.githubusercontent.com/1640096/31116961-556f2dcc-a828-11e7-941e-d967eb04fed3.gif)

Sample:

```cs
private static bool TryGetDropTarget(object sender, out ContentPresenter target)
{
    target = null;
    if (sender is ContentPresenter cp &&
        cp.Content == null)
    {
        target = cp;
    }

    return target != null;
}

private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
{
    if (e.Source is ContentPresenter contentPresenter &&
        contentPresenter.Content != null)
    {
        var data = new DataObject(typeof(DragItem), contentPresenter.Content);
        using (var adorner = DragAdorner.Create(contentPresenter))
        {
            data.SetData(typeof(Adorner), adorner);
            contentPresenter.SetCurrentValue(ContentPresenter.ContentProperty, null);
            DragDrop.DoDragDrop(contentPresenter, data, DragDropEffects.Move);
            var target = data.GetData(typeof(UIElement));
            if (target == null)
            {
                contentPresenter.SetCurrentValue(ContentPresenter.ContentProperty, data.GetData(typeof(DragItem)));
            }
        }
    }
}

private void OnDrop(object sender, DragEventArgs e)
{
    if (TryGetDropTarget(e.Source, out var contentPresenter))
    {
        contentPresenter.SetCurrentValue(ContentPresenter.ContentProperty, e.Data.GetData(typeof(DragItem)));
        e.Effects = DragDropEffects.Move;
        e.Data.SetData(typeof(UIElement), contentPresenter);
        e.Handled = true;
    }
}

private void OnDragLeave(object sender, DragEventArgs e)
{
    if (TryGetDropTarget(e.Source, out var contentPresenter) &&
        e.Data.GetData(typeof(Adorner)) is ContentDragAdorner adorner)
    {
        adorner.RemoveSnap(contentPresenter);
        e.Effects = DragDropEffects.None;
        e.Handled = true;
    }
}

private void OnDragEnter(object sender, DragEventArgs e)
{
    if (TryGetDropTarget(e.Source, out var contentPresenter) &&
        e.Data.GetData(typeof(Adorner)) is ContentDragAdorner adorner)
    {
        adorner.SnapTo(contentPresenter);
        e.Effects = DragDropEffects.Move;
        e.Handled = true;
    }
}
```
