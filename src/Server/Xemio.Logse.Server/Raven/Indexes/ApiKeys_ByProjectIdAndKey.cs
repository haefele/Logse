using System.Linq;
using Raven.Client.Indexes;
using Xemio.Logse.Server.Data.Entities;

namespace Xemio.Logse.Server.Raven.Indexes
{
    // ReSharper disable once InconsistentNaming
    internal class ApiKeys_ByProjectIdAndKey : AbstractIndexCreationTask<ApiKey>
    {
        public ApiKeys_ByProjectIdAndKey()
        {
            this.Map = apiKeys =>
                from apiKey in apiKeys
                select new 
                {
                    apiKey.ProjectId,
                    apiKey.Key
                };
        }
    }
}