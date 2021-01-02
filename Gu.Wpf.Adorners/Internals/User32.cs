namespace Gu.Wpf.Adorners
{
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Media;

    internal class User32
    {
        internal static Point GetMousePosition()
        {
            var p = default(NativeMethods.Win32Point);
            _ = NativeMethods.GetCursorPos(ref p);
            return new Point(p.X, p.Y);
        }

        internal static Point GetMousePosition(Visual relativeTo)
        {
            var p = default(NativeMethods.Win32Point);
            _ = NativeMethods.GetCursorPos(ref p);
            return relativeTo.PointFromScreen(new Point(p.X, p.Y));
        }

        private static class NativeMethods
        {
            [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
            [DllImport("user32.dll")]
            internal static extern bool GetCursorPos(ref Win32Point pt);

            [StructLayout(LayoutKind.Sequential)]
            internal struct Win32Point
            {
                internal int X;
                internal int Y;
            }
        }
    }
}
