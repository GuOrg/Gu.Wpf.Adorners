//namespace Gu.Wpf.Adorners
//{
//    using System;
//    using System.Diagnostics;
//    using System.Linq;
//    using System.Windows;
//    using System.Windows.Controls;
//    using System.Windows.Documents;
//    using System.Windows.Media;

//    /// <summary>
//    /// ~Inspired by: http://referencesource.microsoft.com/#PresentationFramework/src/Framework/MS/Internal/Controls/TemplatedAdorner.cs,c0a050a6ac0c693d
//    /// </summary>
//    public sealed class TemplatedAdorner : Adorner
//    {
//        private Control child;

//        public TemplatedAdorner(UIElement adornedElement, ControlTemplate adornerTemplate)
//            : base(adornedElement)
//        {
//            Debug.Assert(adornedElement != null, "adornedElement should not be null");
//            Debug.Assert(adornerTemplate != null, "adornerTemplate should not be null");
//            var control = new Control
//            {
//                DataContext = Validation.GetErrors(adornedElement),
//                IsTabStop = false,
//                Focusable = false,
//                Template = adornerTemplate
//            };

//            this.child = control;
//            this.AddVisualChild(this.child);
//        }

//        public FrameworkElement ReferenceElement { get; set; }

//        protected override int VisualChildrenCount => this.child != null ? 1 : 0;

//        /// <summary>
//        /// Adorners don't always want to be transformed in the same way as the elements they
//        /// adorn.  Adorners which adorn points, such as resize handles, want to be translated
//        /// and rotated but not scaled.  Adorners adorning an object, like a marquee, may want
//        /// all transforms.  This method is called by AdornerLayer to allow the adorner to
//        /// filter out the transforms it doesn't want and return a new transform with just the
//        /// transforms it wants applied.  An adorner can also add an additional translation
//        /// transform at this time, allowing it to be positioned somewhere other than the upper
//        /// left corner of its adorned element.
//        /// </summary>
//        /// <param name="transform">The transform applied to the object the adorner adorns</param>
//        /// <returns>Transform to apply to the adorner</returns>
//        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
//        {
//            if (this.ReferenceElement == null)
//            {
//                return transform;
//            }

//            GeneralTransformGroup group = new GeneralTransformGroup();
//            group.Children.Add(transform);

//            GeneralTransform t = this.TransformToDescendant(this.ReferenceElement);
//            if (t != null)
//            {
//                group.Children.Add(t);
//            }

//            return group;
//        }

//        public void ClearChild()
//        {
//            this.RemoveVisualChild(this.child);
//            this.child = null;
//        }

//        /// <summary>
//        ///   Derived class must implement to support Visual children. The method must return
//        ///    the child at the specified index. Index must be between 0 and GetVisualChildrenCount-1.
//        ///
//        ///    By default a Visual does not have any children.
//        ///
//        ///  Remark:
//        ///       During this virtual call it is not valid to modify the Visual tree.
//        /// </summary>
//        protected override Visual GetVisualChild(int index)
//        {
//            if (this.child == null || index != 0)
//            {
//                throw new ArgumentOutOfRangeException(nameof(index));
//            }

//            return this.child;
//        }

//        protected override Size MeasureOverride(Size constraint)
//        {
//            Debug.Assert(this.child != null, "child should not be null");

//            if (this.ReferenceElement != null &&
//                this.AdornedElement != null &&
//                this.AdornedElement.IsMeasureValid &&
//                !DoubleUtil.AreClose(this.ReferenceElement.DesiredSize, this.AdornedElement.DesiredSize))
//            {
//                this.ReferenceElement.InvalidateMeasure();
//            }
//            var placeholder = this.child.NestedChildren()
//                                  .OfType<AdornedElementPlaceholder>()
//                                  .SingleOrDefault();
//            this.child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

//            return this.child.DesiredSize;
//        }

//        /// <summary>
//        ///     Default control arrangement is to only arrange
//        ///     the first visual child. No transforms will be applied.
//        /// </summary>
//        protected override Size ArrangeOverride(Size size)
//        {
//            var finalSize = base.ArrangeOverride(size);
//            this.child?.Arrange(new Rect(new Point(0, 0), finalSize));
//            return finalSize;
//        }

//        ////internal override bool NeedsUpdate(Size oldSize)
//        ////{
//        ////    bool result = base.NeedsUpdate(oldSize);
//        ////    Visibility desired = this.AdornedElement.IsVisible ? Visibility.Visible : Visibility.Collapsed;
//        ////    if (desired != this.Visibility)
//        ////    {
//        ////        this.Visibility = desired;
//        ////        result = true;
//        ////    }
//        ////    return result;
//        ////}
//    }
//}