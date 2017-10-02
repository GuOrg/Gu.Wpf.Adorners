namespace Gu.Wpf.Adorners
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public static class DragAdorner
    {
        public static IDisposable Create(UIElement adornedElement, object content, DataTemplate contentTemplate, DataTemplateSelector contentTemplateSelector)
        {
            var adorner = new ContentDragAdorner(adornedElement)
            {
                Content = content,
                ContentTemplate = contentTemplate,
                ContentTemplateSelector = contentTemplateSelector,
            };

            AdornerService.Show(adorner);
            return adorner;
        }

        public static IDisposable Create(ContentPresenter adornedElement)
        {
            var adorner = new ContentDragAdorner(adornedElement)
                          {
                              Content = adornedElement.Content,
                              ContentTemplate = adornedElement.ContentTemplate,
                              ContentTemplateSelector = adornedElement.ContentTemplateSelector,
                          };

            AdornerService.Show(adorner);
            return adorner;
        }
    }
}