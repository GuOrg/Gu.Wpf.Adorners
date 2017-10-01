namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public static class DragAdorner
    {
        public static IDisposable For(UIElement adornedElement, object content, DataTemplate contentTemplate, DataTemplateSelector contentTemplateSelector)
        {
            var adorner = new ContentDragAdorner(adornedElement, new TranslateTransform())
            {
                Content = content,
                ContentTemplate = contentTemplate,
                ContentTemplateSelector = contentTemplateSelector,
                IsHitTestVisible = false,
            };

            AdornerService.Show(adorner);
            var tracker = new MouseTracker(adorner);

            return new ActionDisposable(() =>
            {
                tracker.Dispose();
                AdornerService.Remove(adorner);
            });
        }

        private sealed class MouseTracker : IDisposable
        {
            private readonly ContentDragAdorner adorner;
            private readonly Point position;

            private bool disposed;

            public MouseTracker(ContentDragAdorner adorner)
            {
                this.adorner = adorner;
                this.position = Mouse.GetPosition(adorner);
                DragDrop.AddPreviewQueryContinueDragHandler(adorner.AdornedElement, this.Update);
            }

            public void Dispose()
            {
                if (this.disposed)
                {
                    return;
                }

                this.disposed = true;
                DragDrop.RemovePreviewQueryContinueDragHandler(this.adorner.AdornedElement, this.Update);
            }

            private void Update(object sender, QueryContinueDragEventArgs e)
            {
                var point = Mouse.GetPosition(this.adorner);
                this.adorner.Offset.SetCurrentValue(TranslateTransform.XProperty, this.position.X - point.X);
                this.adorner.Offset.SetCurrentValue(TranslateTransform.YProperty, this.position.Y - point.Y);
                System.Diagnostics.Debug.WriteLine($"Update pos: {this.adorner.Offset.X}, {this.adorner.Offset.Y}");
            }
        }
    }
}