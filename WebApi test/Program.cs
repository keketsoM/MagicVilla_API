using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi_test;
using WebApi_test.Data;
using WebApi_test.Model;
using WebApi_test.Repository;
using WebApi_test.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbcontext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConection"));
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbcontext>();
builder.Services.AddResponseCaching();
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("CachProfile", new CacheProfile
    {
        Duration = 120
    });
}).AddNewtonsoftJson();
builder.Services.AddApiVersioning
(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
}).AddApiExplorer(option =>
{
    option.GroupNameFormat = "'v'VVV";
    option.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var Key = builder.Configuration.GetValue<string>("Apisettings:Secret");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key)),
            ValidateAudience = false,
            ValidateIssuer = false
        };
    });
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the bearer scheme. \r\n\r\n" +
        "Enter 'Bearer' [Space] and then your token in the text input below. \r\n\r\n" +
        "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                        {
                            Type =ReferenceType.SecurityScheme,
                            Id="Bearer"
                        },
                Scheme = "oauth2",
                Name="Bearer",
                In= ParameterLocation.Header
            },
            new List<string>()
        }
    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WebApi_test",
        Version = "v1.0",
        Description = "Api to manage Villa",
        Contact = new OpenApiContact
        {
            Name = "keke",
            Email = "keketsokeke03@gmail.com",
        },
        TermsOfService = new Uri("https://www.example.com"),
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://www.example.com/license"),
        }
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "WebApi_test",
        Version = "v2.0",
        Description = "Api to manage Villa",
        Contact = new OpenApiContact
        {
            Name = "keke",
            Email = "keketsokeke03@gmail.com",
        },
        TermsOfService = new Uri("https://www.example.com"),
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://www.example.com/license"),
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApi_test v2");
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi_test v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
