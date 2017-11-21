namespace Gu.Wpf.Adorners.Demo
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;

    public partial class DragAdornerTemplateView : UserControl
    {
        public DragAdornerTemplateView()
        {
            this.InitializeComponent();
        }

        private static bool TryGetDropTarget(object sender, out ContentPresenter target)
        {
            target = null;
            if (sender is ContentPresenter cp &&
                cp.Content == null)
            {
                target = cp;
            }

            return target != null;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ContentPresenter contentPresenter &&
                contentPresenter.Content != null)
            {
                var data = new DataObject(typeof(DragItem), contentPresenter.Content);
                using (var adorner = DragAdorner.Create(contentPresenter, contentPresenter.Content))
                {
                    data.SetData(typeof(Adorner), adorner);
                    contentPresenter.SetCurrentValue(ContentPresenter.ContentProperty, null);
                    DragDrop.DoDragDrop(contentPresenter, data, DragDropEffects.Move);
                    var target = data.GetData(typeof(UIElement));
                    if (target == null)
                    {
                        contentPresenter.SetCurrentValue(ContentPresenter.ContentProperty, data.GetData(typeof(DragItem)));
                    }
                }
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (TryGetDropTarget(e.Source, out var contentPresenter))
            {
                contentPresenter.SetCurrentValue(ContentPresenter.ContentProperty, e.Data.GetData(typeof(DragItem)));
                e.Effects = DragDropEffects.Move;
                e.Data.SetData(typeof(UIElement), contentPresenter);
                e.Handled = true;
            }
        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            if (TryGetDropTarget(e.Source, out var contentPresenter) &&
                e.Data.GetData(typeof(Adorner)) is ContentDragAdorner adorner)
            {
                adorner.RemoveSnap(contentPresenter);
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (TryGetDropTarget(e.Source, out var contentPresenter) &&
                e.Data.GetData(typeof(Adorner)) is ContentDragAdorner adorner)
            {
                adorner.SnapTo(contentPresenter);
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }
    }
}
