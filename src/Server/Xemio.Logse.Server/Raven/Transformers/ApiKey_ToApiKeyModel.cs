using System.Linq;
using Raven.Client.Indexes;
using Xemio.Logse.Server.Data.Entities;
using Xemio.Logse.Server.Data.Models;

namespace Xemio.Logse.Server.Raven.Transformers
{
    // ReSharper disable once InconsistentNaming
    internal class ApiKey_ToApiKeyModel : AbstractTransformerCreationTask<ApiKey>
    {
        public ApiKey_ToApiKeyModel()
        {
            this.TransformResults = apiKeys =>
                from apiKey in apiKeys
                select new ApiKeyModel
                {
                    Key = apiKey.Key,
                    IsDeactivated = apiKey.IsDeactivated,
                    Name = apiKey.Name,
                    Mode = apiKey.Mode,
                    ProjectId = int.Parse(apiKey.ProjectId.Split('/').Last())
                };
        }
    }
}