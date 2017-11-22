namespace Gu.Wpf.Adorners
{
    using System.Windows.Documents;
    using System.Windows.Threading;

    /// <summary>
    /// Helper class for adding and removing adorners to the <see cref="AdornerLayer"/>
    /// </summary>
    public static class AdornerService
    {
        /// <summary>
        /// Adds <paramref name="adorner"/> to the <see cref="AdornerLayer"/>
        /// If no adorner layer is present a retry is performed with  DispatcherPriority.Loaded
        /// </summary>
        /// <param name="adorner">The <see cref="Adorner"/></param>
        public static void Show(Adorner adorner)
        {
            Show(adorner, retry: true);
        }

        /// <summary>
        /// Removes <paramref name="adorner"/> from the <see cref="AdornerLayer"/>
        /// </summary>
        /// <param name="adorner">The <see cref="Adorner"/></param>
        public static void Remove(Adorner adorner)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(adorner.AdornedElement);
            adornerLayer?.Remove(adorner);
        }

        private static void Show(Adorner adorner, bool retry)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(adorner.AdornedElement);
            if (adornerLayer != null)
            {
                adornerLayer.Remove(adorner);
                adornerLayer.Add(adorner);
            }
            else if (retry)
            {
                // try again later, perhaps giving layout a chance to create the adorner layer
                adorner.Dispatcher.BeginInvoke(
                    DispatcherPriority.Loaded,
                    new DispatcherOperationCallback(ShowAdornerOperation),
                    new object[] { adorner });
            }
        }

        private static object ShowAdornerOperation(object arg)
        {
            var args = (object[])arg;
            var adorner = (Adorner)args[0];
            Show(adorner, retry: false);
            return null;
        }
    }
}
