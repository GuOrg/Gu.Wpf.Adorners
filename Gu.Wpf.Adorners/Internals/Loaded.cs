namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;

    internal static class Loaded
    {
        internal static bool IsLoaded(this DependencyObject element)
        {
            if (element is FrameworkElement fe)
            {
                return fe.IsLoaded;
            }

            if (element is FrameworkContentElement fce)
            {
                return fce.IsLoaded;
            }

            throw new ArgumentException(nameof(element));
        }
    }
}
