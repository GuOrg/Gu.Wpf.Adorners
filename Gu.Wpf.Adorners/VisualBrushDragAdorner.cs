namespace Gu.Wpf.Adorners
{
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class VisualBrushDragAdorner : DragAdorner<Rectangle>
    {
        static VisualBrushDragAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VisualBrushDragAdorner), new FrameworkPropertyMetadata(typeof(VisualBrushDragAdorner)));
        }

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