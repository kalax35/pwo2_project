using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PWO.API;
using PWO.API.Endpoints;
using PWO.API.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=PWO2Proj.db"));

builder.Services.AddIdentityCore<PWOUser>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddApiEndpoints();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "PWO API", Version = "v1" });

    c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Scheme = "bearer"
    });
    c.OperationFilter<AuthenticationRequirementsOperationFilter>();
});


var app = builder.Build();

// Adds /register, /login and /refresh endpoints
app.MapIdentityApi<PWOUser>();

app.MapGet("/", (HttpContext httpContext) => $"ID: {httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)}").RequireAuthorization().WithOpenApi();



//app endpoints
ToDoListEndpointBuilder.RegisterEndpoints(ref app);
ToDoListItemEndpointBuilder.RegisterEndpoints(ref app);
ToDoListISharesEndpointBuilder.RegisterEndpoints(ref app);
NotificationEndpointBuilder.RegisterEndpoints(ref app);


app.UseSwagger();
app.UseSwaggerUI();

app.Run();


public class AuthenticationRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Security == null)
            operation.Security = new List<OpenApiSecurityRequirement>();


        var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" } };
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [scheme] = new List<string>()
        });
    }
}