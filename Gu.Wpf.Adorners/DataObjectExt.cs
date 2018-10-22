namespace Gu.Wpf.Adorners
{
    using System.Windows;

    /// <summary>
    /// Extension methods for <see cref="IDataObject"/>.
    /// </summary>
    public static class DataObjectExt
    {
        /// <summary>
        /// Calls return (T)dataObject.GetData(typeof(T));.
        /// </summary>
        /// <typeparam name="T">The type of the key and the data.</typeparam>
        /// <param name="dataObject">The <see cref="IDataObject"/>.</param>
        /// <returns>The value returned by <see cref="IDataObject.GetData(System.Type)"/>.</returns>
        public static T GetData<T>(this IDataObject dataObject)
        {
            return (T)dataObject.GetData(typeof(T));
        }

        /// <summary>
        /// Calls return (T)dataObject.GetData(typeof(T));.
        /// </summary>
        /// <typeparam name="T">The type of the key and the data.</typeparam>
        /// <param name="dataObject">The <see cref="IDataObject"/>.</param>
        /// <param name="data">The data keyed by <typeparamref name="T"/>.</param>
        /// <returns>True if <see cref="IDataObject.GetData(System.Type)"/> returns not null.</returns>
        public static bool TryGetData<T>(this IDataObject dataObject, out T data)
        {
            data = (T)dataObject.GetData(typeof(T));
            return data != null;
        }

        /// <summary>
        /// Calls dataObject.SetData(typeof(T), data);.
        /// </summary>
        /// <typeparam name="T">The type to key the data with.</typeparam>
        /// <param name="dataObject">The <see cref="IDataObject"/>.</param>
        /// <param name="data">The value to set.</param>
        public static void SetData<T>(this IDataObject dataObject, T data)
        {
            dataObject.SetData(typeof(T), data);
        }
    }
}
