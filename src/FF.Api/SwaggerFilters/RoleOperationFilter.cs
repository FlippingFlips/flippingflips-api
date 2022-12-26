using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FF.Api.SwaggerFilters
{
    /// <summary>
    /// Reads attributes from endpoint methods to display in swagger UI
    /// </summary>
    public class RoleOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //get roles from attributes
            var roles = context.MethodInfo.
                 GetCustomAttributes(true)
                 .OfType<AuthorizeAttribute>()
                 .Select(a => a.Roles)
                 .Distinct()
                 .ToArray();

            if (!roles.Any())
            {
                roles = context.MethodInfo.DeclaringType?
                     .GetCustomAttributes(true)
                     .OfType<AuthorizeAttribute>()
                     .Select(attr => attr.Roles)
                     .Distinct()
                     .ToArray();
            }

            //add required roles
            if (roles.Any())
            {
                string rolesStr = string.Join(",", roles);
                operation.Description += $"<p> Required Roles ({rolesStr})</p>";
            }
        }
    }
}
