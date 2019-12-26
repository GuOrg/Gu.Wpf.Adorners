namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;

    internal static class Loaded
    {
        internal static bool IsLoaded(this DependencyObject element)
        {
            return element switch
            {
                FrameworkElement fe => fe.IsLoaded,
                FrameworkContentElement fce => fce.IsLoaded,
                _ => throw new ArgumentException($"Did not find an IsLoaded property on the element: {element}.", nameof(element))
            };
        }
    }
}
