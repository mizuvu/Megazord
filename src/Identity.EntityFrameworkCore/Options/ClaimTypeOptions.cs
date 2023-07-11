namespace Zord.Identity.EntityFrameworkCore.Options;

public class ClaimTypeOptions
{
    public string Permission { get; set; } = "permission";

    public string UserId { get; set; } = "uid";

    public string UserName { get; set; } = "name";

    public string FirstName { get; set; } = "first_name";

    public string LastName { get; set; } = "last_name";

    public string FullName { get; set; } = "full_name";

    public string PhoneNumber { get; set; } = "phone_number";

    public string Email { get; set; } = "email";

    public string Role { get; set; } = "role";

    public string ImageUrl { get; set; } = "image_url";

    public string Expiration { get; set; } = "exp";

    public string AccessToken { get; set; } = "token";
}
