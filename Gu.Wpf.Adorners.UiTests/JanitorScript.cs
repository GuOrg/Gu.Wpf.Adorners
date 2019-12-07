namespace Gu.Wpf.Adorners.UiTests
{
    using System.IO;

    public static class JanitorScript
    {
        [Script]
        public static void Rename()
        {
            var folder = @"C:\Git\_GuOrg\Gu.Wpf.Adorners\Gu.Wpf.Adorners.UiTests";
            var oldName = "Red_border_default_visibility_width_100.png";
            var newName = "Red_border_default_visibility_width_100.png";

            foreach (var file in Directory.EnumerateFiles(folder, oldName, SearchOption.AllDirectories))
            {
                File.Move(file, file.Replace(oldName, newName));
            }

            foreach (var file in Directory.EnumerateFiles(folder, "*.cs", SearchOption.AllDirectories))
            {
                File.WriteAllText(file, File.ReadAllText(file).Replace(oldName, newName));
            }
        }
    }
}
