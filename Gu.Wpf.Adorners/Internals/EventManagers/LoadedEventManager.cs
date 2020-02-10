namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;

    /// <summary>
    /// Manager for the DependencyObject.Loaded event.
    /// </summary>
    internal class LoadedEventManager : WeakEventManager
    {
        private LoadedEventManager()
        {
        }

        // get the event manager for the current thread
        private static LoadedEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(LoadedEventManager);
                var manager = (LoadedEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager is null)
                {
                    manager = new LoadedEventManager();
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
                fe.Loaded += this.OnLoaded;
            }
            else if (source is FrameworkContentElement fce)
            {
                fce.Loaded += this.OnLoaded;
            }
            else
            {
                // ReSharper disable once ConstantConditionalAccessQualifier
                // ReSharper disable once ConstantNullCoalescingCondition
                throw new ArgumentException($"Cannot start listening to {source?.GetType().Name ?? "null"}");
            }
        }

        /// <inheritdoc />
        protected override void StopListening(object source)
        {
            if (source is FrameworkElement fe)
            {
                fe.Loaded -= this.OnLoaded;
            }
            else if (source is FrameworkContentElement fce)
            {
                fce.Loaded -= this.OnLoaded;
            }
            else
            {
                // ReSharper disable once ConstantConditionalAccessQualifier
                // ReSharper disable once ConstantNullCoalescingCondition
                throw new ArgumentException($"Cannot stop listening to {source?.GetType().Name ?? "null"}");
            }
        }

        // event handler for Loaded event
        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            this.DeliverEvent(sender, args);
        }
    }
}
