//namespace Gu.Wpf.Adorners
//{
//    using System;
//    using System.Windows;
//    using System.Windows.Controls;
//    using System.Windows.Documents;
//    using System.Windows.Media;

//    /// <summary>
//    /// http://tech.pro/tutorial/856/wpf-tutorial-using-a-visual-collection
//    /// </summary>
//    public class ContentAdorner : Adorner
//    {
//        private readonly VisualCollection children;
//        private readonly ContentPresenter contentPresenter;

//        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
//            "Content",
//            typeof(object),
//            typeof(ContentAdorner),
//            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.AffectsMeasure));

//        public static readonly DependencyProperty DataTemplateProperty = DependencyProperty.Register(
//            "DataTemplate",
//            typeof(DataTemplate),
//            typeof(ContentAdorner),
//            new PropertyMetadata(default(DataTemplate)));

//        public static readonly DependencyProperty DataTemplateSelectorProperty = DependencyProperty.Register(
//            "DataTemplateSelector",
//            typeof(DataTemplateSelector),
//            typeof(ContentAdorner),
//            new PropertyMetadata(default(DataTemplateSelector)));

//        static ContentAdorner()
//        {
//            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentAdorner), new FrameworkPropertyMetadata(typeof(ContentAdorner)));
//        }

//        public ContentAdorner(UIElement adornedElement, object content)
//            : base(adornedElement)
//        {
//            this.children = new VisualCollection(this);
//            this.contentPresenter = new ContentPresenter();
//            this.children.Add(this.contentPresenter);
//            BindingHelper.Bind(this.contentPresenter, MarginProperty, this, MarginProperty);
//            BindingHelper.Bind(this.contentPresenter, ContentPresenter.ContentProperty, this, ContentProperty);
//            BindingHelper.Bind(this.contentPresenter, ContentPresenter.ContentTemplateProperty, this, DataTemplateProperty);
//            BindingHelper.Bind(this.contentPresenter, ContentPresenter.ContentTemplateSelectorProperty, this, DataTemplateSelectorProperty);
//            Content = content;
//        }

//        protected override int VisualChildrenCount => 1;

//        public object Content
//        {
//            get { return GetValue(ContentProperty); }
//            set { SetValue(ContentProperty, value); }
//        }

//        public DataTemplate DataTemplate
//        {
//            get { return (DataTemplate)GetValue(DataTemplateProperty); }
//            set { SetValue(DataTemplateProperty, value); }
//        }

//        public DataTemplateSelector DataTemplateSelector
//        {
//            get { return (DataTemplateSelector)GetValue(DataTemplateSelectorProperty); }
//            set { SetValue(DataTemplateSelectorProperty, value); }
//        }

//        protected override Size MeasureOverride(Size constraint)
//        {
//            this.contentPresenter.Measure(constraint);
//            return constraint;
//        }

//        protected override Size ArrangeOverride(Size finalSize)
//        {
//            this.contentPresenter.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
//            return this.contentPresenter.RenderSize;
//        }

//        protected override Visual GetVisualChild(int index)
//        {
//            if (index != 0)
//            {
//                throw new ArgumentOutOfRangeException("index");
//            }
//            return this.contentPresenter;
//        }
//    }
//}
