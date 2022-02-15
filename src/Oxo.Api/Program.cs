using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Oxo.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "Open Id" },
                    { "profile", "name, nickname, and picture" }
                },
                AuthorizationUrl = new Uri(builder.Configuration["Authentication:Domain"] + "authorize?audience=" + builder.Configuration["Authentication:Audience"])
            }
        }
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Authentication:Domain"];
    options.Audience = builder.Configuration["Authentication:Audience"];
});

builder.Services.AddAuthorization();
builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserDataAccess, UserDataAccess>();
builder.Services.AddScoped<IFriendInviteDataAccess, FriendInviteDataAccess>();
builder.Services.AddScoped<GetUserForSubject>();
builder.Services.AddScoped<AddUserWithSubjectSubject>();
builder.Services.AddScoped<AddFriendInvite>();
builder.Services.AddScoped<IOuth0UserProfileApi, Outh0UserProfileApi>();
builder.Services.AddScoped<UserContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId(app.Configuration["Authentication:ClientId"]);
    });
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.UseAuthentication();
app.UseAuthorization();

app.Use(UseUser);

static async Task UseUser(HttpContext httpContext, Func<Task> next)
{
    var subject = httpContext.User.Claims
        .Where(t => t.Type == ClaimTypes.NameIdentifier)
        .FirstOrDefault()?.Value;

    if (string.IsNullOrEmpty(subject))
    {
        await next();
        return;
    }

    var getUserForSubject = httpContext.RequestServices.GetRequiredService<GetUserForSubject>();
    var user = await getUserForSubject.GetUserAsync(subject);

    if (user is not null)
    {
        await next();
        return;
    }

    var token = await httpContext.GetTokenAsync("access_token");

    if (string.IsNullOrEmpty(token))
    {
        await next();
        return;
    }

    var outh0UserProfileApi = httpContext.RequestServices.GetRequiredService<IOuth0UserProfileApi>();
    var userProfile = await outh0UserProfileApi.GetUserProfileAsync(token);

    var newUser = new User(Guid.NewGuid(), userProfile.Name);
    var addUserWithSubjectSubject = httpContext.RequestServices.GetRequiredService<AddUserWithSubjectSubject>();
    await addUserWithSubjectSubject.AddUserAsync(newUser, subject);

    await next();
}

app.MapGet("/user", async ([FromServices] UserContext userContext) =>
{
    var user = await userContext.GetUserAsync();
    return user;
})
.WithName("GetUser")
.RequireAuthorization();

app.MapPost("/addfriendinvite", async ([FromBody] AddFriendInviteBody addFriendInviteBody, [FromServices] UserContext userContext,
    [FromServices] AddFriendInvite addFriendInvite) =>
{
    var user = await userContext.GetUserAsync();

    if (user is null)
    {
        return Results.Problem();
    }

    await addFriendInvite.AddInviteAsync(addFriendInviteBody.InviteeId, user.Id);

    return Results.Accepted();
})
.RequireAuthorization();

app.Run();

public record AddFriendInviteBody(Guid InviteeId);
