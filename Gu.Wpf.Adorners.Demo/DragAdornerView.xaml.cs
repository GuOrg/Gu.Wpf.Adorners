namespace Gu.Wpf.Adorners.Demo
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class DragAdornerView : UserControl
    {
        public DragAdornerView()
        {
            this.InitializeComponent();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ContentPresenter contentPresenter &&
                contentPresenter.Content != null)
            {
                var data = new DataObject(typeof(DragItem), contentPresenter.Content);
                using (DragAdorner.For(
                    contentPresenter,
                    contentPresenter.Content,
                    (DataTemplate)this.FindResource("AdornerDragItemTemplate"),
                    null))
                {
                    DragDrop.DoDragDrop(contentPresenter, data, DragDropEffects.Move);
                    var target = data.GetData(typeof(UIElement));
                    if (target != null &&
                        !ReferenceEquals(target, contentPresenter))
                    {
                        contentPresenter.SetCurrentValue(ContentPresenter.ContentProperty, null);
                    }
                }
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Source is ContentPresenter contentPresenter &&
                contentPresenter.Content == null)
            {
                contentPresenter.SetCurrentValue(ContentPresenter.ContentProperty, e.Data.GetData(typeof(DragItem)));
                e.Effects = DragDropEffects.Move;
                e.Data.SetData(typeof(UIElement), contentPresenter);
                e.Handled = true;
            }
        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
        }
    }
}
