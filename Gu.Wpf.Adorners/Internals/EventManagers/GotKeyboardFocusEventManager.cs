namespace Gu.Wpf.Adorners;

using System;
using System.Windows;

/// <summary>
/// Manager for the DependencyObject.GotKeyboardFocus event.
/// </summary>
internal sealed class GotKeyboardFocusEventManager : WeakEventManager
{
    private GotKeyboardFocusEventManager()
    {
    }

    // get the event manager for the current thread
    private static GotKeyboardFocusEventManager CurrentManager
    {
        get
        {
            var managerType = typeof(GotKeyboardFocusEventManager);
            var manager = (GotKeyboardFocusEventManager)GetCurrentManager(managerType);

            // at first use, create and register a new manager
            if (manager is null)
            {
                manager = new GotKeyboardFocusEventManager();
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
            fe.GotKeyboardFocus += this.OnGotKeyboardFocus;
        }
        else if (source is FrameworkContentElement fce)
        {
            fce.GotKeyboardFocus += this.OnGotKeyboardFocus;
        }
        else
        {
            // ReSharper disable once ConstantConditionalAccessQualifier R# dumbs it here
            // ReSharper disable once ConstantNullCoalescingCondition
            throw new ArgumentException($"Cannot start listening to {source?.GetType().Name ?? "null"}");
        }
    }

    /// <inheritdoc />
    protected override void StopListening(object source)
    {
        if (source is FrameworkElement fe)
        {
            fe.GotKeyboardFocus -= this.OnGotKeyboardFocus;
        }
        else if (source is FrameworkContentElement fce)
        {
            fce.GotKeyboardFocus -= this.OnGotKeyboardFocus;
        }
        else
        {
            // ReSharper disable once ConstantConditionalAccessQualifier
            // ReSharper disable once ConstantNullCoalescingCondition
            throw new ArgumentException($"Cannot stop listening to {source?.GetType().Name ?? "null"}");
        }
    }

    // event handler for GotKeyboardFocus event
    private void OnGotKeyboardFocus(object sender, RoutedEventArgs args)
    {
        this.DeliverEvent(sender, args);
    }
}
