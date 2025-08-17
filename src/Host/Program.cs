using Aspire;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

var builder = DistributedApplication.CreateBuilder(args);

// 1. Recurso de Banco de Dados
IResourceBuilder<IResourceWithConnectionString> vcheckDb;
const string sqlName = "sqlserver", dbName = "VCheckDb";
if (!builder.ExecutionContext.IsPublishMode)
{
    var senha = builder.AddParameter("pwdSql", "S3nh@F0rte123!");
    vcheckDb = (IResourceBuilder<IResourceWithConnectionString>)
    builder.AddSqlServer(sqlName, senha, port: 62574) 
      .WithDataVolume() //Opicional - Mantem os dados
      .AddDatabase(dbName);
}
else
{
    vcheckDb = builder.AddAzureSqlServer(sqlName)
                       .AddDatabase(dbName);
}

// 2. Recurso de Autenticação (Keycloak)
//var keycloak = builder.AddKeycloak("keycloak")
//    .WithDataVolume() //Opicional - Mantem os dados
//    .WithRealmImport("./KeycloackConfiguration/vcheck-realm.json");

//keycloak.WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin");
//keycloak.WithEnvironment("KEYCLOAK_ADMIN", "admin");

// 3. Recurso da API Principal e Conexões
builder.AddProject<Projects.VCheck_Api>("vcheck-api")
       .WithReference(vcheckDb).WaitFor(vcheckDb)
       //.WithReference(keycloak).WaitFor(keycloak)
       ;

builder.Build().Run();
