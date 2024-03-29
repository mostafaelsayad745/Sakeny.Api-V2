//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.SwaggerGen;

//public class IgnorePropertiesSchemaFilter : ISchemaFilter
//{
    

//    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
//    {
//        var ignoreProperties = new[] { "postFeedbackTbls", "post", "user", "notifications", "items" } ;

//        foreach (var prop in ignoreProperties)
//        {
//            if (schema.Properties.ContainsKey(prop))
//                schema.Properties.Remove(prop);
//        }
//    }
//}