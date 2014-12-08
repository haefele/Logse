using System.Collections.Generic;

namespace Xemio.Logse.Server.Entities
{
    internal class Project : AggregateRoot
    {
        public string Name { get; set; }
        public List<ApiKey> ApiKeys { get; set; }
        public bool IsDeactivated { get; set; }
    }
}