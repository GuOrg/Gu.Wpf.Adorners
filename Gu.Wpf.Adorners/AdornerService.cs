namespace Gu.Wpf.Adorners;

using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;

/// <summary>
/// Helper class for adding and removing adorners to the <see cref="AdornerLayer"/>.
/// </summary>
public static class AdornerService
{
    private static readonly DependencyProperty AdornerLayerProperty = DependencyProperty.RegisterAttached(
        "AdornerLayer",
        typeof(AdornerLayer),
        typeof(AdornerService),
        new PropertyMetadata(default(AdornerLayer)));

    /// <summary>
    /// Adds <paramref name="adorner"/> to the <see cref="AdornerLayer"/>
    /// If no adorner layer is present a retry is performed with  DispatcherPriority.Loaded.
    /// </summary>
    /// <param name="adorner">The <see cref="Adorner"/>.</param>
    public static void Show(Adorner adorner)
    {
        if (adorner is null)
        {
            throw new System.ArgumentNullException(nameof(adorner));
        }

        Show(adorner, retry: true);
    }

    /// <summary>
    /// Removes <paramref name="adorner"/> from the <see cref="AdornerLayer"/>.
    /// </summary>
    /// <param name="adorner">The <see cref="Adorner"/>.</param>
    public static void Remove(Adorner adorner)
    {
        if (adorner is null)
        {
            throw new System.ArgumentNullException(nameof(adorner));
        }

        var adornerLayer = (AdornerLayer)adorner.GetValue(AdornerLayerProperty) ??
                           GetAdornerLayer(adorner.AdornedElement);
        adornerLayer?.Remove(adorner);
        adorner.ClearValue(AdornerLayerProperty);
    }

    /// <summary>
    /// Calls <see cref="AdornerLayer.GetAdornerLayer"/> unless <paramref name="adornedElement"/> is a window
    /// For window we fall back on finding the first <see cref="AdornerDecorator"/> and returning its <see cref="AdornerDecorator.AdornerLayer"/>.
    /// </summary>
    /// <param name="adornedElement">The adorned element.</param>
    /// <returns>First AdornerLayer above given element, or null.</returns>
    public static AdornerLayer? GetAdornerLayer(UIElement adornedElement)
    {
        if (adornedElement is Window window)
        {
            return AdornerLayer.GetAdornerLayer(adornedElement) ??
                   window.FirstRecursiveVisualChild<AdornerDecorator>()?.AdornerLayer;
        }

        return AdornerLayer.GetAdornerLayer(adornedElement);
    }

    private static void Show(Adorner adorner, bool retry)
    {
        var adornerLayer = GetAdornerLayer(adorner.AdornedElement);
        if (adornerLayer != null)
        {
            adornerLayer.Remove(adorner);
            adornerLayer.Add(adorner);
            adorner.SetCurrentValue(AdornerLayerProperty, adornerLayer);
        }
        else if (retry)
        {
            // try again later, perhaps giving layout a chance to create the adorner layer
#pragma warning disable VSTHRD001 // Avoid legacy thread switching APIs
            _ = adorner.Dispatcher.BeginInvoke(
#pragma warning restore VSTHRD001 // Avoid legacy thread switching APIs
                DispatcherPriority.Loaded,
                new DispatcherOperationCallback(ShowAdornerOperation),
                new object[] { adorner });
        }
    }

    private static object? ShowAdornerOperation(object arg)
    {
        var args = (object[])arg;
        var adorner = (Adorner)args[0];
        Show(adorner, retry: false);
        return null;
    }
}
