using System.Collections.Generic;

namespace Xemio.Logse.Server.Data.Entities
{
    internal class Project : AggregateRoot
    {
        public string Name { get; set; }
        public bool IsDeactivated { get; set; }
    }
}