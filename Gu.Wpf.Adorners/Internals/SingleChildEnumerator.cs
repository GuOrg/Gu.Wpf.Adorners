namespace Gu.Wpf.Adorners
{
    using System.Collections;

    internal sealed class SingleChildEnumerator : IEnumerator
    {
        private readonly int count;
        private readonly object child;
        private int index = -1;

        internal SingleChildEnumerator(object child)
        {
            this.child = child;
            this.count = child is null ? 0 : 1;
        }

        object? IEnumerator.Current => this.index == 0 ? this.child : null;

        bool IEnumerator.MoveNext()
        {
            this.index++;
            return this.index < this.count;
        }

        void IEnumerator.Reset()
        {
            this.index = -1;
        }
    }
}
