using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EasyAdmin.Utilitys;

/// <summary>
/// swagger描述添加actid
/// </summary>
public class ActHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var attr = context.ApiDescription.ActionDescriptor.AttributeRouteInfo;
        var metaDatas = context.ApiDescription.ActionDescriptor.EndpointMetadata;
        var authset = metaDatas.FirstOrDefault(x => x is AuthSetAttribute);
        var allowAnonymous = metaDatas.Any(x => x is IAllowAnonymousFilter);
        
        if (authset != null && !allowAnonymous)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            //operation.Description = "aid = "+ (int)((AuthSetAttribute)authset).authId;
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "aid",
                In = ParameterLocation.Header,
                Description = "权限Id",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "int",
                    Default= new OpenApiInteger((int)((AuthSetAttribute)authset).authId)
                }
            });
            //operation.Parameters.Add(new OpenApiParameter
            //{
            //    Name = "name",
            //    In = ParameterLocation.Header,
            //    Description = "参数备注",
            //    Required = true,
            //    Schema = new OpenApiSchema
            //    {
            //        Type = "string",
            //        Default = new OpenApiString("Bearer ")
            //    }
            //});
        }
    }
}

