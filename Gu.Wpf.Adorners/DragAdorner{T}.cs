namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// A base class for drag adorners.
    /// </summary>
    /// <typeparam name="T">The type of the content.</typeparam>
    public abstract class DragAdorner<T> : ContainerAdorner<T>, IDisposable
        where T : FrameworkElement
    {
        /// <summary>Identifies the <see cref="DropTarget"/> dependency property.</summary>
        public static readonly DependencyProperty DropTargetProperty = DependencyProperty.Register(
            nameof(DropTarget),
            typeof(UIElement),
            typeof(DragAdorner<T>),
            new PropertyMetadata(
                default(UIElement),
                OnDropTargetChanged));

        private readonly Vector elementOffset;

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
            base.Child = child;
            DragDrop.AddPreviewQueryContinueDragHandler(adornedElement, this.UpdatePosition);
        }

        /// <summary>
        /// Gets a transform that specifies the offset for the position.
        /// By default the adorner is positioned so that the mouse is glued to the position where the drag started.
        /// </summary>
        public TranslateTransform Offset { get; }

        /// <summary>
        /// Gets or sets the drop target to snap position to.
        /// </summary>
        public UIElement DropTarget
        {
            get => (UIElement)this.GetValue(DropTargetProperty);
            set => this.SetValue(DropTargetProperty, value);
        }

        /// <inheritdoc />
        public sealed override T Child
        {
            get => base.Child;
            set => base.Child = value;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Set <see cref="DropTarget"/> to <paramref name="dropTarget"/> and update the position.
        /// Provides a visual hint that the item can be dropped.
        /// </summary>
        /// <param name="dropTarget">The drop target to snap position to.</param>
        public void SnapTo(UIElement dropTarget)
        {
            this.SetCurrentValue(DropTargetProperty, dropTarget);
        }

        /// <summary>
        /// Set <see cref="DropTarget"/> to null and start following mouse cursor again.
        /// </summary>
        public void RemoveSnap()
        {
            this.SetCurrentValue(DropTargetProperty, null);
        }

        /// <summary>
        /// Set <see cref="DropTarget"/> to null if <see cref="DropTarget"/> is the same instance as <paramref name="dropTarget"/> and start following mouse cursor again.
        /// </summary>
        /// <param name="dropTarget">The drop target to stop snapping position to.</param>
        public void RemoveSnap(UIElement dropTarget)
        {
            if (ReferenceEquals(this.DropTarget, dropTarget))
            {
                this.RemoveSnap();
            }
        }

        /// <summary>
        /// Called when <see cref="DropTarget"/> changes value.
        /// </summary>
        /// <param name="oldValue">The old drop target.</param>
        /// <param name="newValue">The new drop target.</param>
        protected virtual void OnDropTargetChanged(UIElement oldValue, UIElement newValue)
        {
            this.UpdatePosition();
            this.InvalidateMeasure();
            this.InvalidateVisual();
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

        /// <inheritdoc />
        protected override Size MeasureOverride(Size constraint)
        {
            if (this.Child != null)
            {
                var size = this.DropTarget?.RenderSize ?? new Size(double.PositiveInfinity, double.PositiveInfinity);
                this.Child.Measure(size);
                return this.Child.DesiredSize;
            }

            return new Size(0.0, 0.0);
        }

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size finalSize)
        {
            var bounds = this.DropTarget?.TransformToVisual(this.AdornedElement)
                                         .TransformBounds(new Rect(finalSize)) ??
                              new Rect(finalSize);
            this.Child?.Arrange(bounds);
            return finalSize;
        }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> is this instance is disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        /// <summary>
        /// Update the position of the adorner. This is called when QueryContinueDrag is raised or DropTarget changes.
        /// </summary>
        protected virtual void UpdatePosition()
        {
            this.ThrowIfDisposed();
            if (this.DropTarget == null)
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

        private static void OnDropTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var adorner = (DragAdorner<T>)d;
            adorner.OnDropTargetChanged((UIElement)e.OldValue, (UIElement)e.NewValue);
        }

        private void UpdatePosition(object sender, QueryContinueDragEventArgs e)
        {
            this.UpdatePosition();
        }
    }
}