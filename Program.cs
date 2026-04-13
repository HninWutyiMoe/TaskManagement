using BAL.IServiceFunction;
using BAL.ServiceFunction;
using Microsoft.EntityFrameworkCore;
using REPOSITORY;
using REPOSITORY.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Connect to SQL Server
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TaskManagement;Trusted_Connection=True;TrustServerCertificate=True"));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskHeaderService, TaskHeaderService>();
builder.Services.AddScoped<ITaskDetailService, TaskDetailService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddHttpContextAccessor();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "super_secret_key_123!";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task API v1");
    c.RoutePrefix = "swagger"; 
});

app.MapControllers();

app.Run();