using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace works.ei8.IdentityAccess
{
    public class Constants
    {
        public struct Paths
        {
            public const string Logout = "/Account/Redirecting";
        }
        public struct EnvironmentVariableKeys
        {
            public const string ClientsXamarin = "CLIENTS_XAMARIN";
            public const string IssuerUri = "ISSUER_URI";
            public const string ConnectionStringsDefault = "CONNECTION_STRINGS_DEFAULT";
        }
    }
}
