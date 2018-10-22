namespace Gu.Wpf.Adorners.Tests
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Documents;
    using NUnit.Framework;

    [Explicit("Code gen.")]
    public class CodeGen
    {
        [Test]
        public void DumpTextElementProps()
        {
            var dependencyProperties = typeof(TextElement).GetFields()
                .Where(f => f.FieldType == typeof(DependencyProperty))
                .Select(f => (DependencyProperty)f.GetValue(null))
                .ToArray();
            foreach (var property in dependencyProperties)
            {
                Console.WriteLine($"<Setter Property=\"{property.OwnerType.Name}.{property.Name}\" Value=\"{{Binding AdornedElement.({property.OwnerType.Name}.{property.Name}), RelativeSource ={{RelativeSource Self}}}}\" />");
            }
        }
    }
}
