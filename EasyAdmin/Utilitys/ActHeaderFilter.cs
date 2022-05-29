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
        var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
        var authset = filterPipeline.Select(filterInfo => filterInfo.Filter).FirstOrDefault(filter => filter is AuthSetAttribute);
        var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);

        if (authset != null && !allowAnonymous)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Description = "aid = "+ (int)((AuthSetAttribute)authset).authId;
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

