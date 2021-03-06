using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Reactivities.API.Extensions;
using Reactivities.API.Middleware;
using Reactivities.API.SignalR;
using Reactivities.Domain;
using Reactivities.Infrastructure.Security;
using Reactivities.Persistence;
using Reactivties.Application.Activities;
using Reactivties.Application.Core;
using Reactivties.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

 builder.Services.AddControllers(
 opt =>
 {
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
  })
  .AddFluentValidation
  (config =>
  {
      config.RegisterValidatorsFromAssemblyContaining<Create>();
  });
    /*opt =>
{
    /*var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});*/
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
//builder.Services.AddScoped<IUserAccessor, UserAccessor>();

//builder.Services.AddMediatR(typeof(List   .Handler).Assembly);

//builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
//Configure the request pipeline

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
}

else
{
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("Strict-Teansport-Security", "max-age = 31536000");
        await next.Invoke();
    });
}
//app.UseHttpsRedirection();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseCors("CorsPolicy");


app.UseAuthorization();

app.UseAuthentication();


app.MapControllers();
app.MapHub<ChatHub>("/chat"); 
//app.MapFallbackToController("Index", "Fallback");

//app.MapControllers();

//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehaviour", true);

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context, userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
     throw;
}


await app.RunAsync();
