namespace Xemio.Logse.Server.Data.Models
{
    public class ApiKeyModel
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public ApiKeyMode Mode { get; set; }
        public bool IsDeactivated { get; set; }
    }
}