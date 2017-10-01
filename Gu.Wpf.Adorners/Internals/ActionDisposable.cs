namespace Gu.Wpf.Adorners
{
    using System;
    using System.Threading;

    internal sealed class ActionDisposable : IDisposable
    {
        private volatile Action dispose;

        public ActionDisposable(Action dispose)
        {
            this.dispose = dispose;
        }

        public void Dispose()
        {
#pragma warning disable 0420
            var action = Interlocked.Exchange(ref this.dispose, null);
#pragma warning restore 0420
            action?.Invoke();
        }
    }
}