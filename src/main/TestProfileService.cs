using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace works.ei8.IdentityAccess
{
    internal class TestProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("example_owner", "Joe Tester"));
            claims.Add(new Claim("CustomClaim2", "SomeValue"));
            context.IssuedClaims = claims;
            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.FromResult(0);
        }
    }
}