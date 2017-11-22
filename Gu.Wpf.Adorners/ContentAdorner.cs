namespace Gu.Wpf.Adorners
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// An <see cref="System.Windows.Documents.Adorner"/>
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentAdorner"/> class.
        /// </summary>
        public ContentAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            base.Child = new ContentPresenter();
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

        /// <inheritdoc />
        public sealed override ContentPresenter Child
        {
            get => base.Child;
            set => base.Child = value;
        }

        /// <summary>
        /// Gets or sets the content is the data used to generate the child elements of this control.
        /// </summary>
        public object Content
        {
            get => this.GetValue(ContentProperty);
            set => this.SetValue(ContentProperty, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> used to display the content of the control.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get => (DataTemplate)this.GetValue(ContentTemplateProperty);
            set => this.SetValue(ContentTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="DataTemplateSelector"/> that allows the application writer to provide custom logic
        /// for choosing the template used to display the content of the control.
        /// </summary>
        /// <remarks>
        /// This property is ignored if <seealso cref="ContentTemplate"/> is set.
        /// </remarks>
        public DataTemplateSelector ContentTemplateSelector
        {
            get => (DataTemplateSelector)this.GetValue(ContentTemplateSelectorProperty);
            set => this.SetValue(ContentTemplateSelectorProperty, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Style"/> for <see cref="Child"/>
        /// </summary>
        public Style ContentPresenterStyle
        {
            get => (Style)this.GetValue(ContentPresenterStyleProperty);
            set => this.SetValue(ContentPresenterStyleProperty, value);
        }

        /// <inheritdoc />
        protected override Size MeasureOverride(Size constraint)
        {
            var desiredSize = this.AdornedElement.RenderSize;
            this.Child?.Measure(desiredSize);
            return desiredSize;
        }

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.Child?.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return this.Child?.RenderSize ?? base.ArrangeOverride(finalSize);
        }
    }
}
