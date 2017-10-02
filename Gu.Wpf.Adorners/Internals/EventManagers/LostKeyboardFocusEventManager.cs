namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;

    /// <summary>
    /// Manager for the DependencyObject.LostKeyboardFocus event.
    /// </summary>
    internal class LostKeyboardFocusEventManager : WeakEventManager
    {
        private LostKeyboardFocusEventManager()
        {
        }

        // get the event manager for the current thread
        private static LostKeyboardFocusEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(LostKeyboardFocusEventManager);
                var manager = (LostKeyboardFocusEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager == null)
                {
                    manager = new LostKeyboardFocusEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }

        internal static void UpdateHandler(DependencyObject source, EventHandler<RoutedEventArgs> handler)
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
        protected override ListenerList NewListenerList() => new ListenerList<RoutedEventArgs>();

        /// <inheritdoc />
        protected override void StartListening(object source)
        {
            if (source is FrameworkElement fe)
            {
                fe.LostKeyboardFocus += this.OnLostKeyboardFocus;
            }
            else if (source is FrameworkContentElement fce)
            {
                fce.LostKeyboardFocus += this.OnLostKeyboardFocus;
            }
            else
            {
                throw new ArgumentException($"Cannot start listening to {source?.GetType().Name ?? "null"}");
            }
        }

        /// <inheritdoc />
        protected override void StopListening(object source)
        {
            if (source is FrameworkElement fe)
            {
                fe.LostKeyboardFocus -= this.OnLostKeyboardFocus;
            }
            else if (source is FrameworkContentElement fce)
            {
                fce.LostKeyboardFocus -= this.OnLostKeyboardFocus;
            }
            else
            {
                throw new ArgumentException($"Cannot stop listening to {source?.GetType().Name ?? "null"}");
            }
        }

        // event handler for LostKeyboardFocus event
        private void OnLostKeyboardFocus(object sender, RoutedEventArgs args)
        {
            this.DeliverEvent(sender, args);
        }
    }
}
