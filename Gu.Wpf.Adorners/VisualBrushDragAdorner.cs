namespace Gu.Wpf.Adorners
{
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// A drag adorner that renders a rectangle with the visual brush of the adorned element.
    /// </summary>
    public class VisualBrushDragAdorner : DragAdorner<Rectangle>
    {
        static VisualBrushDragAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VisualBrushDragAdorner), new FrameworkPropertyMetadata(typeof(VisualBrushDragAdorner)));
        }

        /// <inheritdoc />
        public VisualBrushDragAdorner(UIElement adornedElement)
            : base(adornedElement, new Rectangle())
        {
            this.Child.Bind(WidthProperty)
                .OneWayTo(adornedElement, ActualWidthProperty);
            this.Child.Bind(HeightProperty)
                .OneWayTo(adornedElement, ActualHeightProperty);
            this.Child.Fill = new VisualBrush(adornedElement);
        }
    }
}