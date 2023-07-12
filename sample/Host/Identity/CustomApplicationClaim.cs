using Host.Identity.Authorization;
using Zord.Identity;

namespace Host.Identity
{
    public class CustomApplicationClaim
    {
        public string Permission => AppClaimType.Permission;

        public string UserId => AppClaimType.UserId;

        public string UserName => AppClaimType.UserName;

        public string FirstName => AppClaimType.FirstName;

        public string LastName => AppClaimType.LastName;

        public string FullName => AppClaimType.FullName;

        public string PhoneNumber => AppClaimType.PhoneNumber;

        public string Email => AppClaimType.Email;

        public string Role => AppClaimType.Role;

        public string ImageUrl => nameof(ImageUrl);

        public string Expiration => AppClaimType.Expiration;

        public string AccessToken => AppClaimType.AccessToken;

        public string NewId() => Guid.NewGuid().ToString();

        public Task<IEnumerable<ClaimDto>> GetAllClaims()
        {
            var claims = ClaimsExtensions.AppClaims;
            var dto = claims.Select(s => new ClaimDto
            {
                Type = s.Type,
                Value = s.Value,
            });

            return Task.FromResult(dto);
        }

        public IEnumerable<string> MasterUsers => DefaultUser.MASTER_USERS;
    }
}
