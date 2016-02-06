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
        private TextBlock child;
        public static readonly DependencyProperty TextStyleProperty = Watermark.TextStyleProperty.AddOwner(
            typeof(WatermarkAdorner),
            new FrameworkPropertyMetadata(default(Style)));

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

            this.child.Bind(TextBlock.StyleProperty)
                .OneWayTo(this, TextStyleProperty);

            this.child.Bind(TextBlock.TextProperty)
                .OneWayTo(textBox, Watermark.TextProperty);
        }

        public Style TextStyle
        {
            get { return (Style)this.GetValue(TextStyleProperty); }
            set { this.SetValue(TextStyleProperty, value); }
        }

        public TextBoxBase AdornedTextBox => (TextBoxBase)this.AdornedElement;

        protected override IEnumerator LogicalChildren => this.child == null
                                                              ? EmptyEnumerator.Instance
                                                              : new SingleChildEnumerator(this.child);

        protected FrameworkElement TextView
        {
            get
            {
                FrameworkElement textView;
                if (this.textViewRef.TryGetTarget(out textView))
                {
                    return textView;
                }
                textView = (FrameworkElement)this.AdornedElement.NestedChildren()
                    .OfType<ScrollContentPresenter>()
                    .SingleOrDefault()
                    ?.VisualChildren()
                    .OfType<IScrollInfo>()
                    .SingleOrDefault();
                if (textView != null)
                {
                    this.textViewRef.SetTarget(textView);
                }

                return textView;
            }
        }

        public void ClearChild()
        {
            this.RemoveVisualChild(this.child);
            this.RemoveLogicalChild(this.child);
            this.child = null;
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
            var desiredSize = this.AdornedElement.RenderSize;
            this.child.Measure(desiredSize);
            return desiredSize;
        }

        protected override Size ArrangeOverride(Size size)
        {
            var view = this.TextView;
            if (view != null)
            {
                var aSize = this.AdornedElement.RenderSize;
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
    }
}