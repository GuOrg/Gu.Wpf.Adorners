namespace Gu.Wpf.Adorners
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Markup.Primitives;

    /// <summary>
    /// https://social.msdn.microsoft.com/Forums/vstudio/en-US/580234cb-e870-4af1-9a91-3e3ba118c89c/getting-list-of-all-dependencyattached-properties-of-an-object?forum=wpf
    /// </summary>
    internal static class DependencyObjectHelper
    {
        internal static IReadOnlyList<DependencyProperty> GetDependencyProperties(object element)
        {
            var properties = new List<DependencyProperty>();
            var markupObject = MarkupWriter.GetMarkupObjectFor(element);
            if (markupObject != null)
            {
                foreach (MarkupProperty mp in markupObject.Properties)
                {
                    if (mp.DependencyProperty != null)
                    {
                        properties.Add(mp.DependencyProperty);
                    }
                }
            }

            return properties;
        }

        internal static IReadOnlyList<DependencyProperty> GetAttachedProperties(object element)
        {
            var attachedProperties = new List<DependencyProperty>();
            var markupObject = MarkupWriter.GetMarkupObjectFor(element);
            if (markupObject != null)
            {
                foreach (MarkupProperty mp in markupObject.Properties)
                {
                    if (mp.IsAttached)
                    {
                        attachedProperties.Add(mp.DependencyProperty);
                    }
                }
            }

            return attachedProperties;
        }
    }
}