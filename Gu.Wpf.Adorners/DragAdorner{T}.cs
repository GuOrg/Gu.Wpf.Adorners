namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    public class DragAdorner<T> : ContainerAdorner<T>, IDisposable
        where T : FrameworkElement
    {
        private readonly Vector elementOffset;
        private bool disposed;

        public DragAdorner(UIElement adornedElement, T child)
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
                this.Child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                return this.Child.DesiredSize;
            }

            return new Size(0.0, 0.0);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.Child?.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        private void UpdatePosition(object sender, QueryContinueDragEventArgs e)
        {
            var mp = User32.GetMousePosition(this.AdornedElement) + this.elementOffset;
            this.Offset.SetCurrentValue(TranslateTransform.XProperty, mp.X);
            this.Offset.SetCurrentValue(TranslateTransform.YProperty, mp.Y);
        }
    }
}