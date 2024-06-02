using IdentityServer4.Models;
using System.Security.Cryptography;
using System.Text;

public static class Config
{
   

    public static IEnumerable<ApiResource> ApiResources =>
    new List<ApiResource>
    {
        new ApiResource("api1", "My API")
        {
            
            Scopes = { "read", "write", "update", "Delete" }
        }
    };


    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("read", "Read access to API"),
            new ApiScope("write", "Write access to API"),
            new ApiScope("update", "Update access to API"),
            new ApiScope("Delete", "Delete access to API")
        };

    
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };

    
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
                
                AllowedScopes = { "openid", "profile", "email", "read", "write", "update", "Delete" }
            },

            new Client
        {
            ClientId = "SportAPI",
            ClientName = "APIclint",
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            ClientSecrets = { new Secret("SportClient".Sha256()) },
            AllowedScopes = { "openid", "profile", "read", "write", "update", "Delete"},
            AccessTokenType = AccessTokenType.Jwt,
            AccessTokenLifetime = 3600,
            AllowOfflineAccess = true,
            UpdateAccessTokenClaimsOnRefresh = true
        },

        new Client
        {
            ClientId = "SportUI",
            ClientName = "APIclint",
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            ClientSecrets = { new Secret("DtuSport".Sha256()) },
            AllowedScopes = { "openid", "profile", "read", "write", "update", "Delete"},
            AccessTokenType = AccessTokenType.Jwt,
            AccessTokenLifetime = 3600,
            AllowOfflineAccess = true,
            UpdateAccessTokenClaimsOnRefresh = true,
            AllowedCorsOrigins = { "https://localhost:7033" }
        },
};}

