namespace Gu.Wpf.Adorners.Demo
{
    using System.Windows;
    using System.Windows.Controls;

    public class DragItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? EmptyTemplate { get; set; }

        public DataTemplate? ItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is DragItem)
            {
                return this.ItemTemplate ?? base.SelectTemplate(item, container);
            }

            return this.EmptyTemplate ?? base.SelectTemplate(item, container);
        }
    }
}
