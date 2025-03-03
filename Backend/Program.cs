#region Global Imports

global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Newtonsoft.Json;
global using Backend.Models;
global using System.Data;

#endregion

#region Local Imports

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using AutoMapper;
using Backend;
using Backend.Services.AuthService;
using Backend.Services.UserService;
using Backend.Services.VaultService;
using Backend.Services.OfficeService;
using Backend.Services.StatusService;
using Backend.Services.BranchService;
using Backend.Services.DivisonService;
using Backend.Services.JobTitleService;
using Backend.Services.DocumentService;
using Backend.Services.GLAccountService;
using Backend.Services.DepartmentService;
using Backend.Services.AccountSetService;
using Backend.Services.TransactionService;
using Backend.Services.RequisitionService;
using Backend.Services.NotificationService;

#endregion

try
{
    var builder = WebApplication.CreateBuilder(args);

    #region Scoping Services and Mapping

    builder.Services.AddScoped<IAuth, AuthService>();
    builder.Services.AddScoped<IUser, UserService>();
    builder.Services.AddScoped<IVault, VaultService>();
    builder.Services.AddScoped<IOffice, OfficeService>();
    builder.Services.AddScoped<IStatus, StatusService>();
    builder.Services.AddScoped<IBranch, BranchService>();
    builder.Services.AddScoped<IDivision, DivisonService>();
    builder.Services.AddScoped<IJobTitle, JobTitleService>();
    builder.Services.AddScoped<IDocument, DocumentService>();
    builder.Services.AddScoped<IGLAccount, GLAccountService>();
    builder.Services.AddScoped<IDepartment, DepartmentService>();
    builder.Services.AddScoped<IAccountSet, AccountSetService>();
    builder.Services.AddScoped<ITransaction, TransactionService>();
    builder.Services.AddScoped<IRequisition, RequisitionService>();
    //builder.Services.AddHostedService<PettyCashNotification>();
    //builder.Services.AddHostedService<VaultNotification>();

    builder.Services.AddAutoMapper(typeof(Program));

    #endregion

    #region Identity

    builder.Services.AddDbContext<BackendContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DevelopmentConnection"));
        //options.EnableSensitiveDataLogging(); 
    });
    builder.Services.AddIdentityCore<User>()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<BackendContext>()
        .AddDefaultTokenProviders();
    builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = true;
        options.Password.RequiredUniqueChars = 1;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
    });

    #endregion

    #region Auth

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CORS", o => {
            o
                //.WithOrigins("http://localhost:8080")
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }
    );
    });
    builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromMinutes(30));
    builder.Services.AddAuthentication();
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("StudentAdminPolicy", policy => policy.RequireRole("Student", "Admin"));
    });


    #endregion

    #region General

    builder.Services.ConfigureIdentity();
    builder.Services.ConfigureJWT(builder.Configuration);

    #endregion

    #region Controllers and SwaggerGen

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddControllers()
        .AddNewtonsoftJson(o =>
        {
            o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSwaggerGen(o =>
    {
        o.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "backend",
            Version = "v1"
        });

        o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorisation",
            Description = "Bearer Auth with JWT",
            Type = SecuritySchemeType.Http
        });

        o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    });

    #endregion

    var app = builder.Build();

    #region Web Application

    app.UseCors("CORS");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options => {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "backend");
            //options.RoutePrefix = string.Empty;
        });

    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    var loggerFactory = LoggerFactory.Create(builder => {
        builder.AddConsole();
    });
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError("Application is starting...");

    app.Run();

    #endregion
} catch (Exception ex)
{
    Console.WriteLine($"Unhandled Exception: ${ex}");
}