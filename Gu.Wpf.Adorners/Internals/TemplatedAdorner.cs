// ReSharper disable UnusedMember.Global
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
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
            TemplatedAdornerType = Type.GetType(fullName, throwOnError: true) ?? throw new InvalidOperationException("Could not find type: MS.Internal.Controls.TemplatedAdorner");
            Constructor = TemplatedAdornerType.GetConstructor(new[] { typeof(UIElement), typeof(ControlTemplate) }) ?? throw new InvalidOperationException("Could not find constructor for TemplatedAdorner");
            ClearChildMethod = TemplatedAdornerType.GetMethod("ClearChild") ?? throw new InvalidOperationException("Could not find method ClearChild");
            ReferenceElementProperty = TemplatedAdornerType.GetProperty("ReferenceElement") ?? throw new InvalidOperationException("Could not find property ReferenceElement");
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
            AssertTemplatedAdornerType(adorner);
            ClearChildMethod.Invoke(adorner, null);
        }

        internal static FrameworkElement GetTemplatedAdornerReferenceElement(this Adorner adorner)
        {
            AssertTemplatedAdornerType(adorner);
            return (FrameworkElement)ReferenceElementProperty.GetValue(adorner);
        }

        internal static void SetTemplatedAdornerReferenceElement(this Adorner adorner, FrameworkElement referenceElement)
        {
            AssertTemplatedAdornerType(adorner);
            ReferenceElementProperty.SetValue(adorner, referenceElement);
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private static void AssertTemplatedAdornerType(Adorner adorner, [CallerMemberName] string caller = null)
        {
            if (adorner?.GetType() != TemplatedAdornerType)
            {
                throw new InvalidOperationException($"{caller} can only be called on {TemplatedAdornerType}");
            }
        }
    }
}