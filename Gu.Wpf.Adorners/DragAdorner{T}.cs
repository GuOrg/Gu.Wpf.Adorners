namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    public abstract class DragAdorner<T> : ContainerAdorner<T>, IDisposable
        where T : FrameworkElement
    {
        private readonly Vector elementOffset;
        private UIElement dropTarget;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DragAdorner{T}"/> class.
        /// </summary>
        /// <param name="adornedElement">The drag source.</param>
        /// <param name="child">The content to render.</param>
        protected DragAdorner(UIElement adornedElement, T child)
            : base(adornedElement)
        {
            this.elementOffset = adornedElement.PointToScreen(new Point(0, 0)) - User32.GetMousePosition();
            var mp = User32.GetMousePosition(adornedElement) + this.elementOffset;

            this.Offset = new TranslateTransform(mp.X, mp.Y);
            child.RenderTransform = this.Offset;
            this.Child = child;
            DragDrop.AddPreviewQueryContinueDragHandler(adornedElement, this.UpdatePosition);
        }

        public TranslateTransform Offset { get; }

        public sealed override T Child
        {
            get => base.Child;
            set => base.Child = value;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public void SnapTo(UIElement dropTarget)
        {
            this.dropTarget = dropTarget;
            this.UpdatePosition();
            this.InvalidateMeasure();
            this.InvalidateVisual();
        }

        public void RemoveSnap()
        {
            this.dropTarget = null;
            this.UpdatePosition();
            this.InvalidateMeasure();
            this.InvalidateVisual();
        }

        public void RemoveSnap(UIElement dropTarget)
        {
            if (ReferenceEquals(this.dropTarget, dropTarget))
            {
                this.RemoveSnap();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            if (disposing)
            {
                DragDrop.RemovePreviewQueryContinueDragHandler(this.AdornedElement, this.UpdatePosition);
                AdornerService.Remove(this);
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (this.Child != null)
            {
                var size = this.dropTarget?.RenderSize ?? new Size(double.PositiveInfinity, double.PositiveInfinity);
                this.Child.Measure(size);
                return this.Child.DesiredSize;
            }

            return new Size(0.0, 0.0);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var bounds = this.dropTarget?.TransformToVisual(this.AdornedElement)
                                         .TransformBounds(new Rect(finalSize)) ??
                              new Rect(finalSize);
            this.Child?.Arrange(bounds);
            return finalSize;
        }

        protected void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        protected virtual void UpdatePosition()
        {
            if (this.dropTarget == null)
            {
                var mp = User32.GetMousePosition(this.AdornedElement) + this.elementOffset;
                this.Offset.SetCurrentValue(TranslateTransform.XProperty, mp.X);
                this.Offset.SetCurrentValue(TranslateTransform.YProperty, mp.Y);
            }
            else
            {
                this.Offset.SetCurrentValue(TranslateTransform.XProperty, 0.0);
                this.Offset.SetCurrentValue(TranslateTransform.YProperty, 0.0);
            }
        }

        private void UpdatePosition(object sender, QueryContinueDragEventArgs e)
        {
            this.UpdatePosition();
        }
    }
}