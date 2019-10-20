namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Manager for the DependencyObject.TextInput event.
    /// </summary>
    internal class TextInputEventManager : WeakEventManager
    {
        private TextInputEventManager()
        {
        }

        // get the event manager for the current thread
        private static TextInputEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(TextInputEventManager);
                var manager = (TextInputEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager == null)
                {
                    manager = new TextInputEventManager();
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
                element.TextInput += this.OnTextInput;
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
            if (source is UIElement element)
            {
                element.TextInput -= this.OnTextInput;
            }
            else
            {
                // ReSharper disable once ConstantConditionalAccessQualifier
                // ReSharper disable once ConstantNullCoalescingCondition
                throw new ArgumentException($"Cannot stop listening to {source?.GetType().Name ?? "null"}");
            }
        }

        // event handler for TextInput event
        private void OnTextInput(object sender, TextCompositionEventArgs args)
        {
            this.DeliverEvent(sender, args);
        }
    }
}
