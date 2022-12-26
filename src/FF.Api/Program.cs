using AspNetCoreRateLimit;
using FF.Api.SwaggerFilters;
using FF.Domain.Messaging;
using FF.Infrastructure;
using FF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FF.Core;

var builder = WebApplication.CreateBuilder(args);

GlobalFlipsConfig.ContentRootPath = builder.Environment.ContentRootPath;
GlobalFlipsConfig.WebRootPath = builder.Environment.WebRootPath;

#region FF.Infrastructure Extensions
//Infrastructure add database SQlite context
builder.Services.AddSQliteDbContext(builder.Configuration);
builder.Services.AddDefaultIdentity(builder.Configuration);
builder.Services.AddIdentityServerWithJWT(builder.Configuration);
builder.Services.AddAuthorizationAndAuthentication(builder.Configuration);
builder.Services.AddRateLimiting(builder.Configuration);
builder.Services.AddInfrastructure();
#endregion

#region FF.Core Extensions
builder.Services.AddCore();
#endregion

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//CORS options
bool.TryParse(builder.Configuration["Cors:UseCORS"], out bool useCors);
if (useCors) { builder.Services.AddCors(); }

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
            new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
             }
     });

    //add filter to add roles to the swagger UI
    option.OperationFilter<RoleOperationFilter>();
});

//sendgrid emails
builder.Services.Configure<FlipsMailMessageSenderOptions>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

//CORS - Cross origin. Allow origins set in appsettings.json
if (useCors)
{
    var origins = builder.Configuration["Cors:AllowOrigins"].Split(",");
    app.UseCors(o => o.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader());
}

app.UseClientRateLimiting();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
//make sure these two setups are in this oder otherwise "DenyAnonymousAuthorizationRequirement"
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

//seed the database if no games exist
await RunMigrations(app);

//restrict the identity login pages
bool.TryParse(builder.Configuration["Identity:RestrictPages"], out bool restrictPages);
app.UseEndpoints(opt =>
{
    if (restrictPages)
    {
        opt.MapGet("/Identity/Account/ResendEmailConfirmation",
        (x) => Task.Factory.StartNew(() => x.Response.Redirect("Login", true, false)));
        opt.MapGet("/Identity/Account/ForgotPassword",
            (x) => Task.Factory.StartNew(() => x.Response.Redirect("Login", true, false)));
        opt.MapGet("/Identity/Account/Manage",
            (x) => Task.Factory.StartNew(() => x.Response.Redirect("AccessDenied", true, false)));
        opt.MapGet("/Identity/Account/loginwith2fa",
            (x) => Task.Factory.StartNew(() => x.Response.Redirect("AccessDenied", true, false)));
    }    
});

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

//Runs Database.MigrateAsync and inserts seed data if new database
static async Task RunMigrations(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
        if (!dbContext.Games.Any())
        {
            var dataDir = Path.Combine(GlobalFlipsConfig.ContentRootPath, @"..\Domain\FF.Infrastructure\Data");
            //inserting names
            var text = File.ReadAllText(Path.Combine(dataDir, "SQL", "Names.sql"));            
            await dbContext.Database.ExecuteSqlRawAsync(text);

            //inserting update mappings
            text = File.ReadAllText(Path.Combine(dataDir, "SQL", "Mappings.sql"));
            await dbContext.Database.ExecuteSqlRawAsync(text);
            ////inserting games
            text = File.ReadAllText(Path.Combine(dataDir, "SQL", "Games.sql"));
            await dbContext.Database.ExecuteSqlRawAsync(text);
        }

        await dbContext.Database.CloseConnectionAsync();
    }
}