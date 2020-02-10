namespace Gu.Wpf.Adorners.Demo
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class DragAdornerTemplateView : UserControl
    {
        public DragAdornerTemplateView()
        {
            this.InitializeComponent();
        }

        private static bool TryGetDropTarget(object sender, [NotNullWhen(true)] out ContentPresenter? target)
        {
            target = null;
            if (sender is ContentPresenter cp &&
                cp.Content is null)
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
                using var adorner = DragAdorner.Create(contentPresenter, contentPresenter.Content);
                data.SetData(adorner);
                contentPresenter.SetCurrentValue(ContentPresenter.ContentProperty, null);
                DragDrop.DoDragDrop(contentPresenter, data, DragDropEffects.Move);
                if (!data.TryGetData<ContentPresenter>(out _) &&
                    data.TryGetData<DragItem>(out var item))
                {
                    contentPresenter.SetCurrentValue(ContentPresenter.ContentProperty, item);
                }
            }
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (TryGetDropTarget(e.Source, out var contentPresenter) &&
                e.Data.TryGetData<ContentDragAdorner>(out var adorner))
            {
                adorner.SnapTo(contentPresenter);
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            if (TryGetDropTarget(e.Source, out var contentPresenter) &&
                e.Data.TryGetData<ContentDragAdorner>(out var adorner))
            {
                adorner.RemoveSnap(contentPresenter);
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (TryGetDropTarget(e.Source, out var contentPresenter) &&
                e.Data.TryGetData<DragItem>(out var item))
            {
                contentPresenter.SetCurrentValue(ContentPresenter.ContentProperty, item);
                e.Effects = DragDropEffects.Move;
                e.Data.SetData(contentPresenter);
                e.Handled = true;
            }
        }
    }
}
