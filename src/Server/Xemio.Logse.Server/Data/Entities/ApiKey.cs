namespace Xemio.Logse.Server.Data.Entities
{
    internal class ApiKey
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public ApiKeyMode Mode { get; set; }
        public bool IsDeactivated { get; set; }
    }
}