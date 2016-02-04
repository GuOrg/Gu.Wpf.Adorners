namespace Gu.Wpf.Adorners
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Media;

    public class WatermarkAdorner : Adorner
    {
        private readonly TextBlock child;
        public static readonly DependencyProperty TextStyleProperty = Watermark.TextStyleProperty.AddOwner(
            typeof(WatermarkAdorner),
            new FrameworkPropertyMetadata(
                default(Style),
                FrameworkPropertyMetadataOptions.Inherits,
                OnTextStyleChanged));

        private readonly WeakReference<FrameworkElement> textViewRef = new WeakReference<FrameworkElement>(null);

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

            this.child = textBlock;
            this.AddVisualChild(this.child);
            this.AddLogicalChild(this.child);
        }

        public string Text
        {
            get { return this.child.Text; }
            set
            {
                if (child.Text == value)
                {
                    return;
                }

                child.Text = value;
            }
        }

        public Style TextStyle
        {
            get { return (Style)this.GetValue(TextStyleProperty); }
            set { this.SetValue(TextStyleProperty, value); }
        }

        public new TextBoxBase AdornedElement => (TextBoxBase)base.AdornedElement;

        protected override IEnumerator LogicalChildren => new SingleChildEnumerator(this.child);

        protected FrameworkElement TextView
        {
            get
            {
                FrameworkElement textView;
                if (this.textViewRef.TryGetTarget(out textView))
                {
                    return textView;
                }
                textView = (FrameworkElement)AdornedElement.NestedChildren()
                    .OfType<ScrollContentPresenter>()
                    .SingleOrDefault()
                    ?.VisualChildren()
                    .OfType<IScrollInfo>()
                    .SingleOrDefault();
                if (textView != null)
                {
                    textViewRef.SetTarget(textView);
                }

                return textView;
            }
        }

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
            var desiredSize = AdornedElement.RenderSize;
            child.Measure(desiredSize);
            return desiredSize;
        }

        protected override Size ArrangeOverride(Size size)
        {
            var view = TextView;
            if (view != null)
            {
                var aSize = AdornedElement.RenderSize;
                var wSize = view.RenderSize;
                var x = (aSize.Width - wSize.Width) / 2;
                var y = (aSize.Height - wSize.Height) / 2;
                var location = new Point(x, y);
                this.child.Arrange(new Rect(location, wSize));
            }
            else
            {
                this.child.Arrange(new Rect(new Point(0, 0), size));
            }
            return size;
        }

        private static void OnTextStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adorner = (WatermarkAdorner)d;
            adorner.child.SetCurrentValue(FrameworkElement.StyleProperty, e.NewValue);
            adorner.InvalidateMeasure();
            adorner.InvalidateVisual();
        }
    }
}