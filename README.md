# Gu.Wpf.Adorners

A collection of adorners for wpf.

## Watermark
Sample:
```
<TextBox adorners:Watermark.Text="Write something here" 
         adorners:Watermark.TextStyle="{StaticResource AdornerTextStyle}"
         adorners:Watermark.VisibleWhen="EmptyAndNotKeyboardFocused"/>
```

Renders: ![watermarked](http://i.imgur.com/HppBPBC.png)

##### TextStyle 
Accepts a style for `TextBlock` the text is drawn where the textbox text is drawn so no margins needed.

##### VisibleWhen {`Empty`, `EmptyAndNotKeyboardFocused`}
Default is `EmptyAndNotKeyboardFocused`

##### Default style:
```
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

##### Attached properties
All properties are attached properties so you can do:
```
<StackPanel adorners:Watermark.Text="Write something here"
            adorners:Watermark.TextStyle="{StaticResource AdornerTextStyle}"
            adorners:Watermark.VisibleWhen="EmptyAndNotKeyboardFocused">
    <TextBox Text="{Binding SomeProp}"/>
    <TextBox Text="{Binding SomeOtherProp}" />
</StackPanel>
```

## Overlay
The overlay visibility is controled with adorners:Overlay.IsVisible if set to null the overlay is shown if adorners:Overlay.Content != null
Sample:
```
<Button Width="100"
        Height="100"
        Margin="5"
        adorners:Overlay.IsVisible="{Binding IsChecked,
                                             ElementName=IsVisibleButton}">
    <adorners:Overlay.Content>
        <Border BorderBrush="HotPink"
                BorderThickness="3" />
    </adorners:Overlay.Content>
</Button>
```
Renders: ![watermarked](http://i.imgur.com/HppBPBC.png)

##### Attached properties
All properties are attached properties so you can do:
Note that this sample makes little sense overspecifying, providing it to give copy-paste friendly xaml.
```
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
