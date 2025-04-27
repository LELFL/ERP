using ELF.Endpoints;
using ELF.Interfaces.Permissions;
using ELF.Web.Endpoints;
using Entities;

namespace ELF.Web.Infrastructure;

public static class WebApplicationExtensions
{
    //public static RouteGroupBuilder MapGroup(this WebApplication app, EndpointGroupBase group)
    //{
    //    var groupName = group.GetType().Name;

    //    return app
    //        .MapGroup($"/api/{groupName}")
    //        .WithGroupName(groupName)
    //        .WithTags(groupName);
    //}

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        //var endpointGroupType = typeof(EndpointGroupBase);

        //var assembly = Assembly.GetExecutingAssembly();

        //var endpointGroupTypes = assembly.GetExportedTypes()
        //    .Where(t => t.IsSubclassOf(endpointGroupType));

        //foreach (var type in endpointGroupTypes)
        //{
        //    if (Activator.CreateInstance(type) is EndpointGroupBase instance)
        //    {
        //        instance.Map(app);
        //    }
        //}
        Users.Map(app);
        Permissions.Map(app);
        Roles.Map(app);
        Auth.Map(app);

        List<Endpoint> endpoints = new();
        foreach (var item in (app as IEndpointRouteBuilder).DataSources)
        {
            foreach (Endpoint endpoint in item.Endpoints)
            {
                endpoints.Add(endpoint);
            }
        }

        Task.Run(async () =>
        {
            using (var scope = app.Services.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IPermissionRepository>();
                var permissions = await repository.ToListAsync(repository.AsQueryable());
                foreach (var endpoint in endpoints)
                {
                    var tags = endpoint.Metadata.GetMetadata<TagsAttribute>();
                    if (tags is not null && tags.Tags.Count > 0)
                    {
                        var permissionName = tags.Tags[0];
                        var permission = permissions.FirstOrDefault(p => p.Name == permissionName);
                        if (permission == null)
                        {
                            long? parentPermissionId = null;
                            var parentTags = endpoint.Metadata.Where(x => x is TagsAttribute).FirstOrDefault() as TagsAttribute;//permissionName.Split(':')[0];
                            if (parentTags != null && parentTags.Tags.Count >= 2)
                            {
                                var parentPermissionName = parentTags.Tags[0];
                                var parentPermissionDescription = parentTags.Tags[1];
                                var parentPermission = permissions.FirstOrDefault(p => p.Name == parentPermissionName);
                                if (parentPermission == null)
                                {
                                    parentPermission = new Permission() { Name = parentPermissionName, Description = parentPermissionDescription, ParentId = parentPermissionId };
                                    await repository.AddAsync(parentPermission);
                                    permissions.Add(parentPermission);
                                }
                                parentPermissionId = parentPermission.Id;
                            }
                            permission = new Permission() { Name = permissionName, Description = endpoint.DisplayName ?? "", ParentId = parentPermissionId };
                            await repository.AddAsync(permission);
                            permissions.Add(permission);
                        }
                        //else if (permission.Description != endpoint.DisplayName && endpoint.DisplayName != null)
                        //{
                        //    permission.Description = endpoint.DisplayName;
                        //}
                    }
                }
                await repository.SaveChangesAsync();
            }
        });


        return app;
    }
}
