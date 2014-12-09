using System.Linq;
using Raven.Client.Indexes;
using Xemio.Logse.Server.Data.Entities;
using Xemio.Logse.Server.Data.Models;

namespace Xemio.Logse.Server.Raven.Transformers
{
    internal class ProjectToApiKeyModels : AbstractTransformerCreationTask<Project>
    {
        public ProjectToApiKeyModels()
        {
            TransformResults = projects =>
                from project in projects
                from apiKey in project.ApiKeys
                where 
                    string.IsNullOrWhiteSpace(ParameterOrDefault("key", string.Empty).Value<string>()) ||
                    apiKey.Key == ParameterOrDefault("key", string.Empty).Value<string>()
                select new ApiKeyModel
                {
                    Name = apiKey.Name,
                    Mode = apiKey.Mode,
                    Key = apiKey.Key,
                    IsDeactivated = apiKey.IsDeactivated
                };
        }

        public override string TransformerName
        {
            get { return "Project/ToApiKeyModels"; }
        }
    }
}