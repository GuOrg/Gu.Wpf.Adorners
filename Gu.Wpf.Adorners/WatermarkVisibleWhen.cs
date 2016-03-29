namespace Gu.Wpf.Adorners
{
    public enum WatermarkVisibleWhen
    {
        /// <summary>
        /// Shows the watermark as long as the textbox is empty
        /// </summary>
        Empty,

        /// <summary>
        ///  Shows the watermark as long as the textbox is empty and not focused.
        /// </summary>
        EmptyAndNotKeyboardFocused
    }
}