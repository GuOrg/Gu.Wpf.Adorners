namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;

    internal static class Loaded
    {
        internal static bool IsLoaded(this DependencyObject element)
        {
            switch (element)
            {
                case FrameworkElement fe:
                    return fe.IsLoaded;
                case FrameworkContentElement fce:
                    return fce.IsLoaded;
                default:
                    throw new ArgumentException($"Did not find an IsLoaded property on the element: {element}.", nameof(element));
            }
        }
    }
}
