namespace Gu.Wpf.Adorners;

using System;
using System.Windows;

/// <summary>
/// ~Inspired~ by: http://referencesource.microsoft.com/#WindowsBase/Shared/MS/Internal/DoubleUtil.cs,ef2a956bcf846761.
/// </summary>
internal static class DoubleUtil
{
    // Const values come from sdk\inc\crt\float.h
    internal const double DblEpsilon = 2.2204460492503131e-016; /* smallest such that 1.0+DBL_EPSILON != 1.0 */
    internal const float FltMin = 1.175494351e-38F; /* Number close to zero, where float.MinValue is -float.MaxValue */

    /// <summary>
    /// AreClose - Returns whether or not two doubles are "close".  That is, whether or
    /// not they are within epsilon of each other.  Note that this epsilon is proportional
    /// to the numbers themselves to that AreClose survives scalar multiplication.
    /// There are plenty of ways for this to return false even for numbers which
    /// are theoretically identical, so no code calling this should fail to work if this
    /// returns false.  This is important enough to repeat:
    /// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
    /// used for optimizations *only*.
    /// </summary>
    /// <returns>
    /// bool - the result of the AreClose comparison.
    /// </returns>
    /// <param name="value1"> The first double to compare. </param>
    /// <param name="value2"> The second double to compare. </param>
    internal static bool AreClose(double value1, double value2)
    {
        // in case they are Infinities (then epsilon check does not work)
        if (value1 == value2)
        {
            return true;
        }

        // This computes (|value1-value2| / (|value1| + |value2| + 10.0)) < DBL_EPSILON
        double eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * DblEpsilon;
        double delta = value1 - value2;
        return (-eps < delta) && (eps > delta);
    }

    /// <summary>
    /// LessThan - Returns whether or not the first double is less than the second double.
    /// That is, whether or not the first is strictly less than *and* not within epsilon of
    /// the other number.  Note that this epsilon is proportional to the numbers themselves
    /// to that AreClose survives scalar multiplication.  Note,
    /// There are plenty of ways for this to return false even for numbers which
    /// are theoretically identical, so no code calling this should fail to work if this
    /// returns false.  This is important enough to repeat:
    /// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
    /// used for optimizations *only*.
    /// </summary>
    /// <returns>
    /// bool - the result of the LessThan comparison.
    /// </returns>
    /// <param name="value1"> The first double to compare. </param>
    /// <param name="value2"> The second double to compare. </param>
    internal static bool LessThan(double value1, double value2)
    {
        return (value1 < value2) && !AreClose(value1, value2);
    }

    /// <summary>
    /// GreaterThan - Returns whether or not the first double is greater than the second double.
    /// That is, whether or not the first is strictly greater than *and* not within epsilon of
    /// the other number.  Note that this epsilon is proportional to the numbers themselves
    /// to that AreClose survives scalar multiplication.  Note,
    /// There are plenty of ways for this to return false even for numbers which
    /// are theoretically identical, so no code calling this should fail to work if this
    /// returns false.  This is important enough to repeat:
    /// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
    /// used for optimizations *only*.
    /// </summary>
    /// <returns>
    /// bool - the result of the GreaterThan comparison.
    /// </returns>
    /// <param name="value1"> The first double to compare. </param>
    /// <param name="value2"> The second double to compare. </param>
    internal static bool GreaterThan(double value1, double value2)
    {
        return (value1 > value2) && !AreClose(value1, value2);
    }

    /// <summary>
    /// LessThanOrClose - Returns whether or not the first double is less than or close to
    /// the second double.  That is, whether or not the first is strictly less than or within
    /// epsilon of the other number.  Note that this epsilon is proportional to the numbers
    /// themselves to that AreClose survives scalar multiplication.  Note,
    /// There are plenty of ways for this to return false even for numbers which
    /// are theoretically identical, so no code calling this should fail to work if this
    /// returns false.  This is important enough to repeat:
    /// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
    /// used for optimizations *only*.
    /// </summary>
    /// <returns>
    /// bool - the result of the LessThanOrClose comparison.
    /// </returns>
    /// <param name="value1"> The first double to compare. </param>
    /// <param name="value2"> The second double to compare. </param>
    internal static bool LessThanOrClose(double value1, double value2)
    {
        return (value1 < value2) || AreClose(value1, value2);
    }

    /// <summary>
    /// GreaterThanOrClose - Returns whether or not the first double is greater than or close to
    /// the second double.  That is, whether or not the first is strictly greater than or within
    /// epsilon of the other number.  Note that this epsilon is proportional to the numbers
    /// themselves to that AreClose survives scalar multiplication.  Note,
    /// There are plenty of ways for this to return false even for numbers which
    /// are theoretically identical, so no code calling this should fail to work if this
    /// returns false.  This is important enough to repeat:
    /// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
    /// used for optimizations *only*.
    /// </summary>
    /// <returns>
    /// bool - the result of the GreaterThanOrClose comparison.
    /// </returns>
    /// <param name="value1"> The first double to compare. </param>
    /// <param name="value2"> The second double to compare. </param>
    internal static bool GreaterThanOrClose(double value1, double value2)
    {
        return (value1 > value2) || AreClose(value1, value2);
    }

