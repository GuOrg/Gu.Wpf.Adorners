// ReSharper disable UnusedMember.Global
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
namespace Gu.Wpf.Adorners;

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

internal static class TemplatedAdorner
{
    private static readonly Type TemplatedAdornerType = typeof(AdornedElementPlaceholder).Assembly.GetType("MS.Internal.Controls.TemplatedAdorner", throwOnError: true)!;
    private static readonly ConstructorInfo Constructor = TemplatedAdornerType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(UIElement), typeof(ControlTemplate) }, null) ?? throw new InvalidOperationException("Could not find constructor for TemplatedAdorner");
    private static readonly MethodInfo ClearChildMethod = TemplatedAdornerType.GetMethod("ClearChild", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, Type.EmptyTypes, null) ?? throw new InvalidOperationException("Could not find method ClearChild");

    internal static Adorner Create(UIElement element, ControlTemplate template)
    {
        var adorner = (Adorner)Constructor.Invoke(new object[] { element, template });
        _ = adorner.Bind(FrameworkElement.DataContextProperty)
                   .OneWayTo(element, FrameworkElement.DataContextProperty);
        return adorner;
    }

    internal static void ClearTemplatedAdornerChild(this Adorner adorner)
    {
        AssertTemplatedAdornerType(adorner);
        _ = ClearChildMethod.Invoke(adorner, null);
    }

    private static void AssertTemplatedAdornerType(Adorner adorner, [CallerMemberName] string? caller = null)
    {
        if (adorner?.GetType() != TemplatedAdornerType)
        {
            throw new InvalidOperationException($"{caller} can only be called on {TemplatedAdornerType}");
        }
    }
}
