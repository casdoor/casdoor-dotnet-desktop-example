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
        public const string OrganizationName = "casbin";
        public const string ApplicationType = "webapp";
        public const string Scope = "profile";
        public const bool RequireHttpsMetadata = false;
        public static CasdoorOptions options = new CasdoorOptions
        {
            Endpoint = Domain,
            OrganizationName = OrganizationName,
            ApplicationName = AppName,
            ApplicationType = ApplicationType,
            ClientId = ClientId,
            CallbackPath = CallbackUrl,
            RequireHttpsMetadata = RequireHttpsMetadata,
            Scope = Scope
        };
        public static CasdoorClient casdoorClient = new CasdoorClient(new HttpClient(), options);
    }
}
