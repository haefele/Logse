namespace Xemio.Logse.Server.Data.Entities
{
    internal class ApiKey : AggregateRoot
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public ApiKeyMode Mode { get; set; }
        public bool IsDeactivated { get; set; }
    }
}