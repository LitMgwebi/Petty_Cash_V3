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
builder.Services.AddHostedService<PettyCashNotification>();
builder.Services.AddHostedService<VaultNotification>();

builder.Services.AddAutoMapper(typeof(Program));

#endregion

#region General

builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddHttpContextAccessor();

#endregion

#region Controllers and SwaggerGen

builder.Services.AddControllers()
    .AddNewtonsoftJson(o =>
    {
        o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
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
                }
            },
            new List<string>()
        }
    });
});

#endregion

#region Auth

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});
//.AddJwtBearer (o =>
//{
//    o.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new Sy
//    }
//});
builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("StudentAdminPolicy", policy => policy.RequireRole("Student", "Admin"));
});


#endregion

#region Identity

builder.Services.AddDbContext<BackendContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevelopmentConnection"));
    options.EnableSensitiveDataLogging();
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

var app = builder.Build();

#region Web Application

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(options => options
    .AllowAnyHeader()
    .AllowAnyMethod()
//.AllowAnyOrigin()
//.WithOrigins("http://localhost:8080")
);

app.UseRouting();

app.UseHttpsRedirection();
app.UseFileServer();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion