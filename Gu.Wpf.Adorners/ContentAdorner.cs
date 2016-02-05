namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// http://tech.pro/tutorial/856/wpf-tutorial-using-a-visual-collection
    /// </summary>
    public class ContentAdorner : Adorner
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly VisualCollection children;
        private readonly ContentPresenter contentPresenter;

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

        static ContentAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentAdorner), new FrameworkPropertyMetadata(typeof(ContentAdorner)));
        }

        public ContentAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            this.children = new VisualCollection(this);
            this.contentPresenter = new ContentPresenter();
            this.children.Add(this.contentPresenter);
            this.contentPresenter.Bind(MarginProperty)
                .OneWayTo(this, MarginProperty);
            this.contentPresenter.Bind(ContentPresenter.ContentProperty)
                .OneWayTo(this, ContentProperty);
            this.contentPresenter.Bind(ContentPresenter.ContentTemplateProperty)
                .OneWayTo(this, ContentTemplateProperty);
            this.contentPresenter.Bind(ContentPresenter.ContentTemplateSelectorProperty)
                .OneWayTo(this, ContentTemplateSelectorProperty);
        }

        protected override int VisualChildrenCount => 1;

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public DataTemplateSelector ContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty); }
            set { SetValue(ContentTemplateSelectorProperty, value); }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var desiredSize = AdornedElement.RenderSize;
            this.contentPresenter.Measure(desiredSize);
            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.contentPresenter.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return this.contentPresenter.RenderSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.contentPresenter;
        }
    }
}
