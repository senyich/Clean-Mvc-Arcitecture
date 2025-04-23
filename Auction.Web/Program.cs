using OrderWebsite.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddConfigurationFile("appsettings.json");
builder.Services
    .AddDbContexts(builder.Configuration)
    .AddServices(builder.Configuration)
    .AddRepositories();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.Run();