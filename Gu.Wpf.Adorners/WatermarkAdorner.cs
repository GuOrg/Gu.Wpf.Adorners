namespace Gu.Wpf.Adorners
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public class WatermarkAdorner : ContainerAdorner<TextBlock>
    {
        public static readonly DependencyProperty TextStyleProperty = Watermark.TextStyleProperty.AddOwner(
            typeof(WatermarkAdorner),
            new FrameworkPropertyMetadata(
                default(Style)));

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
            this.Child = textBlock;

            // For some reason setting the style directly in PropertyChangedCallback did not work.
            // Binding it instead.
            this.Child.Bind(StyleProperty)
                .OneWayTo(this, TextStyleProperty);

            this.Child.Bind(TextBlock.TextProperty)
                .OneWayTo(textBox, Watermark.TextProperty);
        }

        public Style TextStyle
        {
            get { return (Style)this.GetValue(TextStyleProperty); }
            set { this.SetValue(TextStyleProperty, value); }
        }

        public TextBoxBase AdornedTextBox => (TextBoxBase)this.AdornedElement;

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

        protected override Size MeasureOverride(Size constraint)
        {
            var desiredSize = this.AdornedElement.RenderSize;
            this.Child.Measure(desiredSize);
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
                this.Child.Arrange(new Rect(location, wSize));
            }
            else
            {
                this.Child.Arrange(new Rect(new Point(0, 0), size));
            }
            return size;
        }
    }
}