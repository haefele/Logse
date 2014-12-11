namespace Xemio.Logse.Server.Data.Models
{
    public class ApiKeyModel
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public ApiKeyMode Mode { get; set; }
        public bool IsDeactivated { get; set; }
    }
}