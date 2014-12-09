namespace Xemio.Logse.Server.Data.Entities
{
    public class GlobalSettings : AggregateRoot
    {
        public static string GlobalId = "GlobalSettings/Global";

        public GlobalSettings()
        {
            this.Id = GlobalId;
            this.GlobalPassword = new Password();
        }

        public Password GlobalPassword { get; set; }
    }
}