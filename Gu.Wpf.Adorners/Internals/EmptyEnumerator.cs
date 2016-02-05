namespace Gu.Wpf.Adorners
{
    using System;
    using System.Collections;

    /// <summary>
    /// Returns an Enumerator that enumerates over nothing.
    /// </summary>
    internal class EmptyEnumerator : IEnumerator
    {
        /// <summary>
        /// Read-Only instance of an Empty Enumerator.
        /// </summary>
        public static readonly IEnumerator Instance = new EmptyEnumerator();

        // singleton class, private ctor
        private EmptyEnumerator()
        {
        }

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/>
        /// </summary>
        object IEnumerator.Current
        {
            get { throw new InvalidOperationException(); }
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        public void Reset()
        {
        }

        /// <summary>
        /// Returns false.
        /// </summary>
        /// <returns>false</returns>
        public bool MoveNext()
        {
            return false;
        }
    }
}