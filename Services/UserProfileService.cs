using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dtu_Sport_IDS.Services
{
    public class UserProfileService : IProfileService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserProfileService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.FindByIdAsync(context.Subject.GetSubjectId());
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleClaims = new List<Claim>();
                foreach (var role in roles)
                {
                    roleClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                context.IssuedClaims.AddRange(roleClaims);
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.FindByIdAsync(context.Subject.GetSubjectId());
            context.IsActive = (user != null);
        }
    }
}
