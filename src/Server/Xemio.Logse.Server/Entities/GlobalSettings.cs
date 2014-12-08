namespace Xemio.Logse.Server.Entities
{
    public class GlobalSettings : AggregateRoot
    {
        public GlobalSettings()
        {
            this.GlobalPassword = new Password();
        }

        public Password GlobalPassword { get; set; }
    }
}