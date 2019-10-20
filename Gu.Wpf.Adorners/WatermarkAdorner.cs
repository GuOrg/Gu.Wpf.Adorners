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
    public sealed class WatermarkAdorner : ContainerAdorner<TextBlock>
    {
        /// <summary>Identifies the <see cref="TextStyle"/> dependency property.</summary>
        public static readonly DependencyProperty TextStyleProperty = Watermark.TextStyleProperty.AddOwner(
            typeof(WatermarkAdorner),
            new FrameworkPropertyMetadata(default(Style)));

        private readonly WeakReference<FrameworkElement> placementReference = new WeakReference<FrameworkElement>(null);

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
        /// Gets the adorned <see cref="TextBoxBase"/>.
        /// </summary>
        [Obsolete("This property will be removed, use AdornedElement.")]
        public TextBoxBase AdornedTextBox => this.AdornedElement as TextBoxBase;

        /// <summary>
        /// Gets or sets the style for the <see cref="TextBlock"/> rendering <see cref="Watermark.TextProperty"/> for <see cref="System.Windows.Documents.Adorner.AdornedElement"/>.
        /// </summary>
        public Style TextStyle
        {
            get => (Style)this.GetValue(TextStyleProperty);
            set => this.SetValue(TextStyleProperty, value);
        }

        private FrameworkElement PlacementReference
        {
            get
            {
                if (this.AdornedElement is null)
                {
                    this.placementReference.SetTarget(null);
                    return null;
                }

                if (this.placementReference.TryGetTarget(out var contentPresenter))
                {
                    return contentPresenter;
                }

                if (this.AdornedElement is TextBoxBase ||
                    this.AdornedElement is PasswordBox)
                {
                    // ReSharper disable once ConstantConditionalAccessQualifier
                    contentPresenter = (FrameworkElement)this.AdornedElement
                                                     ?.FirstOrDefaultRecursiveVisualChild<ScrollContentPresenter>()
                                                     ?.FirstOrDefaultRecursiveVisualChild<IScrollInfo>(); // The TextView is internal but implements IScrollInfo
                    if (contentPresenter != null)
                    {
                        this.placementReference.SetTarget(contentPresenter);
                    }
                }
                else if (this.AdornedElement is ComboBox comboBox)
                {
                    contentPresenter = (FrameworkElement)comboBox.FirstOrDefaultRecursiveVisualChild<ContentPresenter>() ??
                                                         comboBox.FirstOrDefaultRecursiveVisualChild<ToggleButton>();
                    if (contentPresenter != null)
                    {
                        this.placementReference.SetTarget(contentPresenter);
                    }
                }

                return contentPresenter;
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
            var finalRect = GetFinalRect();
            if (finalRect.Width < 0 ||
                finalRect.Height < 0)
            {
                finalRect = new Rect(size);
            }

            this.Child?.Arrange(finalRect);
            return size;

            Rect GetFinalRect()
            {
                var reference = this.PlacementReference;
                switch (reference)
                {
                    case IScrollInfo _:
                        var aSize = this.AdornedElement.RenderSize;
                        var wSize = reference.RenderSize;
                        var x = (aSize.Width - wSize.Width) / 2;
                        var y = (aSize.Height - wSize.Height) / 2;
                        var location = new Point(x, y);
                        return new Rect(location, wSize);
                    case ContentPresenter _:
                        var margin = reference.Margin;
                        var rect = new Rect(this.AdornedElement.RenderSize);
                        rect.Inflate(-margin.Left, -margin.Top);
                        return rect;
                    default:
                        return new Rect(new Point(0, 0), size);
                }
            }
        }
    }
}
