namespace Zord.DomainActiveDirectory.Configurations
{
    public class ActiveDirectoryConfiguration
    {
        public string Name { get; set; } = default!;

        public LdapConfiguration? LDAP { get; set; }
    }
}