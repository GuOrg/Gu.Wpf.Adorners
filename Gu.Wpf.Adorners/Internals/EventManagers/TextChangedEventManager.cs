namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// Manager for the TextBoxBase.TextChanged event.
    /// </summary>
    internal class TextChangedEventManager : WeakEventManager
    {
        private TextChangedEventManager()
        {
        }

        // get the event manager for the current thread
        private static TextChangedEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(TextChangedEventManager);
                var manager = (TextChangedEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager is null)
                {
                    manager = new TextChangedEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }

        internal static void UpdateHandler(TextBoxBase source, EventHandler<RoutedEventArgs> handler)
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
            if (source is TextBoxBase textBox)
            {
                textBox.TextChanged += this.OnTextChanged;
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
            if (source is TextBoxBase textBox)
            {
                textBox.TextChanged -= this.OnTextChanged;
            }
            else
            {
                // ReSharper disable once ConstantConditionalAccessQualifier
                // ReSharper disable once ConstantNullCoalescingCondition
                throw new ArgumentException($"Cannot stop listening to {source?.GetType().Name ?? "null"}");
            }
        }

        // event handler for TextChanged event
        private void OnTextChanged(object sender, RoutedEventArgs args)
        {
            this.DeliverEvent(sender, args);
        }
    }
}
