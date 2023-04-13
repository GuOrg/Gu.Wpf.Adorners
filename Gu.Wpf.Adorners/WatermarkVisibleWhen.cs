namespace Gu.Wpf.Adorners;

/// <summary>
/// For specifying how visibility of a <see cref="WatermarkAdorner"/> behaves.
/// </summary>
public enum WatermarkVisibleWhen
{
    /// <summary>
    /// Shows the watermark as long as the <see cref="System.Windows.Controls.TextBox"/> is empty.
    /// </summary>
    Empty,

    /// <summary>
    ///  Shows the watermark as long as the <see cref="System.Windows.Controls.TextBox"/> is empty and not focused.
    /// </summary>
    EmptyAndNotKeyboardFocused,

    /// <summary>
    ///  Don't show the watermark.
    /// </summary>
    Never,
}
