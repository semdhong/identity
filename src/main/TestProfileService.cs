using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using works.ei8.IdentityAccess.Models;

namespace works.ei8.IdentityAccess
{
    internal class TestProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> _userManager;

        // https://stackoverflow.com/questions/44761058/how-to-add-custom-claims-to-access-token-in-identityserver4

        public TestProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //>Processing
            var user = await _userManager.GetUserAsync(context.Subject);

            var claims = new List<Claim>
            {
                // TODO: confirm if safe to include username in claims
                // new Claim("user", user.UserName)
            };

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            //>Processing
            var user = await _userManager.GetUserAsync(context.Subject);

            context.IsActive = true; // TODO: (user != null) && user.IsActive;
        }
    }
}