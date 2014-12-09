using Raven.Client;

namespace Xemio.Logse.Server.Extensions
{
    public static class AsyncAdvancedSessionOperationsExtensions
    {
        /// <summary>
        /// Gets the string id for the given type.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="advanced">The advanced session operations.</param>
        /// <param name="id">The id.</param>
        public static string GetStringIdFor<T>(this IAsyncAdvancedSessionOperations advanced, object id)
        {
            return advanced.DocumentStore.Conventions.FindFullDocumentKeyFromNonStringIdentifier(id, typeof(T), false);
        }
    }
}