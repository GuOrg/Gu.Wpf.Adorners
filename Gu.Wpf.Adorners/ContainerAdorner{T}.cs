namespace Gu.Wpf.Adorners
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Markup;
    using System.Windows.Media;

    /// <summary>
    /// Base class for adorners rendering content.
    /// </summary>
    /// <typeparam name="T">The type of visual to use for rendering content.</typeparam>
    [ContentProperty(nameof(Child))]
    public abstract class ContainerAdorner<T> : Adorner
        where T : Visual
    {
        private T? child;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerAdorner{T}"/> class.
        /// </summary>
        protected ContainerAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
        }

        /// <summary>
        /// Gets or sets the visual that renders the content.
        /// marked virtual because AddVisualChild calls the virtual OnVisualChildrenChanged.
        /// </summary>
        [DefaultValue(null)]
        public virtual T? Child
        {
            get => this.child;

            set
            {
                var old = this.child;
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

        /// <inheritdoc />
        protected override int VisualChildrenCount => this.child == null ? 0 : 1;

        /// <inheritdoc />
        protected override IEnumerator LogicalChildren => this.child == null
                                              ? EmptyEnumerator.Instance
                                              : new SingleChildEnumerator(this.child);

        /// <summary>
        /// Set child to null and remove it as visual and logical child.
        /// </summary>
        public virtual void ClearChild()
        {
            this.RemoveVisualChild(this.child);
            this.RemoveLogicalChild(this.child);
            this.child = null;
        }

        /// <inheritdoc />
        protected override Visual GetVisualChild(int index)
        {
            if (this.child == null || index != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.child;
        }
    }
}
