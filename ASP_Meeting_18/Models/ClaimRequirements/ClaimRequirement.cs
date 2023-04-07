using Microsoft.AspNetCore.Authorization;

namespace ASP_Meeting_18.Models.ClaimRequirements
{
    public class ClaimRequirement : IAuthorizationRequirement
    {
        public ClaimRequirement(params string[] claims)
        {
            Claims = claims;
        }

        public string[] Claims { get; }
    }
    public class ClaimHandler : AuthorizationHandler<ClaimRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimRequirement requirement)
        {
            if (context.User.HasClaim(c => requirement.Claims.Contains(c.Type) && c.Type.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }
            else if (context.User.HasClaim(c => requirement.Claims.Contains(c.Type) && c.Type.Equals("Manager", StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
