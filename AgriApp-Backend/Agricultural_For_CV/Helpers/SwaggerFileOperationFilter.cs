using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

public class SwaggerFileOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var formParams = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile)
                     || p.ParameterType == typeof(IFormFile[])
                     || p.GetCustomAttributes(typeof(FromFormAttribute), false).Any())
            .ToList();

        if (!formParams.Any()) return;

        var schema = new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>(),
            Required = new HashSet<string>()
        };

        foreach (var p in formParams)
        {
            if (p.ParameterType == typeof(IFormFile))
            {
                schema.Properties[p.Name] = new OpenApiSchema { Type = "string", Format = "binary" };
            }
            else if (p.ParameterType == typeof(IFormFile[]))
            {
                schema.Properties[p.Name] = new OpenApiSchema
                {
                    Type = "array",
                    Items = new OpenApiSchema { Type = "string", Format = "binary" }
                };
            }
            else
            {
                // لأي نوع بيانات آخر موجود في الفورم مثل int, string
                schema.Properties[p.Name] = new OpenApiSchema { Type = "string" };
            }

            schema.Required.Add(p.Name);
        }

        operation.RequestBody = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = schema
                }
            }
        };
    }
}
