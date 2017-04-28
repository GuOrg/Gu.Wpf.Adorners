namespace Gu.Wpf.Adorners
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    internal static class TemplatedAdorner
    {
        private static readonly ConstructorInfo Constructor;
        private static readonly MethodInfo ClearChildMethod;
        private static readonly Type TemplatedAdornerType;
        private static readonly PropertyInfo ReferenceElementProperty;

        static TemplatedAdorner()
        {
            var fullName = "MS.Internal.Controls.TemplatedAdorner, " + typeof(AdornedElementPlaceholder).Assembly.FullName;
            TemplatedAdornerType = Type.GetType(fullName, throwOnError: true);
            Constructor = TemplatedAdornerType.GetConstructor(new[] { typeof(UIElement), typeof(ControlTemplate) });
            ClearChildMethod = TemplatedAdornerType.GetMethod("ClearChild");
            ReferenceElementProperty = TemplatedAdornerType.GetProperty("ReferenceElement");
        }

        internal static Adorner Create(UIElement element, ControlTemplate template)
        {
            var adorner = (Adorner)Constructor.Invoke(new object[] { element, template });
            adorner.Bind(FrameworkElement.DataContextProperty)
                   .OneWayTo(element, FrameworkElement.DataContextProperty);
            return adorner;
        }

        internal static void ClearTemplatedAdornerChild(this Adorner adorner)
        {
            AssertTemplatedAdornerTypee(adorner);
            ClearChildMethod.Invoke(adorner, null);
        }

        internal static FrameworkElement GetTemplatedAdornerReferenceElement(this Adorner adorner)
        {
            AssertTemplatedAdornerTypee(adorner);
            return (FrameworkElement)ReferenceElementProperty.GetValue(adorner);
        }

        internal static void SetTemplatedAdornerReferenceElement(this Adorner adorner, FrameworkElement referenceElement)
        {
            AssertTemplatedAdornerTypee(adorner);
            ReferenceElementProperty.SetValue(adorner, referenceElement);
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private static void AssertTemplatedAdornerTypee(Adorner adorner, [CallerMemberName] string caller = null)
        {
            if (adorner?.GetType() != TemplatedAdornerType)
            {
                throw new InvalidOperationException($"{caller} can only be called on {TemplatedAdornerType}");
            }
        }
    }
}