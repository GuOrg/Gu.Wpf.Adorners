namespace Gu.Wpf.Adorners
{
    using System.Windows;
    using System.Windows.Controls;

    public class ContentDragAdorner : DragAdorner<ContentPresenter>
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
            nameof(ContentPresenterStyle),
            typeof(Style),
            typeof(ContentDragAdorner),
            new PropertyMetadata(default(Style)));

        static ContentDragAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentDragAdorner), new FrameworkPropertyMetadata(typeof(ContentDragAdorner)));
        }

        public ContentDragAdorner(UIElement adornedElement)
            : base(adornedElement, new ContentPresenter())
        {
            this.Child.Bind(ContentPresenter.ContentProperty)
                .OneWayTo(this, ContentProperty);
            this.Child.Bind(ContentPresenter.ContentTemplateProperty)
                .OneWayTo(this, ContentTemplateProperty);
            this.Child.Bind(ContentPresenter.ContentTemplateSelectorProperty)
                .OneWayTo(this, ContentTemplateSelectorProperty);
            this.Child.Bind(StyleProperty)
                .OneWayTo(this, ContentPresenterStyleProperty);
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
    }
}