    /// <summary>
    /// IsOne - Returns whether or not the double is "close" to 1.  Same as AreClose(double, 1),
    /// but this is faster.
    /// </summary>
    /// <returns>
    /// bool - the result of the AreClose comparison.
    /// </returns>
    /// <param name="value"> The double to compare to 1. </param>
    internal static bool IsOne(double value)
    {
        return Math.Abs(value - 1.0) < 10.0 * DblEpsilon;
    }

    /// <summary>
    /// IsZero - Returns whether or not the double is "close" to 0.  Same as AreClose(double, 0),
    /// but this is faster.
    /// </summary>
    /// <returns>
    /// bool - the result of the AreClose comparison.
    /// </returns>
    /// <param name="value"> The double to compare to 0. </param>
    internal static bool IsZero(double value)
    {
        return Math.Abs(value) < 10.0 * DblEpsilon;
    }

    // The Point, Size, Rect and Matrix class have moved to WinCorLib.  However, we provide
    // internal AreClose methods for our own use here.

    /// <summary>
    /// Compares two points for fuzzy equality.  This function
    /// helps compensate for the fact that double values can
    /// acquire error when operated upon.
    /// </summary>
    /// <param name='point1'>The first point to compare.</param>
    /// <param name='point2'>The second point to compare.</param>
    /// <returns>Whether or not the two points are equal.</returns>
    internal static bool AreClose(Point point1, Point point2)
    {
        return AreClose(point1.X, point2.X) &&
               AreClose(point1.Y, point2.Y);
    }

    /// <summary>
    /// Compares two Size instances for fuzzy equality.  This function
    /// helps compensate for the fact that double values can
    /// acquire error when operated upon.
    /// </summary>
    /// <param name='size1'>The first size to compare.</param>
    /// <param name='size2'>The second size to compare.</param>
    /// <returns>Whether or not the two Size instances are equal.</returns>
    internal static bool AreClose(Size size1, Size size2)
    {
        return AreClose(size1.Width, size2.Width) &&
               AreClose(size1.Height, size2.Height);
    }

    /// <summary>
    /// Compares two Vector instances for fuzzy equality.  This function
    /// helps compensate for the fact that double values can
    /// acquire error when operated upon.
    /// </summary>
    /// <param name='vector1'>The first Vector to compare.</param>
    /// <param name='vector2'>The second Vector to compare.</param>
    /// <returns>Whether or not the two Vector instances are equal.</returns>
    internal static bool AreClose(Vector vector1, Vector vector2)
    {
        return AreClose(vector1.X, vector2.X) &&
               AreClose(vector1.Y, vector2.Y);
    }

    /// <summary>
    /// Compares two rectangles for fuzzy equality.  This function
    /// helps compensate for the fact that double values can
    /// acquire error when operated upon.
    /// </summary>
    /// <param name='rect1'>The first rectangle to compare.</param>
    /// <param name='rect2'>The second rectangle to compare.</param>
    /// <returns>Whether or not the two rectangles are equal.</returns>
    internal static bool AreClose(Rect rect1, Rect rect2)
    {
        // If they're both empty, don't bother with the double logic.
        if (rect1.IsEmpty)
        {
            return rect2.IsEmpty;
        }

        // At this point, rect1 isn't empty, so the first thing we can test is
        // rect2.IsEmpty, followed by property-wise compares.
        return !rect2.IsEmpty &&
               AreClose(rect1.X, rect2.X) &&
               AreClose(rect1.Y, rect2.Y) &&
               AreClose(rect1.Height, rect2.Height) &&
               AreClose(rect1.Width, rect2.Width);
    }

    internal static bool IsBetweenZeroAndOne(double val) => GreaterThanOrClose(val, 0) && LessThanOrClose(val, 1);

    internal static int DoubleToInt(double val) => val > 0 ? (int)(val + 0.5) : (int)(val - 0.5);

    /// <summary>
    /// rectHasNaN - this returns true if this rect has X, Y , Height or Width as NaN.
    /// </summary>
    /// <param name='r'>The rectangle to test.</param>
    /// <returns>returns whether the Rect has NaN.</returns>
    internal static bool RectHasNaN(Rect r)
    {
        return double.IsNaN(r.X) ||
               double.IsNaN(r.Y) ||
               double.IsNaN(r.Height) ||
               double.IsNaN(r.Width);
    }
}
