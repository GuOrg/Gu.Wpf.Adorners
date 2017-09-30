namespace Gu.Wpf.Adorners.UiTests
{
    using System.Drawing;
    using System.IO;

    public static class BitmapExt
    {
        public static void SaveToTemp(this Bitmap bitmap, string fileName)
        {
            bitmap.Save(Path.Combine(Path.GetTempPath(), Path.GetFileName(fileName)));
        }
    }
}