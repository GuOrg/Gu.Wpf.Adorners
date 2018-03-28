# Gu.Wpf.Adorners
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md) [![NuGet](https://img.shields.io/nuget/v/Gu.Wpf.Adorners.svg)](https://www.nuget.org/packages/Gu.Wpf.Adorners/)
[![Build status](https://ci.appveyor.com/api/projects/status/7jwv4kskke9kraa0?svg=true)](https://ci.appveyor.com/project/JohanLarsson/gu-wpf-adorners)


## A collection of adorners for wpf.

- [Watermark](#watermark)
  - [TextStyle](#textstyle)
  - [VisibleWhen {`Empty`, `EmptyAndNotKeyboardFocused`}](#visiblewhen--empty--emptyandnotkeyboardfocused)
  - [Default style:](#default-style)
  - [Attached properties](#attached-properties)
- [Overlay](#overlay)
  - [Attached properties](#attached-properties)
- [Info](#info)
- [DragAdorner](#dragadorner)

# Watermark

For adding watermark text to text boxes, password boxes and combo boxes.

Simple sample:
```xaml
<StackPanel>
    <TextBox adorners:Watermark.Text="Write something here" />

    <PasswordBox adorners:Watermark.Text="Write something here" />

    <ComboBox adorners:Watermark.Text="Write something here">
        <ComboBoxItem>abc</ComboBoxItem>
        <ComboBoxItem>cde</ComboBoxItem>
        <ComboBoxItem>fgh</ComboBoxItem>
    </ComboBox>
</StackPanel>
```

Full sample with many combinations:

```xaml
<UserControl ...
             xmlns:adorners="http://gu.se/Adorners">
...
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

<StackPanel>
    <UniformGrid Rows="1">
        <StackPanel>
            <TextBlock Text="TextBox with watermark" />
            <TextBox x:Name="TextBoxWithDefaultWatermark" adorners:Watermark.Text="Write something here" />

            <TextBlock Text="Bound text" />
            <TextBox x:Name="TextBoxWithWatermarkWithBoundText" adorners:Watermark.Text="{Binding Text, ElementName=AdornerText}" />

            <TextBlock Text="Inherits Fontsize via default style" />
            <TextBox x:Name="TextBoxWithWatermarkWithInheritedFontSize"
                     adorners:Watermark.Text="Foo"
                     FontSize="32" />

            <TextBlock Text="Explicit style" />
            <TextBox x:Name="TextBoxWithWatermarkWithExplicitTextStyle"
                     adorners:Watermark.Text="Explicit style"
                     adorners:Watermark.TextStyle="{StaticResource AdornerTextStyle}" />

            <GroupBox adorners:Watermark.Text="Inherited style"
                      adorners:Watermark.TextStyle="{StaticResource AdornerTextStyle}"
                      Header="Inherited style">
                <StackPanel>
                    <TextBox />
                    <TextBox />
                </StackPanel>
            </GroupBox>

            <GroupBox adorners:Watermark.Text="Inherited text" Header="Inherited text">
                <StackPanel>
                    <TextBox />
                    <TextBox />
                </StackPanel>
            </GroupBox>

            <TextBlock Text="VisibleWhen=Empty" />
            <TextBox x:Name="TextBoxWithWatermarkVisibleWhenEmpty"
                     adorners:Watermark.Text="visible when empty"
                     adorners:Watermark.VisibleWhen="Empty" />

            <TextBlock Text="VisibleWhen=EmptyAndNotKeyboardFocused" />
            <TextBox x:Name="TextBoxWithWatermarkVisibleWhenEmptyAndNotFocused"
                     adorners:Watermark.Text="visible when not keyboard focused (default)"
                     adorners:Watermark.VisibleWhen="EmptyAndNotKeyboardFocused" />
        </StackPanel>

        <StackPanel>
            <TextBlock Text="PasswordBox with watermark" />
            <PasswordBox x:Name="PasswordBoxWithDefaultWatermark" adorners:Watermark.Text="Write something here" />

            <TextBlock Text="Bound text" />
            <PasswordBox x:Name="PasswordBoxWithWatermarkWithBoundText" adorners:Watermark.Text="{Binding Text, ElementName=AdornerText}" />

            <TextBlock Text="Inherits Fontsize via default style" />
            <PasswordBox x:Name="PasswordBoxWithWatermarkWithInheritedFontSize"
                         adorners:Watermark.Text="Foo"
                         FontSize="32" />

            <TextBlock Text="Explicit style" />
            <PasswordBox x:Name="PasswordBoxWithWatermarkWithExplicitTextStyle"
                         adorners:Watermark.Text="Explicit style"
                         adorners:Watermark.TextStyle="{StaticResource AdornerTextStyle}" />

            <GroupBox adorners:Watermark.Text="Inherited style"
                      adorners:Watermark.TextStyle="{StaticResource AdornerTextStyle}"
                      Header="Inherited style">
                <StackPanel>
                    <PasswordBox />
                    <PasswordBox />
                </StackPanel>
            </GroupBox>

            <GroupBox adorners:Watermark.Text="Inherited text" Header="Inherited text">
                <StackPanel>
                    <PasswordBox />
                    <PasswordBox />
                </StackPanel>
            </GroupBox>

            <TextBlock Text="VisibleWhen=Empty" />
            <PasswordBox x:Name="PasswordBoxWithWatermarkVisibleWhenEmpty"
                         adorners:Watermark.Text="visible when empty"
                         adorners:Watermark.VisibleWhen="Empty" />

            <TextBlock Text="VisibleWhen=EmptyAndNotKeyboardFocused" />
            <PasswordBox x:Name="PasswordBoxWithWatermarkVisibleWhenEmptyAndNotFocused"
                         adorners:Watermark.Text="visible when not keyboard focused (default)"
                         adorners:Watermark.VisibleWhen="EmptyAndNotKeyboardFocused" />
        </StackPanel>

        <StackPanel>
            <TextBlock Text="ComboBox with watermark" />
            <ComboBox x:Name="ComboBoxWithDefaultWatermark" adorners:Watermark.Text="Write something here">
                <ComboBoxItem>abc</ComboBoxItem>
                <ComboBoxItem>cde</ComboBoxItem>
                <ComboBoxItem>fgh</ComboBoxItem>
            </ComboBox>

            <TextBlock Text="Bound text" />
            <ComboBox x:Name="ComboBoxWithWatermarkWithBoundText" adorners:Watermark.Text="{Binding Text, ElementName=AdornerText}">
                <ComboBoxItem>abc</ComboBoxItem>
                <ComboBoxItem>cde</ComboBoxItem>
                <ComboBoxItem>fgh</ComboBoxItem>
            </ComboBox>

            <TextBlock Text="Inherits Fontsize via default style" />
            <ComboBox x:Name="ComboBoxWithWatermarkWithInheritedFontSize"
                      adorners:Watermark.Text="Foo"
                      FontSize="32">
                <ComboBoxItem>abc</ComboBoxItem>
                <ComboBoxItem>cde</ComboBoxItem>
                <ComboBoxItem>fgh</ComboBoxItem>
            </ComboBox>

            <TextBlock Text="Explicit style" />
            <ComboBox x:Name="ComboBoxWithWatermarkWithExplicitTextStyle"
                      adorners:Watermark.Text="Explicit style"
                      adorners:Watermark.TextStyle="{StaticResource AdornerTextStyle}" />

            <GroupBox adorners:Watermark.Text="Inherited style"
                      adorners:Watermark.TextStyle="{StaticResource AdornerTextStyle}"
                      Header="Inherited style">
                <StackPanel>
                    <ComboBox>
                        <ComboBoxItem>abc</ComboBoxItem>
                        <ComboBoxItem>cde</ComboBoxItem>
                        <ComboBoxItem>fgh</ComboBoxItem>
                    </ComboBox>
                    <ComboBox>
                        <ComboBoxItem>abc</ComboBoxItem>
                        <ComboBoxItem>cde</ComboBoxItem>
                        <ComboBoxItem>fgh</ComboBoxItem>
                    </ComboBox>
                </StackPanel>
            </GroupBox>

            <GroupBox adorners:Watermark.Text="Inherited text" Header="Inherited text">
                <StackPanel>
                    <ComboBox>
                        <ComboBoxItem>abc</ComboBoxItem>
                        <ComboBoxItem>cde</ComboBoxItem>
                        <ComboBoxItem>fgh</ComboBoxItem>
                    </ComboBox>
                    <ComboBox>
                        <ComboBoxItem>abc</ComboBoxItem>
                        <ComboBoxItem>cde</ComboBoxItem>
                        <ComboBoxItem>fgh</ComboBoxItem>
                    </ComboBox>
                </StackPanel>
            </GroupBox>

            <TextBlock Text="VisibleWhen=Empty" />
            <ComboBox x:Name="ComboBoxWithWatermarkVisibleWhenEmpty"
                      adorners:Watermark.Text="visible when empty"
                      adorners:Watermark.VisibleWhen="Empty">
                <ComboBoxItem>abc</ComboBoxItem>
                <ComboBoxItem>cde</ComboBoxItem>
                <ComboBoxItem>fgh</ComboBoxItem>
            </ComboBox>

            <TextBlock Text="VisibleWhen=EmptyAndNotKeyboardFocused" />
            <ComboBox x:Name="ComboBoxWithWatermarkVisibleWhenEmptyAndNotFocused"
                      adorners:Watermark.Text="visible when not keyboard focused (default)"
                      adorners:Watermark.VisibleWhen="EmptyAndNotKeyboardFocused">
                <ComboBoxItem>abc</ComboBoxItem>
                <ComboBoxItem>cde</ComboBoxItem>
                <ComboBoxItem>fgh</ComboBoxItem>
            </ComboBox>
        </StackPanel>
    </UniformGrid>
    <TextBox x:Name="AdornerText" Text="AAA" />
    <Button Content="Lose focus" />
</StackPanel>
...
```

Renders: 
![watermarked](http://i.imgur.com/CGMrn3S.gif)

## TextStyle 
Accepts a style for `TextBlock` the text is drawn where the textbox text is drawn so no margins needed.

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

## VisibleWhen {`Empty`, `EmptyAndNotKeyboardFocused`}
Default is `EmptyAndNotKeyboardFocused`

## Default style:
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

## Attached properties
All properties are attached properties so you can do:
```xaml
<StackPanel adorners:Watermark.Text="Write something here"
            adorners:Watermark.TextStyle="{StaticResource AdornerTextStyle}"
            adorners:Watermark.VisibleWhen="EmptyAndNotKeyboardFocused">
    <TextBox Text="{Binding SomeProp}"/>
    <TextBox Text="{Binding SomeOtherProp}" />
</StackPanel>
```

# Overlay

For adding an overlay to an element.
The overlay visibility is controled with adorners:Overlay.IsVisible if set to null the overlay is shown if adorners:Overlay.Content != null

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
