namespace Gu.Wpf.Adorners
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Media;

    public static class DragAdorner
    {
        public static IDisposable For(UIElement adornedElement, object content, DataTemplate contentTemplate, DataTemplateSelector contentTemplateSelector)
        {
            var adorner = new ContentAdorner(adornedElement)
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
            private readonly Adorner adorner;
            private readonly Point position;
            private readonly Window window;
            private readonly MouseEventHandler mouseMoveEventHandler;

            private bool disposed;
            private TranslateTransform transform;

            public MouseTracker(Adorner adorner)
            {
                this.adorner = adorner;
                this.position = Mouse.GetPosition(adorner);
                this.window = Window.GetWindow(adorner.AdornedElement);
                this.mouseMoveEventHandler = this.OnMouseMove;
                this.window.AddHandler(Mouse.MouseMoveEvent, this.mouseMoveEventHandler, handledEventsToo: true);
            }

            public void Dispose()
            {
                if (this.disposed)
                {
                    return;
                }

                this.disposed = true;
                Mouse.RemovePreviewMouseMoveHandler(this.window, this.mouseMoveEventHandler);
            }

            private void OnMouseMove(object sender, MouseEventArgs e)
            {
                Debug.WriteLine("OnMouseMove");
                if (this.transform == null)
                {
                    if (this.adorner.RenderTransform == null)
                    {
                        Debug.WriteLine("this.adorner.RenderTransform == null");
                        this.adorner.SetCurrentValue(UIElement.RenderTransformProperty, this.transform = new TranslateTransform());
                    }
                    else if (this.adorner.RenderTransform is TransformGroup transformGroup)
                    {
                        transformGroup.Children.Add(this.transform = new TranslateTransform());
                    }
                    else if(this.adorner.RenderTransform is MatrixTransform matrixTransform)
                    {
                        Debug.WriteLine("Update matrix pos");
                        var point = Mouse.GetPosition(this.adorner);
                        var matrix = matrixTransform.Matrix;
                        matrix.OffsetX = point.X - this.position.X;
                        matrix.OffsetY = point.Y - this.position.Y;
                        matrixTransform.SetCurrentValue(MatrixTransform.MatrixProperty, matrix);
                    }
                }

                if (this.transform != null)
                {
                    Debug.WriteLine("Update pos");
                    var point = Mouse.GetPosition(this.adorner);
                    this.transform.SetCurrentValue(TranslateTransform.XProperty, point.X - this.position.X);
                    this.transform.SetCurrentValue(TranslateTransform.YProperty, point.Y - this.position.Y);
                }
            }
        }
    }
}