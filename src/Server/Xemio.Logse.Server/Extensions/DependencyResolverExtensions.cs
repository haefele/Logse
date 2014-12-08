using System.Web.Http.Dependencies;

namespace Xemio.Logse.Server.Extensions
{
    public static class DependencyResolverExtensions
    {
        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="resolver">The resolver.</param>
        public static T GetService<T>(this IDependencyResolver resolver)
        {
            return (T)resolver.GetService(typeof (T));
        }
    }
}