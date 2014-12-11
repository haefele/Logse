using System.Linq;
using Raven.Client.Indexes;
using Xemio.Logse.Server.Data.Entities;
using Xemio.Logse.Server.Data.Models;

namespace Xemio.Logse.Server.Raven.Transformers
{
    // ReSharper disable once InconsistentNaming
    internal class Project_ToProjectModel : AbstractTransformerCreationTask<Project>
    {
        public Project_ToProjectModel()
        {
            TransformResults = projects =>
                from project in projects
                select new ProjectModel
                {
                    Id = int.Parse(project.Id.Split('/').Last()),
                    IsDeactivated = project.IsDeactivated,
                    Name = project.Name
                };
        }
    }
}