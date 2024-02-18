using Casdoor.Client;
using System.Net.Http;
using System.Security.Cryptography;

namespace DesktopApp
{
    internal static class CasdoorVariables
    {
        public const string Domain = "https://door.casdoor.com";
        public const string AppName = "app-casnode";
        public const string ClientId = "014ae4bd048734ca2dea";
        public const string CallbackUrl = "casdoor://callback";
        public static CasdoorOptions options = new CasdoorOptions
        {
            Endpoint = Domain,
            OrganizationName = "casbin",
            ApplicationName = AppName,
            ApplicationType = "webapp", // webapp, webapi or native
            ClientId = ClientId,

            // Optional: The callback path that the client will be redirected to
            // after the user has authenticated. default is "/casdoor/signin-callback"
            CallbackPath = CallbackUrl,
            // Optional: Whether require https for casdoor endpoint
            RequireHttpsMetadata = false,
            // Optional: The scopes that the client is requesting.
            Scope = "profile"
        };
        public static CasdoorClient casdoorClient = new CasdoorClient(new HttpClient(), options);
    }
}
