namespace Gu.Wpf.Adorners
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Helper for showing drag adorners.
    /// </summary>
    public static class DragAdorner
    {
        /// <summary>
        /// Create a <see cref="ContentDragAdorner"/> that renders a <see cref="ContentPresenter"/>.
        /// The adorner is added to the adorner layer before it is returned.
        /// </summary>
        /// <param name="adornedElement">The drag source.</param>
        /// <param name="content">The content to drag.</param>
        /// <param name="contentTemplate">The template for the dragged content.</param>
        /// <param name="contentTemplateSelector">The template selector for the dragged content.</param>
        /// <returns>
        /// A <see cref="ContentDragAdorner"/> that should be disposed when the drag operation ends.
        /// Disposing it removes subscriptions and removes the adorner.
        /// </returns>
        public static ContentDragAdorner Create(UIElement adornedElement, object content, DataTemplate contentTemplate, DataTemplateSelector contentTemplateSelector)
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

        /// <summary>
        /// Create a <see cref="ContentDragAdorner"/> that renders a <see cref="ContentPresenter"/> with Content, ContentTemplate and ContentTemplateSelector from <paramref name="adornedElement"/>
        /// </summary>
        /// <param name="adornedElement">The drag source.</param>
        /// <returns>
        /// A <see cref="ContentDragAdorner"/> that should be disposed when the drag operation ends.
        /// Disposing it removes subscriptions and removes the adorner.
        /// </returns>
        public static ContentDragAdorner Create(ContentPresenter adornedElement)
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

        /// <summary>
        /// Create a <see cref="VisualBrushDragAdorner"/> that renders a rectangle with fills set to a visual brush of the adorned element.
        /// </summary>
        /// <param name="adornedElement">The drag source.</param>
        /// <returns>
        /// A <see cref="VisualBrushDragAdorner"/> that should be disposed when the drag operation ends.
        /// Disposing it removes subscriptions and removes the adorner.
        /// </returns>
        public static VisualBrushDragAdorner CreateVisualBrushAdorner(FrameworkElement adornedElement)
        {
            var adorner = new VisualBrushDragAdorner(adornedElement);
            AdornerService.Show(adorner);
            return adorner;
        }
    }
}