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

        /// <summary>
        /// Add a listener to the given source's event.
        /// </summary>
        public static void AddListener(FrameworkElement source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(
                source ?? throw new ArgumentNullException(nameof(source)),
                listener ?? throw new ArgumentNullException(nameof(listener)));
        }

        /// <summary>
        /// Remove a listener to the given source's event.
        /// </summary>
        public static void RemoveListener(FrameworkElement source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(
                source ?? throw new ArgumentNullException(nameof(source)),
                listener ?? throw new ArgumentNullException(nameof(listener)));
        }

        /// <summary>
        /// Add a handler for the given source's event.
        /// </summary>
        public static void AddHandler(FrameworkElement source, EventHandler handler)
        {
            CurrentManager.ProtectedAddHandler(
                source,
                handler ?? throw new ArgumentNullException(nameof(handler)));
        }

        /// <summary>
        /// Remove a handler for the given source's event.
        /// </summary>
        public static void RemoveHandler(FrameworkElement source, EventHandler handler)
        {
            CurrentManager.ProtectedRemoveHandler(
                source,
                handler ?? throw new ArgumentNullException(nameof(handler)));
        }

        /// <inheritdoc />
        protected override ListenerList NewListenerList() => new ListenerList<EventArgs>();

        /// <inheritdoc />
        protected override void StartListening(object source)
        {
            if (source is UIElement fe)
            {
                fe.IsVisibleChanged += this.OnIsVisibleChanged;
            }
            else
            {
                throw new ArgumentException($"Cannot start listening to {source?.GetType().Name ?? "null"}");
            }
        }

        /// <inheritdoc />
        protected override void StopListening(object source)
        {
            if (source is UIElement fe)
            {
                fe.IsVisibleChanged -= this.OnIsVisibleChanged;
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
