using System.Linq;
using Raven.Client.Indexes;
using Xemio.Logse.Server.Data.Entities;
using Xemio.Logse.Server.Data.Models;

namespace Xemio.Logse.Server.Raven.Transformers
{
    internal class ProjectToProjectModel : AbstractTransformerCreationTask<Project>
    {
        public ProjectToProjectModel()
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

        public override string TransformerName
        {
            get { return "Project/ToProjectModel"; }
        }
    }
}