using System.Collections.Generic;

namespace Xemio.Logse.Server.Data.Entities
{
    internal class Project : AggregateRoot
    {
        public Project()
        {
            this.ApiKeys = new List<ApiKey>();
        }

        public string Name { get; set; }
        public List<ApiKey> ApiKeys { get; set; }
        public bool IsDeactivated { get; set; }
    }
}