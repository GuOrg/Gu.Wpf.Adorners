namespace Gu.Wpf.Adorners.Tests
{
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using NUnit.Framework;

    [Apartment(ApartmentState.STA)]
    public class WatermarkAdornerTests
    {
        private static readonly MethodInfo MeasureOverrideMethod = typeof(WatermarkAdorner).GetMethod("MeasureOverride", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, new[] { typeof(Size) }, null);
        private static readonly MethodInfo ArrangeOverrideMethod = typeof(WatermarkAdorner).GetMethod("ArrangeOverride", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, new[] { typeof(Size) }, null);

        [Test]
        public void MeasureOverrideWhenChildIsNull()
        {
            var textBox = new TextBox();
            var adorner = new WatermarkAdorner(textBox) { Child = null };
            var result = MeasureOverrideMethod.Invoke(adorner, new object[] { new Size(0, 0) });
            Assert.AreEqual(new Size(0, 0), result);
        }

        [Test]
        public void ArrangeOverrideWhenChildIsNull()
        {
            var textBox = new TextBox();
            var adorner = new WatermarkAdorner(textBox) { Child = null };
            var result = ArrangeOverrideMethod.Invoke(adorner, new object[] { new Size(0, 0) });
            Assert.AreEqual(new Size(0, 0), result);
        }
    }
}
