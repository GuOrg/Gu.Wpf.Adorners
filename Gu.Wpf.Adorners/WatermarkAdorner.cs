namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Media;

    public class WatermarkAdorner : Adorner
    {
        private readonly TextBlock child;
        public static readonly DependencyProperty TextStyleProperty = DependencyProperty.Register(
            "TextStyle", 
            typeof (Style),
            typeof (WatermarkAdorner),
            new PropertyMetadata(default(Style)), OnValidateTextStyle);

        static WatermarkAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkAdorner), new FrameworkPropertyMetadata(typeof(WatermarkAdorner)));
        }

        public WatermarkAdorner(TextBox textBox)
            : base(textBox)
        {
            var textBlock = new TextBlock
            {
                Focusable = false,
            };

            textBlock.Bind(TextBlock.TextProperty)
                     .OneWayTo(textBox, Watermark.TextProperty);

            this.child = textBlock;
        }

        public Style TextStyle
        {
            get { return (Style)this.GetValue(TextStyleProperty); }
            set { this.SetValue(TextStyleProperty, value); }
        }

        public new TextBoxBase AdornedElement => (TextBoxBase) base.AdornedElement;

        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.child;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            this.child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            return this.child.DesiredSize;
        }

        protected override Size ArrangeOverride(Size size)
        {
            var finalSize = base.ArrangeOverride(size);
            this.child.Arrange(new Rect(new Point(0, 0), finalSize));
            return finalSize;
        }

        private static bool OnValidateTextStyle(object value)
        {
            var style = (Style)value;
            return style == null || style.TargetType == typeof(TextBlock);
        }
    }
}