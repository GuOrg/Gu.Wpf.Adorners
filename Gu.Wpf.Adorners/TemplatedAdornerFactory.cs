namespace Gu.Wpf.Adorners
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Media;

    internal static class TemplatedAdornerFactory
    {
        private static readonly FrameworkElement ProxyElement;
        public static readonly DependencyProperty ProxyPropertyProperty = DependencyProperty.RegisterAttached(
            "ProxyProperty",
            typeof(object),
            typeof(TemplatedAdornerFactory),
            new PropertyMetadata(null));
        private static readonly BindingExpression BindingExpression;

        static TemplatedAdornerFactory()
        {
            ProxyElement = new FrameworkElement();
            BindingExpression = ProxyElement.Bind(ProxyPropertyProperty)
                                            .OneWayTo(ProxyElement, FrameworkElement.DataContextProperty);
        }

        public static Adorner Create(UIElement adornedElement, ControlTemplate template)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            if (adornerLayer == null)
            {
                return null;
            }

            var errorTemplate = Validation.GetErrorTemplate(adornedElement);
            Validation.SetErrorTemplate(adornedElement, null);
            Validation.SetErrorTemplate(ProxyElement, template);
            Validation.SetValidationAdornerSite(ProxyElement, adornedElement);
            Validation.MarkInvalid(BindingExpression, new ValidationError(new DataErrorValidationRule(), BindingExpression.ParentBinding));

            var adorners = adornerLayer.GetAdorners(adornedElement);
            var adorner = adorners.LastOrDefault();
            var child = VisualTreeHelper.GetChild(adorner, 0);

            Validation.SetErrorTemplate(adornedElement, errorTemplate);
            Validation.SetErrorTemplate(ProxyElement, null);
            Validation.ClearInvalid(BindingExpression);
            return adorner;
        }
    }
}