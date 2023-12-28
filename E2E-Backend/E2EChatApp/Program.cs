using System.Diagnostics;
using System.Text;
using E2EChatApp.Application.Extensions;
using E2EChatApp.Core.Domain.Exceptions;
using E2EChatApp.Core.Domain.Hubs;
using E2EChatApp.Core.Domain.Responses;
using E2EChatApp.Infrastructure.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Add auth
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    var jwtConfig = builder.Configuration.GetSection("Jwt");
    var key = Encoding.UTF8.GetBytes(jwtConfig.GetValue<string>("Key")
                                     ?? throw new NullReferenceException("JWT key cannot be null"));
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.GetValue<string>("Issuer"),
        ValidAudience = jwtConfig.GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// CORS config

builder.Services.AddCors(option =>
{
    option.AddPolicy("Pw_WebApi",
        builder =>
        {
            builder
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new[] {""}
        }
    });
});
builder.Services.AddServicesAndRepositories();
// Set up the DB connection
builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    {
        var config = builder.Configuration.GetSection("ConnectionStrings");
        return new DbConnectionFactory(config.GetValue<string>("DefaultConnection")
                                       ?? throw new NullReferenceException("Connection string cannot be null"));
    });
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Error handling
app.UseExceptionHandler(a => a.Run(async context => {
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
    var exception = exceptionHandlerPathFeature?.Error;
    // string trace = context.TraceIdentifier;
    var trace = Activity.Current?.Id ?? context.TraceIdentifier;
    var statusCode = context.Response.StatusCode;
    var type = "";
    if (exception is RestException restException) {
        context.Response.StatusCode = statusCode = (int)restException.Status;
        type = restException.Code ?? "";
    }

    await context.Response.WriteAsJsonAsync(new ErrorResponse(type, statusCode, trace, exception?.Message ?? ""));
}));

app.MapHub<ChatHub>("/chatHub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Pw_WebApi");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();