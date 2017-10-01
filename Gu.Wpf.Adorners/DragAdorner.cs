namespace Gu.Wpf.Adorners
{
    using System;
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
            private readonly HwndSource source;
            private bool disposed;
            private TranslateTransform transform;

            public MouseTracker(Adorner adorner)
            {
                this.adorner = adorner;
                this.position = Mouse.GetPosition(adorner);
                this.source = HwndSource.FromHwnd(GetDesktopWindow());
                this.source?.AddHook(this.WndProc);
            }

            public void Dispose()
            {
                if (this.disposed)
                {
                    return;
                }

                this.disposed = true;
                this.source?.RemoveHook(this.WndProc);
            }

            [DllImport("user32.dll", SetLastError = false)]
            private static extern IntPtr GetDesktopWindow();

            private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
            {
                const int WM_MOUSEMOVE = 0x0200;

                switch (msg)
                {
                    case WM_MOUSEMOVE:
                        if (this.transform == null)
                        {
                            if (this.adorner.RenderTransform == null)
                            {
                                this.adorner.SetCurrentValue(UIElement.RenderTransformProperty, this.transform = new TranslateTransform());
                            }
                            else if (this.adorner.RenderTransform is TransformGroup transformGroup)
                            {
                                transformGroup.Children.Add(this.transform = new TranslateTransform());
                            }
                        }

                        if (this.transform != null)
                        {
                            var point = Mouse.GetPosition(this.adorner);
                            this.transform.SetCurrentValue(TranslateTransform.XProperty, point.X - point.X);
                            this.transform.SetCurrentValue(TranslateTransform.YProperty, point.Y - this.position.Y);
                        }

                        // MouseMove?.Invoke();
                        break;
                }

                return IntPtr.Zero;
            }
        }
    }
}