namespace Zord.DomainActiveDirectory.Configurations
{
    public class LdapConfiguration
    {
        public string IpServer { get; set; } = default!;
        public int Port { get; set; }
        public string Connection { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string DomainUser { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
