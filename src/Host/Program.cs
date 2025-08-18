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
var username = builder.AddParameter("keycloak-admin-user", "admin");
var password = builder.AddParameter("keycloak-admin-password", "admin", secret: true);

var keycloak = builder.AddKeycloak("keycloak", adminUsername: username, adminPassword: password)
    .WithDataVolume() //Opicional - Mantem os dados
    .WithRealmImport("./KeycloackConfiguration/transport-realm.json");

// 3. Migração de Banco de Dados
var jobFleetMigration = builder.AddProject<Projects.VCheck_Modules_Fleet_MigrationService>("vcheck-fleet-migrationservice")
    .WithReference(vcheckDb).WaitFor(vcheckDb)
    ;

var jobChecklistsMigration = builder.AddProject<Projects.VCheck_Modules_Checklists_MigrationService>("vcheck-checklists-migrationservice")
    .WithReference(vcheckDb).WaitFor(vcheckDb)
    ;

    // 4. Recurso da API Principal
    builder.AddProject<Projects.VCheck_Api>("vcheck-api")
       .WithReference(vcheckDb).WaitFor(vcheckDb)
       .WaitFor(jobFleetMigration)
       .WaitFor(jobChecklistsMigration)
       .WithReference(keycloak).WaitFor(keycloak)
       ;

builder.Build().Run();
