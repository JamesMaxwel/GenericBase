
using Microsoft.AspNetCore.Authorization;

namespace GenericBase.Infra.Ioc.Extensions.Authentication.PolicyRequirements
{
    public class BusinessTimeHandler : AuthorizationHandler<BusinessTimeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BusinessTimeRequirement requirement)
        {
            var horarioAtual = TimeOnly.FromDateTime(DateTime.Now);
            if (horarioAtual.Hour >= 8 && horarioAtual.Hour <= 18)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}