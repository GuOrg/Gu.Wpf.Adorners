namespace Gu.Wpf.Adorners;

using System;
using System.Windows;

internal sealed class IsKeyboardFocusWithinChangedEventManager : WeakEventManager
{
    private IsKeyboardFocusWithinChangedEventManager()
    {
    }

    // get the event manager for the current thread
    private static IsKeyboardFocusWithinChangedEventManager CurrentManager
    {
        get
        {
            var managerType = typeof(IsKeyboardFocusWithinChangedEventManager);
            var manager = (IsKeyboardFocusWithinChangedEventManager)GetCurrentManager(managerType);

            // at first use, create and register a new manager
            if (manager is null)
            {
                manager = new IsKeyboardFocusWithinChangedEventManager();
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
            element.IsKeyboardFocusWithinChanged += this.OnIsKeyboardFocusWithinChanged;
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
            element.IsKeyboardFocusWithinChanged -= this.OnIsKeyboardFocusWithinChanged;
        }
        else
        {
            // ReSharper disable once ConstantConditionalAccessQualifier
            // ReSharper disable once ConstantNullCoalescingCondition
            throw new ArgumentException($"Cannot stop listening to {source?.GetType().Name ?? "null"}");
        }
    }

    // event handler for IsKeyboardFocusWithinChanged event
    private void OnIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs args)
    {
        this.DeliverEvent(sender, EventArgs.Empty);
    }
}
