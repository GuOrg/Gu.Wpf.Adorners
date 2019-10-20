namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;

    /// <summary>
    /// Adorner that shows watermark text.
    /// </summary>
    [StyleTypedProperty(Property = nameof(TextStyle), StyleTargetType = typeof(TextBlock))]
    public sealed class WatermarkAdorner : ContainerAdorner<TextBlock>
    {
        private const string TextBoxView = "TextBoxView";

        /// <summary>Identifies the <see cref="TextStyle"/> dependency property.</summary>
        public static readonly DependencyProperty TextStyleProperty = Watermark.TextStyleProperty.AddOwner(
            typeof(WatermarkAdorner),
            new FrameworkPropertyMetadata(default(Style)));

        private readonly WeakReference<FrameworkElement> referenceElement = new WeakReference<FrameworkElement>(null);

        static WatermarkAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkAdorner), new FrameworkPropertyMetadata(typeof(WatermarkAdorner)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The adorned element.</param>
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
            _ = this.Child.Bind(StyleProperty)
                    .OneWayTo(this, TextStyleProperty);

            _ = this.Child.Bind(TextBlock.TextProperty)
                    .OneWayTo(adornedElement, Watermark.TextProperty);
        }

        /// <summary>
        /// Gets or sets the style for the <see cref="TextBlock"/> rendering <see cref="Watermark.TextProperty"/> for <see cref="System.Windows.Documents.Adorner.AdornedElement"/>.
        /// </summary>
        public Style TextStyle
        {
            get => (Style)this.GetValue(TextStyleProperty);
            set => this.SetValue(TextStyleProperty, value);
        }

        private FrameworkElement ReferenceElement
        {
            get
            {
                if (this.referenceElement.TryGetTarget(out var reference))
                {
                    return reference;
                }

                foreach (var child in this.AdornedElement.RecursiveVisualChildren())
                {
                    if (child is FrameworkElement candidate &&
                        candidate.DependencyObjectType is DependencyObjectType objectType &&
                        objectType.Name == TextBoxView)
                    {
                        this.referenceElement.SetTarget(candidate);
                        return candidate;
                    }
                }

                if (this.AdornedElement is ComboBox comboBox)
                {
                    reference = (FrameworkElement)comboBox.FirstOrDefaultRecursiveVisualChild<ContentPresenter>() ??
                                                  comboBox.FirstOrDefaultRecursiveVisualChild<ToggleButton>();
                    if (reference != null)
                    {
                        this.referenceElement.SetTarget(reference);
                        return reference;
                    }
                }

                return this.AdornedElement as FrameworkElement;
            }
        }

        /// <inheritdoc />
        protected override Size MeasureOverride(Size constraint)
        {
            if (this.ReferenceElement != null &&
                this.AdornedElement.IsMeasureValid &&
                !DoubleUtil.AreClose(this.ReferenceElement.DesiredSize, this.AdornedElement.DesiredSize))
            {
                this.ReferenceElement.InvalidateMeasure();
            }

            if (this.Child is TextBlock child)
            {
                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                return child.DesiredSize;
            }

            return Size.Empty;
        }

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size size)
        {
            var finalRect = GetFinalRect();
            if (this.Child is TextBlock child)
            {
                child.Arrange(finalRect);
            }

            return finalRect.Size;

            Rect GetFinalRect()
            {
                if (ReferenceEquals(this.AdornedElement, this.ReferenceElement))
                {
                    // Add default text margin.
                    return new Rect(new Point(2, 0), new Size(this.AdornedElement.RenderSize.Width - 4, this.AdornedElement.RenderSize.Height));
                }

                if (this.ReferenceElement.DependencyObjectType?.Name == TextBoxView)
                {
                    return this.ReferenceElement.TransformToAncestor(this.AdornedElement)
                               .TransformBounds(LayoutInformation.GetLayoutSlot(this.ReferenceElement));
                }

                return this.ReferenceElement.TransformToAncestor(this.AdornedElement)
                                            .TransformBounds(LayoutInformation.GetLayoutSlot(this.ReferenceElement));

                // Add default text margin.
                //return new Rect(bounds.TopLeft + new Vector(2, 0), bounds.BottomRight - new Vector(4, 0));
            }
        }
    }
}
