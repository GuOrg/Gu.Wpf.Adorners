namespace Gu.Wpf.Adorners
{
    using System.Windows;
    using System.Windows.Controls;

    public class ContentAdorner : ContainerAdorner<ContentPresenter>
    {
        /// <summary>Identifies the <see cref="Content"/> dependency property.</summary>
        public static readonly DependencyProperty ContentProperty = ContentControl.ContentProperty.AddOwner(
            typeof(ContentAdorner),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>Identifies the <see cref="ContentTemplate"/> dependency property.</summary>
        public static readonly DependencyProperty ContentTemplateProperty = ContentControl.ContentTemplateProperty.AddOwner(
            typeof(ContentAdorner),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>Identifies the <see cref="ContentTemplateSelector"/> dependency property.</summary>
        public static readonly DependencyProperty ContentTemplateSelectorProperty = ContentControl.ContentTemplateSelectorProperty.AddOwner(
                typeof(ContentAdorner),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>Identifies the <see cref="ContentPresenterStyle"/> dependency property.</summary>
        public static readonly DependencyProperty ContentPresenterStyleProperty = DependencyProperty.Register(
            nameof(ContentPresenterStyle),
            typeof(Style),
            typeof(ContentAdorner),
            new PropertyMetadata(default(Style)));

        static ContentAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentAdorner), new FrameworkPropertyMetadata(typeof(ContentAdorner)));
        }

        public ContentAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            this.Child = new ContentPresenter();
            this.Child.Bind(MarginProperty)
                .OneWayTo(this, MarginProperty);
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

        protected override Size MeasureOverride(Size constraint)
        {
            var desiredSize = this.AdornedElement.RenderSize;
            this.Child?.Measure(desiredSize);
            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.Child?.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return this.Child?.RenderSize ?? base.ArrangeOverride(finalSize);
        }
    }
}
