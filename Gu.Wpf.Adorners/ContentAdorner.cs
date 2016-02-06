namespace Gu.Wpf.Adorners
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// http://tech.pro/tutorial/856/wpf-tutorial-using-a-visual-collection
    /// </summary>
    public sealed class ContentAdorner : ContainerAdorner<ContentPresenter>
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content",
            typeof(object),
            typeof(ContentAdorner),
            new FrameworkPropertyMetadata(
                default(object),
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(
            "ContentTemplate",
            typeof(DataTemplate),
            typeof(ContentAdorner),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register(
            "ContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(ContentAdorner),
            new PropertyMetadata(default(DataTemplateSelector)));

        public static readonly DependencyProperty ContentPresenterStyleProperty = DependencyProperty.Register(
            "ContentPresenterStyle",
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

        public object Content
        {
            get { return this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)this.GetValue(ContentTemplateProperty); }
            set { this.SetValue(ContentTemplateProperty, value); }
        }

        public DataTemplateSelector ContentTemplateSelector
        {
            get { return (DataTemplateSelector)this.GetValue(ContentTemplateSelectorProperty); }
            set { this.SetValue(ContentTemplateSelectorProperty, value); }
        }

        public Style ContentPresenterStyle
        {
            get { return (Style)this.GetValue(ContentPresenterStyleProperty); }
            set { this.SetValue(ContentPresenterStyleProperty, value); }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var desiredSize = this.AdornedElement.RenderSize;
            this.Child.Measure(desiredSize);
            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.Child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return this.Child.RenderSize;
        }
    }
}
