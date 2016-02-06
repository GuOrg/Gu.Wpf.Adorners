namespace Gu.Wpf.Adorners
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// This class is sealed because it calls OnVisualChildrenChanged virtual in the
    /// constructor and it does not override it, but derived classes could.
    /// ~Inspired~ by: http://referencesource.microsoft.com/#PresentationFramework/src/Framework/MS/Internal/Controls/TemplatedAdorner.cs,c0a050a6ac0c693d
    /// </summary>
    public class TemplatedAdorner : ContainerAdorner<Control>
    {
        public TemplatedAdorner(UIElement adornedElement, ControlTemplate adornerTemplate) : base(adornedElement)
        {
            Debug.Assert(adornedElement != null, "adornedElement should not be null");
            Debug.Assert(adornerTemplate != null, "adornerTemplate should not be null");

            this.Child = new Control
            {
                IsTabStop = false,
                Template = adornerTemplate
            };

            this.Bind(DataContextProperty)
                .OneWayTo(this.AdornedElement, DataContextProperty);
        }

        public FrameworkElement ReferenceElement { get; set; }

        /// <summary>
        /// Adorners don't always want to be transformed in the same way as the elements they
        /// adorn.  Adorners which adorn points, such as resize handles, want to be translated
        /// and rotated but not scaled.  Adorners adorning an object, like a marquee, may want
        /// all transforms.  This method is called by AdornerLayer to allow the adorner to
        /// filter out the transforms it doesn't want and return a new transform with just the
        /// transforms it wants applied.  An adorner can also add an additional translation
        /// transform at this time, allowing it to be positioned somewhere other than the upper
        /// left corner of its adorned element.
        /// </summary>
        /// <param name="transform">The transform applied to the object the adorner adorns</param>
        /// <returns>Transform to apply to the adorner</returns>
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            if (this.ReferenceElement == null)
                return transform;

            GeneralTransformGroup group = new GeneralTransformGroup();
            group.Children.Add(transform);

            GeneralTransform t = this.TransformToDescendant(this.ReferenceElement);
            if (t != null)
            {
                group.Children.Add(t);
            }
            return group;
        }

        /// <summary>
        /// Measure adorner.
        /// </summary>
        protected override Size MeasureOverride(Size constraint)
        {
            Debug.Assert(this.Child != null, "_child should not be null");

            if (this.ReferenceElement != null && this.AdornedElement != null &&
                this.AdornedElement.IsMeasureValid &&
                !DoubleUtil.AreClose(this.ReferenceElement.DesiredSize, this.AdornedElement.DesiredSize)
                )
            {
                this.ReferenceElement.InvalidateMeasure();
            }

            this.Child.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            return this.Child.DesiredSize;
        }

        /// <summary>
        ///     Default control arrangement is to only arrange
        ///     the first visual child. No transforms will be applied.
        /// </summary>
        protected override Size ArrangeOverride(Size size)
        {
            var finalSize = base.ArrangeOverride(size);
            this.Child?.Arrange(new Rect(new Point(), finalSize));
            return finalSize;
        }

        protected override void OnInitialized(EventArgs e)
        {
            var placeholder = this.NestedChildren()
                                  .OfType<System.Windows.Controls.AdornedElementPlaceholder>()
                                  .FirstOrDefault();
            if (placeholder != null)
            {
                var message = $"{placeholder.GetType().FullName} cannot be used because of poor design in WPF.\r\n" +
                              $"Use adorners:{typeof(AdornedElementPlaceholder).Name} instead.";
                throw new InvalidOperationException(message);
            }

            base.OnInitialized(e);
        }
    }
}