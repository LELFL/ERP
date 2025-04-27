using ELF.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.AddWebServices();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.Services.InitialiseDatabaseAsync();
    app.MapOpenApi();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseDefaultCors();

app.UseExceptionHandler(options => { });

app.Map("/", () => Results.Redirect("/openapi/v1.json"));

app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();

app.Run();

public partial class Program { }
