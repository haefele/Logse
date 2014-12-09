using System.Linq;

namespace Xemio.Logse.Server.Extensions
{
    public static class StringExtensions
    {
        public static int GetIntId(this string id)
        {
            return int.Parse(id.Split('/').Last());
        }
    }
}