namespace Gu.Wpf.Adorners
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// Adorner that shows watermark text.
    /// </summary>
    [StyleTypedProperty(Property = nameof(TextStyle), StyleTargetType = typeof(TextBlock))]
    public sealed class WatermarkAdorner : ContainerAdorner<TextBlock>
    {
        /// <summary>Identifies the <see cref="TextStyle"/> dependency property.</summary>
        public static readonly DependencyProperty TextStyleProperty = Watermark.TextStyleProperty.AddOwner(
            typeof(WatermarkAdorner),
            new FrameworkPropertyMetadata(default(Style)));

        private const string TextBoxView = "TextBoxView";

        private readonly WeakReference<FrameworkElement?> referenceElement = new WeakReference<FrameworkElement?>(null);

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
        public Style? TextStyle
        {
            get => (Style)this.GetValue(TextStyleProperty);
            set => this.SetValue(TextStyleProperty, value);
        }

        private FrameworkElement? ReferenceElement
        {
            get
            {
                if (this.referenceElement.TryGetTarget(out var reference))
                {
                    return reference;
                }

                foreach (var child in this.AdornedElement.RecursiveVisualChildren())
                {
                    if (child is FrameworkElement { DependencyObjectType: { } objectType } candidate &&
                        objectType.Name == TextBoxView)
                    {
                        this.referenceElement.SetTarget(candidate);
                        return candidate;
                    }
                }

                if (this.AdornedElement is ComboBox comboBox)
                {
                    if (comboBox.FirstRecursiveVisualChild<ContentPresenter>() is { } contentPresenter)
                    {
                        this.referenceElement.SetTarget(contentPresenter);
                        return contentPresenter;
                    }

                    if (comboBox.FirstRecursiveVisualChild<ToggleButton>() is { } toggleButton)
                    {
                        if (toggleButton.VisualChildren().SingleOrDefault() is Border border &&
                            border.VisualChildren().SingleOrDefault() is FrameworkElement borderChild)
                        {
                            this.referenceElement.SetTarget(borderChild);
                            return borderChild;
                        }
                        else
                        {
                            this.referenceElement.SetTarget(toggleButton);
                            return toggleButton;
                        }
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

            if (this.Child is { } child)
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
            if (this.Child is { } child)
            {
                child.Arrange(finalRect);
            }

            return finalRect.Size;

            Rect GetFinalRect()
            {
                if (this.ReferenceElement is null)
                {
                    return Rect.Empty;
                }

                if (ReferenceEquals(this.AdornedElement, this.ReferenceElement))
                {
                    var slot = LayoutInformation.GetLayoutSlot(this.ReferenceElement);
                    return CenteredVertically(slot);
                }

                if (this.ReferenceElement.DependencyObjectType?.Name == TextBoxView)
                {
                    return this.ReferenceElement.TransformToAncestor(this.AdornedElement)
                               .TransformBounds(LayoutInformation.GetLayoutSlot(this.ReferenceElement));
                }

                var bounds = this.ReferenceElement.TransformToAncestor(this.AdornedElement)
                                                  .TransformBounds(LayoutInformation.GetLayoutSlot(this.ReferenceElement));

                if (this.ReferenceElement is ContentPresenter)
                {
                    return new Rect(bounds.TopLeft, bounds.BottomRight);
                }

                return CenteredVertically(bounds);

                Rect CenteredVertically(Rect rect)
                {
                    var y = rect.Height > size.Height ? (rect.Height - size.Height) / 2 : 0;
                    return new Rect(rect.TopLeft + new Vector(0, y), rect.BottomRight - new Vector(0, y));
                }
            }
        }
    }
}
