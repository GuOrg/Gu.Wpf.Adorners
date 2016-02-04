using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gu.Wpf.Adorners.Tests
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Markup;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var names = typeof (TextElement).GetFields()
                .Where(f => f.FieldType == typeof (DependencyProperty))
                .Select(f => ((DependencyProperty) f.GetValue(null)).Name)
                .ToArray();
            foreach (var name in names)
            {
                Console.WriteLine($"<Setter Property=\"TextElement.{name}\" Value=\"{{Binding AdornedElement.{name}, RelativeSource ={{RelativeSource Self}}}}\" />");
            }


        }
    }
}
