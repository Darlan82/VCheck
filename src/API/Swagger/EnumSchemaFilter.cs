using System;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VCheck.Api.Swagger
{
    /// <summary>
    /// Adiciona à descrição do schema de enums o mapeamento entre valor numérico e nome para facilitar no Swagger UI.
    /// </summary>
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema == null || context.Type == null) return;
            var type = context.Type;
            if (!type.IsEnum) return;

            var names = Enum.GetNames(type);
            var values = Enum.GetValues(type).Cast<object>().ToArray();
            var pairs = names.Select((n, i) => $"{Convert.ChangeType(values[i], Enum.GetUnderlyingType(type))} = {n}");
            var mapping = string.Join(", ", pairs);

            var prefix = string.IsNullOrWhiteSpace(schema.Description) ? string.Empty : schema.Description + "\n";
            schema.Description = prefix + "Valores possíveis: " + mapping;
        }
    }
}
