namespace Gu.Wpf.Adorners
{
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Media;

    public class User32
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Win32Point
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref Win32Point pt);

        public static Point GetMousePosition()
        {
            var p = default(Win32Point);
            GetCursorPos(ref p);
            return new Point(p.X, p.Y);
        }

        public static Point GetMousePosition(Visual relativeTo)
        {
            var p = default(Win32Point);
            GetCursorPos(ref p);
            return relativeTo.PointFromScreen(new Point(p.X, p.Y));
        }
    }
}