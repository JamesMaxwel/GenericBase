using Microsoft.AspNetCore.Authorization;

namespace GenericBase.Infra.Ioc.Extensions.Authentication.PolicyRequirements
{
    public class BusinessTimeRequirement : IAuthorizationRequirement
    {
        public BusinessTimeRequirement() { }
    }
}