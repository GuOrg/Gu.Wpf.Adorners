namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class ContentDragAdorner : ContainerAdorner<ContentPresenter>, IDisposable
    {
        public static readonly DependencyProperty ContentProperty = ContentControl.ContentProperty.AddOwner(
            typeof(ContentDragAdorner),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ContentTemplateProperty = ContentControl.ContentTemplateProperty.AddOwner(
            typeof(ContentDragAdorner),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ContentTemplateSelectorProperty = ContentControl.ContentTemplateSelectorProperty.AddOwner(
                typeof(ContentDragAdorner),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ContentPresenterStyleProperty = DependencyProperty.Register(
            "ContentPresenterStyle",
            typeof(Style),
            typeof(ContentDragAdorner),
            new PropertyMetadata(default(Style)));

        private readonly Vector offset;
        private bool disposed;

        static ContentDragAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentDragAdorner), new FrameworkPropertyMetadata(typeof(ContentDragAdorner)));
        }

        public ContentDragAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            this.offset = adornedElement.PointToScreen(new Point(0, 0)) - User32.GetMousePosition();
            var mp = User32.GetMousePosition(adornedElement) + this.offset;

            this.Offset = new TranslateTransform(mp.X, mp.Y);
            this.Child = new ContentPresenter { RenderTransform = this.Offset };
            this.Child.Bind(ContentPresenter.ContentProperty)
                .OneWayTo(this, ContentProperty);
            this.Child.Bind(ContentPresenter.ContentTemplateProperty)
                .OneWayTo(this, ContentTemplateProperty);
            this.Child.Bind(ContentPresenter.ContentTemplateSelectorProperty)
                .OneWayTo(this, ContentTemplateSelectorProperty);
            this.Child.Bind(StyleProperty)
                .OneWayTo(this, StyleProperty);

            DragDrop.AddPreviewQueryContinueDragHandler(adornedElement, this.UpdatePosition);
        }

        public TranslateTransform Offset { get; }

        public sealed override ContentPresenter Child
        {
            get => base.Child;
            set => base.Child = value;
        }

        public object Content
        {
            get => this.GetValue(ContentProperty);
            set => this.SetValue(ContentProperty, value);
        }

        public DataTemplate ContentTemplate
        {
            get => (DataTemplate)this.GetValue(ContentTemplateProperty);
            set => this.SetValue(ContentTemplateProperty, value);
        }

        public DataTemplateSelector ContentTemplateSelector
        {
            get => (DataTemplateSelector)this.GetValue(ContentTemplateSelectorProperty);
            set => this.SetValue(ContentTemplateSelectorProperty, value);
        }

        public Style ContentPresenterStyle
        {
            get => (Style)this.GetValue(ContentPresenterStyleProperty);
            set => this.SetValue(ContentPresenterStyleProperty, value);
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
            var mp = User32.GetMousePosition(this.AdornedElement) + this.offset;
            this.Offset.SetCurrentValue(TranslateTransform.XProperty, mp.X);
            this.Offset.SetCurrentValue(TranslateTransform.YProperty, mp.Y);
        }
    }
}