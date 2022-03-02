namespace Gu.Wpf.Adorners
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Data;

    internal static class BindingHelper
    {
        private static readonly Dictionary<DependencyProperty, PropertyPath> PropertyPaths = new();

        internal static BindingBuilder Bind(
            this DependencyObject target,
            DependencyProperty targetProperty)
        {
            return new BindingBuilder(target, targetProperty);
        }

        private static PropertyPath GetPath(DependencyProperty property)
        {
            if (PropertyPaths.TryGetValue(property, out PropertyPath? path))
            {
                return path;
            }

            path = new PropertyPath(property);
            PropertyPaths[property] = path;
            return path;
        }

        internal struct BindingBuilder
        {
            private readonly DependencyObject target;
            private readonly DependencyProperty targetProperty;

            internal BindingBuilder(DependencyObject target, DependencyProperty targetProperty)
            {
                this.target = target;
                this.targetProperty = targetProperty;
            }

            internal BindingExpression OneWayTo(object source, DependencyProperty sourceProperty)
            {
                var sourcePath = GetPath(sourceProperty);
                return this.OneWayTo(source, sourcePath);
            }

            internal BindingExpression OneWayTo(object source, PropertyPath sourcePath)
            {
                var binding = new Binding
                {
                    Path = sourcePath,
                    Source = source,
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                };

                return (BindingExpression)BindingOperations.SetBinding(this.target, this.targetProperty, binding);
            }
        }
    }
}
