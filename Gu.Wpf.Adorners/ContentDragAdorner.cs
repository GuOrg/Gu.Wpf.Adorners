namespace Gu.Wpf.Adorners
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// An <see cref="System.Windows.Documents.Adorner"/> that renders content using a <see cref="ContentPresenter"/>.
    /// </summary>
    public class ContentDragAdorner : DragAdorner<ContentPresenter>
    {
        /// <summary>Identifies the <see cref="Content"/> dependency property.</summary>
        public static readonly DependencyProperty ContentProperty = ContentControl.ContentProperty.AddOwner(
            typeof(ContentDragAdorner),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>Identifies the <see cref="ContentTemplate"/> dependency property.</summary>
        public static readonly DependencyProperty ContentTemplateProperty = ContentControl.ContentTemplateProperty.AddOwner(
            typeof(ContentDragAdorner),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>Identifies the <see cref="ContentTemplateSelector"/> dependency property.</summary>
        public static readonly DependencyProperty ContentTemplateSelectorProperty = ContentControl.ContentTemplateSelectorProperty.AddOwner(
                typeof(ContentDragAdorner),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>Identifies the <see cref="ContentPresenterStyle"/> dependency property.</summary>
        public static readonly DependencyProperty ContentPresenterStyleProperty = DependencyProperty.Register(
            nameof(ContentPresenterStyle),
            typeof(Style),
            typeof(ContentDragAdorner),
            new PropertyMetadata(default(Style)));

        static ContentDragAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentDragAdorner), new FrameworkPropertyMetadata(typeof(ContentDragAdorner)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDragAdorner"/> class.
        /// </summary>
        public ContentDragAdorner(UIElement adornedElement)
            : base(adornedElement, new ContentPresenter())
        {
            this.Child.Bind(ContentPresenter.ContentProperty)
                .OneWayTo(this, ContentProperty)
                .IgnoreReturnValue();
            this.Child.Bind(ContentPresenter.ContentTemplateProperty)
                .OneWayTo(this, ContentTemplateProperty)
                .IgnoreReturnValue();
            this.Child.Bind(ContentPresenter.ContentTemplateSelectorProperty)
                .OneWayTo(this, ContentTemplateSelectorProperty)
                .IgnoreReturnValue();
            this.Child.Bind(StyleProperty)
                .OneWayTo(this, ContentPresenterStyleProperty)
                .IgnoreReturnValue();
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
        /// Gets or sets the <see cref="Style"/> for the <see cref="ContentPresenter"/>.
        /// </summary>
        public Style ContentPresenterStyle
        {
            get => (Style)this.GetValue(ContentPresenterStyleProperty);
            set => this.SetValue(ContentPresenterStyleProperty, value);
        }
    }
}
