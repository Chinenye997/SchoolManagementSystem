
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolManagementTask6.Domain.UserManager;
using SchoolManagementTask6.Persistence;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    // Configure JSON options to convert enums to strings in API responses
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddDbContext<SchoolManagementTask6DbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Define the security scheme for JWT Bearer tokens in Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",  // The name of the header
        Type = SecuritySchemeType.Http,  // The type of security scheme
        Scheme = "Bearer",  // The scheme name
        In = ParameterLocation.Header,  // Where the token is placed (header)
        Description = "JWT Authorization header that uses a bearer scheme. Enter token in the text below"
    });

    // Add a global security requirement that applies to all API endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"  // Reference the security scheme defined above
                }
            },
            Array.Empty<string>()  // No specific scopes required
        }
    });
});

// Configure authentication services to use JWT Bearer tokens
builder.Services.AddAuthentication(options =>
{
    // Set the default authentication scheme to JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>  // Add JWT Bearer token handling
{
    // Configure how to validate incoming JWT tokens
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,  // Validate the token issuer
        ValidateAudience = true,  // Validate the token audience
        ValidateLifetime = true,  // Check if the token is not expired
        ValidateIssuerSigningKey = true,  // Validate the signing key
        ValidIssuer = builder.Configuration["Issuer"],  // Get valid issuer from configuration
        ValidAudience = builder.Configuration["Audience"],  // Get valid audience from configuration
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["key"])),  // Get signing key from configuration
        ClockSkew = TimeSpan.Zero  // No tolerance for expiration time differences
    };
});

// Configure authorization policies based on user roles
builder.Services.AddAuthorizationBuilder()
    // Create an "Admin, Student,Lecturer" policy that requires the user to have the Admin, Student and Lecturer role
    .AddPolicy("Admin", policy => policy.RequireRole(UserRole.Admin.ToString()))
    .AddPolicy("Lecturer", policy => policy.RequireRole(UserRole.Lecturer.ToString()))
    .AddPolicy("Student", policy => policy.RequireRole(UserRole.Student.ToString()));


// Build the application using all the configured services
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
