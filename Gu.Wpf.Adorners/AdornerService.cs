namespace Gu.Wpf.Adorners
{
    using System.Windows.Documents;
    using System.Windows.Threading;

    public static class AdornerService
    {
        public static void Show(Adorner adorner)
        {
            Show(adorner, true);
        }

        public static void Remove(Adorner adorner)
        {
            //Debug.WriteLine(nameof(Remove));
            var adornerLayer = AdornerLayer.GetAdornerLayer(adorner.AdornedElement);
            adornerLayer?.Remove(adorner);
        }

        private static void Show(Adorner adorner, bool retry)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(adorner.AdornedElement);
            if (adornerLayer != null)
            {
                //Debug.WriteLine(nameof(Show));
                adornerLayer.Remove(adorner);
                adornerLayer.Add(adorner);
            }
            else if (retry)
            {
                // try again later, perhaps giving layout a chance to create the adorner layer
                adorner.Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                    new DispatcherOperationCallback(ShowAdornerOperation),
                    new object[] { adorner });
            }
        }

        private static object ShowAdornerOperation(object arg)
        {
            var args = (object[])arg;
            var adorner = (Adorner)args[0];
            Show(adorner, false);
            return null;
        }
    }
}
