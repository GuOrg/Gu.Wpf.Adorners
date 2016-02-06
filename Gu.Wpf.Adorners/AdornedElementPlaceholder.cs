namespace Gu.Wpf.Adorners
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media;

    /// <summary>
    /// ~Inspired~ by: http://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Controls/AdornedElementPlaceholder.cs,a47108e4407d047a
    /// Due to poor design in WPF this needs to be reimplemented.
    /// </summary>
    [ContentProperty("Child")]
    public class AdornedElementPlaceholder : FrameworkElement
    {
        private UIElement child;
        private TemplatedAdorner templatedAdorner;

        ///<summary>
        /// Element for which the AdornedElementPlaceholder is reserving space.
        ///</summary>
        public UIElement AdornedElement
        {
            get
            {
                var adorner = this.TemplatedAdorner;
                return adorner == null ? null : this.TemplatedAdorner.AdornedElement;
            }
        }

        [DefaultValue(null)]
        public virtual UIElement Child
        {
            get
            {
                return this.child;
            }

            set
            {
                UIElement old = this.child;

                if (!ReferenceEquals(old, value))
                {
                    this.RemoveVisualChild(old);
                    this.RemoveLogicalChild(old);
                    this.child = value;
                    this.AddVisualChild(this.child);
                    this.AddLogicalChild(value);
                    this.InvalidateMeasure();
                }
            }
        }

        protected override int VisualChildrenCount => this.child == null ? 0 : 1;

        /// <summary>
        /// Gets the Visual child at the specified index.
        /// </summary>
        protected override Visual GetVisualChild(int index)
        {
            if (this.child == null || index != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.child;
        }

        protected override IEnumerator LogicalChildren => this.child == null
                                              ? EmptyEnumerator.Instance
                                              : new SingleChildEnumerator(this.child);
        private TemplatedAdorner TemplatedAdorner
        {
            get
            {
                if (this.templatedAdorner == null)
                {
                    // find the parent Adorner
                    var templateParent = this.TemplatedParent as FrameworkElement;

                    if (templateParent != null)
                    {
                        this.templatedAdorner = VisualTreeHelper.GetParent(templateParent) as TemplatedAdorner;

                        if (this.templatedAdorner != null && this.templatedAdorner.ReferenceElement == null)
                        {
                            this.templatedAdorner.ReferenceElement = this;
                        }
                    }
                }

                return this.templatedAdorner;
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            if (this.TemplatedParent == null)
            {
                throw new InvalidOperationException($"{nameof(AdornedElementPlaceholder)} can only be used in a Template");
            }

            base.OnInitialized(e);
        }

        /// <summary>
        ///     AdornedElementPlaceholder measure behavior is to measure
        ///     only the first visual child.  Note that the return value
        ///     of Measure on this child is ignored as the purpose of this
        ///     class is to match the size of the element for which this
        ///     is a placeholder.
        /// </summary>
        /// <param name="constraint">The measurement constraints.</param>
        /// <returns>The desired size of the control.</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            if (this.TemplatedParent == null)
            {
                throw new InvalidOperationException($"{nameof(AdornedElementPlaceholder)} can only be used in a Template");
            }

            if (this.AdornedElement == null)
            {
                return new Size(0, 0);
            }

            var desiredSize = this.AdornedElement.RenderSize;
            this.Child?.Measure(desiredSize);
            return desiredSize;
        }

        /// <summary>
        ///     Default AdornedElementPlaceholder arrangement is to only arrange
        ///     the first visual child. No transforms will be applied.
        /// </summary>
        /// <param name="arrangeBounds">The computed size.</param>
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            this.Child?.Arrange(new Rect(arrangeBounds));
            return arrangeBounds;
        }
    }
}
