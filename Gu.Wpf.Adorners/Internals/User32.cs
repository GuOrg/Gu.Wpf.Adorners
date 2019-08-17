namespace Gu.Wpf.Adorners
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Media;

    internal class User32
    {
        internal static Point GetMousePosition()
        {
            var p = default(Win32Point);
            _ = GetCursorPos(ref p);
            return new Point(p.X, p.Y);
        }

        internal static Point GetMousePosition(Visual relativeTo)
        {
            var p = default(Win32Point);
            _ = GetCursorPos(ref p);
            return relativeTo.PointFromScreen(new Point(p.X, p.Y));
        }

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
        private struct Win32Point
        {
            internal int X;
            internal int Y;
        }
    }
}
