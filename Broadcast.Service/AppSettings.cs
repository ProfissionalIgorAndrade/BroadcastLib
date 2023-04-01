namespace Broadcast.Service
{
    public class AppSettings
    {
        public Configurations Configurations { get; set; }
    }

    public record Configurations
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public string Port { get; set; }
    }
}