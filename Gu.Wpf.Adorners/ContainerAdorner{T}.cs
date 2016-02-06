namespace Gu.Wpf.Adorners
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Markup;
    using System.Windows.Media;

    [ContentProperty("Child")]
    public abstract class ContainerAdorner<T> : Adorner
        where T : Visual
    {
        private T child;

        protected ContainerAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
        }

        [DefaultValue(null)]
        public T Child
        {
            get
            {
                return this.child;
            }
            set
            {
                var old = this.child;
                if (!ReferenceEquals(old, value))
                {
                    this.RemoveVisualChild(old);
                    //need to remove old element from logical tree
                    this.RemoveLogicalChild(old);
                    this.child = value;

                    this.AddVisualChild(this.child);
                    this.AddLogicalChild(value);
                    this.InvalidateMeasure();
                }
            }
        }

        protected override int VisualChildrenCount => this.child == null ? 0 : 1;

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

        public virtual void ClearChild()
        {
            this.RemoveVisualChild(this.child);
            this.RemoveLogicalChild(this.child);
            this.child = null;
        }
    }
}