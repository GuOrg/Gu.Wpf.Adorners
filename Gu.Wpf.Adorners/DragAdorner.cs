namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public static class DragAdorner
    {
        public static IDisposable For(UIElement adornedElement, object content, DataTemplate contentTemplate, DataTemplateSelector contentTemplateSelector)
        {
            var adorner = new ContentDragAdorner(adornedElement)
            {
                Content = content,
                ContentTemplate = contentTemplate,
                ContentTemplateSelector = contentTemplateSelector,
                IsHitTestVisible = false,
            };

            AdornerService.Show(adorner);
            return adorner;
        }
    }
}