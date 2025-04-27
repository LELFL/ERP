using ELF.AppHost.Projects;

var builder = DistributedApplication.CreateBuilder(args);


var cache = builder.AddRedis("cache")
                   .WithImageTag("latest")
                   .WithLifetime(ContainerLifetime.Persistent)
                   .WithRedisInsight(optons =>
                   {
                       optons.WithHostPort(8551);
                       optons.WithImageTag("latest");
                   });

var password = builder.AddParameter("mysql-password", secret: true);
var mysql = builder.AddMySql("erp-mysql", password, 3308)
                   .WithEndpoint(name: "mysqlEndpoint", targetPort: 3306)
                   .WithLifetime(ContainerLifetime.Persistent)
                   .WithImageTag("latest")
                   //.WithDataVolume()
                   .WithDataBindMount(source: @"C:\Docker\erp-mysql")
                   .WithPhpMyAdmin(optons =>
                   {
                       optons.WithHostPort(8550);
                       optons.WithImageTag("latest");
                   });

var identityserviceDb = mysql.AddDatabase("identityserviceDb");

builder.AddProject<IdentityWeb>("identity-web")
       .WithReference(cache)
                       .WithReference(identityserviceDb)
                       .WaitFor(identityserviceDb);

builder.Build().Run();
