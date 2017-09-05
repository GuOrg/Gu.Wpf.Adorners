namespace Gu.Wpf.Adorners.UiTests
{
    using System;
    using System.IO;
    using System.Reflection;

    public static class Info
    {
        public static string ExeFileName { get; } = GetExeFileName();

        private static string GetExeFileName()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var testDirectory = Path.GetDirectoryName(new Uri(assembly.CodeBase).LocalPath);
            var assemblyName = assembly.GetName().Name;
            var exeDirectoryName = assemblyName.Replace("UiTests", "Demo");
            //// ReSharper disable once PossibleNullReferenceException
            var exeDirectory = testDirectory.Replace(assemblyName, exeDirectoryName);
            var fileName = Path.Combine(exeDirectory, "Gu.Wpf.Adorners.Demo.exe");
            return fileName;
        }
    }
}
