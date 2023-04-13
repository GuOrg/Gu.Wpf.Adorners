namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Manager for the PasswordBox.PasswordChanged event.
    /// </summary>
    internal sealed class PasswordChangedEventManager : WeakEventManager
    {
        private PasswordChangedEventManager()
        {
        }

        // get the event manager for the current thread
        private static PasswordChangedEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(PasswordChangedEventManager);
                var manager = (PasswordChangedEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager is null)
                {
                    manager = new PasswordChangedEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }

        internal static void UpdateHandler(PasswordBox source, EventHandler<RoutedEventArgs> handler)
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
            if (source is PasswordBox textBox)
            {
                textBox.PasswordChanged += this.OnPasswordChanged;
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
            if (source is PasswordBox textBox)
            {
                textBox.PasswordChanged -= this.OnPasswordChanged;
            }
            else
            {
                // ReSharper disable once ConstantConditionalAccessQualifier
                // ReSharper disable once ConstantNullCoalescingCondition
                throw new ArgumentException($"Cannot stop listening to {source?.GetType().Name ?? "null"}");
            }
        }

        // event handler for PasswordChanged event
        private void OnPasswordChanged(object sender, RoutedEventArgs args)
        {
            this.DeliverEvent(sender, args);
        }
    }
}
