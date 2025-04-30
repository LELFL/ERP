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
var productDb = mysql.AddDatabase("productDb");

var identityService = builder
    .AddProject<IdentityWeb>("identity-web",
        configure: static project =>
        {
            project.ExcludeLaunchProfile = false;
            project.ExcludeKestrelEndpoints = false;
        })
    .WithHttpsEndpoint(port: 8001, targetPort: 18001, name: "https")
                       .WithReference(cache)
                       .WithReference(identityserviceDb)
                       .WaitFor(identityserviceDb);
var identityEndpoint = identityService.GetEndpoint("https");

builder.AddProject<ProductWeb>("product-web",
        configure: static project =>
        {
            project.ExcludeLaunchProfile = false;
            project.ExcludeKestrelEndpoints = false;
        })
    .WithHttpsEndpoint(port: 8002, targetPort: 18002, name: "https")
                       .WithReference(cache)
                       .WithReference(productDb)
                       .WaitFor(productDb)
    .WithEnvironment("OpenIddict__Issuer", identityEndpoint);


builder.Build().Run();
