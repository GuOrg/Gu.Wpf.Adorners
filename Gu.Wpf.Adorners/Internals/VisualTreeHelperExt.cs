namespace Gu.Wpf.Adorners
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    internal static class VisualTreeHelperExt
    {
        internal static IEnumerable<DependencyObject> VisualAncestors(this DependencyObject o)
        {
            var parent = VisualTreeHelper.GetParent(o);
            while (parent != null)
            {
                yield return parent;
                parent = VisualTreeHelper.GetParent(parent);
            }
        }

        internal static IEnumerable<Visual> VisualChildren(this Visual parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                yield return (Visual)VisualTreeHelper.GetChild(parent, i);
            }
        }

        internal static T? FirstRecursiveVisualChild<T>(this DependencyObject parent)
            where T : Visual
        {
            return RecursiveVisualChildren(parent)
                   .OfType<T>()
                   .FirstOrDefault();
        }

        internal static IEnumerable<DependencyObject> RecursiveVisualChildren(this DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                yield return child;
                if (VisualTreeHelper.GetChildrenCount(child) != 0)
                {
                    foreach (var recursiveChild in RecursiveVisualChildren(child))
                    {
                        yield return recursiveChild;
                    }
                }
            }
        }

        internal static string DumpVisualTree(this DependencyObject parent)
        {
            using var stringWriter = new StringWriter();
            using var writer = new IndentedTextWriter(stringWriter);
            DumpVisualTree(parent, writer);
            return stringWriter.ToString();
        }

        private static void DumpVisualTree(this DependencyObject parent, IndentedTextWriter writer)
        {
            writer.WriteLine(parent.GetType().Name);
            if (VisualTreeHelper.GetChildrenCount(parent) != 0)
            {
                writer.Indent++;
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    DumpVisualTree(child, writer);
                }

                writer.Indent--;
            }
        }
    }
}
