using Auction.Web.ServiceExtension;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddDbContexts(builder.Configuration)
    .AddRepositories()
    .AddServices();
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.Run();
