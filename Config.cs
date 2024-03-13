using IdentityServer4.Models;
using System.Security.Cryptography;
using System.Text;

public static class Config
{
    // Define the API scopes your server supports
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("read", "Read access to API"),
            new ApiScope("write", "Write access to API"),
            new ApiScope("update", "Update access to API"),
            new ApiScope("Delete", "Delete access to API")
        };

    // Define the identity resources (standard OpenID Connect scopes)
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };

    // Configure clients that are allowed to access the server
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "postman",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                // Here we combine the OpenID Connect scopes with the custom API scopes
                AllowedScopes = { "openid", "profile", "email", "read", "write", "update", "Delete" }
            }
        };
}

