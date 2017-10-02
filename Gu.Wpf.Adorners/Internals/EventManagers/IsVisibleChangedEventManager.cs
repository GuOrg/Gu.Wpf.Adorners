namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;

    /// <summary>
    /// Manager for the DependencyObject.IsVisibleChanged event.
    /// </summary>
    internal class IsVisibleChangedEventManager : WeakEventManager
    {
        private IsVisibleChangedEventManager()
        {
        }

        // get the event manager for the current thread
        private static IsVisibleChangedEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(IsVisibleChangedEventManager);
                var manager = (IsVisibleChangedEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager == null)
                {
                    manager = new IsVisibleChangedEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }

        internal static void UpdateHandler(UIElement source, EventHandler<EventArgs> handler)
        {
            var manager = CurrentManager;
            manager.ProtectedRemoveHandler(
                source ?? throw new ArgumentNullException(nameof(source)),
                handler ?? throw new ArgumentNullException(nameof(handler)));

            manager.ProtectedAddHandler(
                source,
                handler);
        }

        /// <inheritdoc />
        protected override ListenerList NewListenerList() => new ListenerList<EventArgs>();

        /// <inheritdoc />
        protected override void StartListening(object source)
        {
            if (source is UIElement element)
            {
                element.IsVisibleChanged += this.OnIsVisibleChanged;
            }
            else
            {
                throw new ArgumentException($"Cannot start listening to {source?.GetType().Name ?? "null"}");
            }
        }

        /// <inheritdoc />
        protected override void StopListening(object source)
        {
            if (source is UIElement element)
            {
                element.IsVisibleChanged -= this.OnIsVisibleChanged;
            }
            else
            {
                throw new ArgumentException($"Cannot stop listening to {source?.GetType().Name ?? "null"}");
            }
        }

        // event handler for IsVisibleChanged event
        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            this.DeliverEvent(sender, EventArgs.Empty);
        }
    }
}
