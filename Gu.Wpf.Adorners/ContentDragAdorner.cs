namespace Gu.Wpf.Adorners
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class ContentDragAdorner : ContainerAdorner<ContentPresenter>
    {
        public TranslateTransform Offset { get; }

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

        static ContentDragAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentDragAdorner), new FrameworkPropertyMetadata(typeof(ContentDragAdorner)));
        }

        public ContentDragAdorner(UIElement adornedElement, TranslateTransform offset)
            : base(adornedElement)
        {
            this.Offset = offset;
            this.Child = new ContentPresenter();
            this.Child.Bind(ContentPresenter.ContentProperty)
                .OneWayTo(this, ContentProperty);
            this.Child.Bind(ContentPresenter.ContentTemplateProperty)
                .OneWayTo(this, ContentTemplateProperty);
            this.Child.Bind(ContentPresenter.ContentTemplateSelectorProperty)
                .OneWayTo(this, ContentTemplateSelectorProperty);
            this.Child.Bind(StyleProperty)
                .OneWayTo(this, StyleProperty);
        }

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

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            var transformGroup = new GeneralTransformGroup();
            transformGroup.Children.Add(transform);
            transformGroup.Children.Add(this.Offset);
            return transformGroup;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Debug.WriteLine($"MeasureOverride({constraint})");
            if (this.Child != null)
            {
                this.Child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                return this.Child.DesiredSize;
            }

            return new Size(0.0, 0.0);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Debug.WriteLine($"ArrangeOverride({finalSize})");
            this.Child?.Arrange(new Rect(finalSize));
            return finalSize;
        }
    }
}