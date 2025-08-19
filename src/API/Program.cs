using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.OpenApi.Models;
using VCheck.Modules.Checklists;
using VCheck.Modules.Fleet;

var builder = WebApplication.CreateBuilder(args);

//Desabilite o mapeamento de claims para usar os nomes exatos do JWT. Padrão novo de autenticação
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

//Módulos
builder.AddFleetModule();
builder.AddChecklistsModule();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddHttpClient();

var keycloakUrl = builder.Configuration["services:keycloak:http:0"] ?? "http://localhost:8080";

// Keycloak JWT config via Aspire
builder.Services.AddAuthentication()
                .AddKeycloakJwtBearer(
                    serviceName: "keycloak",
                    realm: "transport",
                    options =>
                    {
                        options.Audience = "vcheck-api";

                        var keycloakUri = new Uri(keycloakUrl);
                        options.Authority = $"{keycloakUri.Scheme}://{keycloakUri.Authority}/realms/transport";

                        // Diz à API para procurar as roles na claim "roles" do token
                        options.TokenValidationParameters.RoleClaimType = "roles";

                        // For development only - disable HTTPS metadata validation
                        // In production, use explicit Authority configuration instead
                        if (builder.Environment.IsDevelopment())
                        {
                            options.RequireHttpsMetadata = false;
                        }
                    });

// Obtém as configurações do Keycloak
var realm = builder.Configuration["Keycloak:Realm"] ?? "transport";
var clientId = builder.Configuration["Keycloak:Audience"] ?? "vcheck-api";
var clientSecret = builder.Configuration["Keycloak:Secret"] ?? "S3cr3t";

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VCheck API", Version = "v1" });

    // Define o esquema de segurança OAuth2
    c.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            // Configura o fluxo de "Password Credentials"
            Password = new OpenApiOAuthFlow
            {
                // URL para onde o usuário é enviado para fazer login
                AuthorizationUrl = new Uri($"{keycloakUrl}/realms/{realm}/protocol/openid-connect/auth"),

                // URL do endpoint de token do Keycloak
                TokenUrl = new Uri($"{keycloakUrl}/realms/{realm}/protocol/openid-connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    // Adicione escopos se o seu cliente Keycloak os exigir
                    { "openid", "OpenID Connect" },
                    { "profile", "User Profile" },
                    { "email", "User Email" }
                }
            }
        },
        Description = "Autenticação via Keycloak"
    });

    // Adiciona o requisito de segurança globalmente a todos os endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "OAuth2" // Deve corresponder ao nome definido em AddSecurityDefinition
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => // Adicionar esta configuração para o UseSwaggerUI
    {
        // Passa o client_id e client_secret para a UI do Swagger
        options.OAuthClientId(clientId);
        options.OAuthClientSecret(clientSecret);
        options.OAuthAppName("VCheck API - Swagger UI");
        options.OAuthUsePkce(); // PKCE é uma boa prática de segurança
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
