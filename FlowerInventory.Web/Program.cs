using FlowerInventory.Web.Configuration;
using FlowerInventory.Web.Repository;
using FlowerInventory.Web.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Minio;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


builder.Services.AddRazorPages()
    .AddMvcOptions(o => o.Filters.Add<GlobalExceptionFilter>());


builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.Configure<MinioOptions>(builder.Configuration.GetSection("Minio"));

builder.Services.AddSingleton<IMinioClient>(sp =>
{
    var o = sp.GetRequiredService<IOptions<MinioOptions>>().Value;
    return new MinioClient()
        .WithEndpoint(o.Endpoint)
        .WithCredentials(o.AccessKey, o.SecretKey)
        .WithSSL(o.UseSsl)
        .Build();
});

builder.Services.AddScoped<IFileStorage, MinioStorageService>();
builder.Services.AddScoped<IFlowerRepository, FlowerRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IFlowerService, FlowerService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

var db = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

if (app.Environment.IsDevelopment())
{
    /* Use only if there are issues with init.sql file used by docker,
     also quicker approach if you want to start locally the app */
    
    //await DbInitializer.InitializeAsync(db);
}

db.Database.Migrate();


app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();