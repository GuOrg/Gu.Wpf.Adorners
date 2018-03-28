namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// Adorner that shows watermark text.
    /// </summary>
    public sealed class WatermarkAdorner : ContainerAdorner<TextBlock>
    {
        /// <summary>Identifies the <see cref="TextStyle"/> dependency property.</summary>
        public static readonly DependencyProperty TextStyleProperty = Watermark.TextStyleProperty.AddOwner(
            typeof(WatermarkAdorner),
            new FrameworkPropertyMetadata(default(Style)));

        private readonly WeakReference<FrameworkElement> textViewRef = new WeakReference<FrameworkElement>(null);

        static WatermarkAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkAdorner), new FrameworkPropertyMetadata(typeof(WatermarkAdorner)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The adorned element</param>
        public WatermarkAdorner(Control adornedElement)
            : base(adornedElement)
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
                .OneWayTo(adornedElement, Watermark.TextProperty);
        }

        /// <summary>
        /// Gets the adorned <see cref="TextBoxBase"/>
        /// </summary>
        [Obsolete("This property will be removed, use AdornedElement.")]
        public TextBoxBase AdornedTextBox => this.AdornedElement as TextBoxBase;

        /// <summary>
        /// Gets or sets the style for the <see cref="TextBlock"/> rendering <see cref="Watermark.TextProperty"/> for <see cref="System.Windows.Documents.Adorner.AdornedElement"/>
        /// </summary>
        public Style TextStyle
        {
            get => (Style)this.GetValue(TextStyleProperty);
            set => this.SetValue(TextStyleProperty, value);
        }

        private FrameworkElement TextView
        {
            get
            {
                if (this.textViewRef.TryGetTarget(out var textView))
                {
                    return textView;
                }

                // ReSharper disable once ConstantConditionalAccessQualifier
                textView = (FrameworkElement)this.AdornedElement
                    ?.NestedChildren()
                    .SingleOrNull<ScrollContentPresenter>()
                    ?.VisualChildren()
                    .SingleOrNull<IScrollInfo>(); // The TextView is internal but implements IScrollInfo
                if (textView != null)
                {
                    this.textViewRef.SetTarget(textView);
                }

                return textView;
            }
        }

        /// <inheritdoc />
        protected override Size MeasureOverride(Size constraint)
        {
            var desiredSize = this.AdornedElement.RenderSize;
            this.Child?.Measure(desiredSize);
            return desiredSize;
        }

        /// <inheritdoc />
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
                this.Child?.Arrange(new Rect(location, wSize));
            }
            else
            {
                this.Child?.Arrange(new Rect(new Point(0, 0), size));
            }

            return size;
        }
    }
}
