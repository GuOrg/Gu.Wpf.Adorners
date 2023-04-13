namespace Gu.Wpf.Adorners;

using System;
using System.Windows;

/// <summary>
/// Manager for the DependencyObject.SizeChanged event.
/// </summary>
internal sealed class SizeChangedEventManager : WeakEventManager
{
    private SizeChangedEventManager()
    {
    }

    // get the event manager for the current thread
    private static SizeChangedEventManager CurrentManager
    {
        get
        {
            var managerType = typeof(SizeChangedEventManager);
            var manager = (SizeChangedEventManager)GetCurrentManager(managerType);

            // at first use, create and register a new manager
            if (manager is null)
            {
                manager = new SizeChangedEventManager();
                SetCurrentManager(managerType, manager);
            }

            return manager;
        }
    }

    internal static void UpdateHandler(FrameworkElement source, EventHandler<RoutedEventArgs> handler)
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
            fe.SizeChanged += this.OnSizeChanged;
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
            fe.SizeChanged -= this.OnSizeChanged;
        }
        else
        {
            // ReSharper disable once ConstantConditionalAccessQualifier
            // ReSharper disable once ConstantNullCoalescingCondition
            throw new ArgumentException($"Cannot stop listening to {source?.GetType().Name ?? "null"}");
        }
    }

    // event handler for SizeChanged event
    private void OnSizeChanged(object sender, RoutedEventArgs args)
    {
        this.DeliverEvent(sender, args);
    }
}
