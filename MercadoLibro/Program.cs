using MercadoLibro.Features.UserFeature;
using MercadoLibro.Filters;
using MercadoLibro.Utils;
using MercadoLibroDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MercadoLibro.Features.AuthFeature;
using MercadoLibro.Features.AuthFeature.Filters;
using System.Security.Claims;
using MercadoLibroDB.Models;
using MercadoLibro.Features.RefreshTokenFeature;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MercadoLibroContext>(options =>
{
    var connection = builder.Configuration.GetConnectionString("MercadoLibroDB"); 
    options.UseNpgsql(connection);
});

//Token
builder.Services.AddSingleton<JWToken>(); //Utils
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"] 
                    ?? throw new Exception("Secret not found");

        byte[] encode = Encoding.UTF8.GetBytes(key);

        SymmetricSecurityKey signingKey = new(encode);
        TimeSpan expirationToleranceTime = TimeSpan.FromMinutes(5);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = expirationToleranceTime
        };
    });

builder.Services.AddAuthorization(option => { 
    option.AddPolicy(
        "Admin", 
        policy => policy.RequireClaim(ClaimTypes.Role, "Admin")
    );
});

//Transaction
builder.Services.AddScoped<TransactionDB>();
builder.Services.AddScoped<TransactionFilter>();
builder.Services.AddScoped<TransactionExceptionFilter>();

//User
builder.Services.AddScoped<UserRepository>();

//Auth
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<LoginExceptionFilter>();

//RefreshToken
builder.Services.AddScoped<RefreshTokenRepository>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
