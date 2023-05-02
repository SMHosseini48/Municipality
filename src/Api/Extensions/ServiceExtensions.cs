using System.Text;
using Application.Interfaces.Business;
using Application.Interfaces.Data;
using Application.Services;
using Domain;
using Domain.Enums;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Api.Extensions;

public static class ServiceExtensions
{
 

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var aliveTokenValidation = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JWT:Issuer"],
            ValidAudience = configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Municipality", policy => policy.RequireAssertion(context =>
                context.User.HasClaim(c => c.Type == "role" && c.Value == Role.Municipality.ToString())));

            options.AddPolicy("MunicipalityOrContractor",
                policy => policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "role" && c.Value == Role.Municipality.ToString()) ||
                    context.User.HasClaim(c => c.Type == "role" && c.Value == Role.Contractor.ToString())));
            
            options.AddPolicy("Employee", policy => policy.RequireAssertion(context =>
                context.User.HasClaim(c => c.Type == "role" && c.Value == Role.Employee.ToString())));
            
            options.AddPolicy("Supervisor", policy => policy.RequireAssertion(context =>
                context.User.HasClaim(c => c.Type == "role" && c.Value == Role.Supervisor.ToString())));

            options.AddPolicy("Customer", policy => policy.RequireAssertion(context =>
                context.User.HasClaim(c => c.Type == "role" && c.Value == Role.Customer.ToString())));


        });

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.SaveToken = true;
            o.MapInboundClaims = false;
            o.TokenValidationParameters = aliveTokenValidation;
        });
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<BaseUser, IdentityRole<string>>(q =>
            {
                q.User.RequireUniqueEmail = true;
                q.Password.RequiredLength = 2;
                q.Password.RequireDigit = false;
                q.Password.RequireNonAlphanumeric = false;
                q.Password.RequireUppercase = false;
                q.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<ResidueContext>()
            .AddDefaultTokenProviders();
    }

    public static void ServiceInjection(this IServiceCollection services, IConfiguration configuration)
    {
        var aliveTokenValidation = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JWT:Issuer"],
            ValidAudience = configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
            ClockSkew = TimeSpan.Zero
        };

        services.AddSingleton(aliveTokenValidation);
        services.AddTransient<IJwtService, JwtService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ICargoService, CargoService>();
        services.AddTransient<ICustomerService, CustomerService>();
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }

    public static void SwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Residue",
                    Description = "An ASP.NET Core Web API for Residue system collection"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] { }
                    }
                });
            }
        );
    }
}