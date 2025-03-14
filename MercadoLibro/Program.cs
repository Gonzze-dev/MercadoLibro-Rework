using MercadoLibro.Features.UserFeature;
using MercadoLibroDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MercadoLibro.Features.AuthFeature;
using System.Security.Claims;
using MercadoLibro.Features.RefreshTokenFeature;
using MercadoLibro.Features.AuthFeature.Utils;
using MercadoLibro.Features.General.Utils;
using MercadoLibro.Features.General.Filters;
using MercadoLibro.Features.UserFeature.Repository;

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

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin")
);

//Transaction
builder.Services.AddScoped<TransactionDB>();
builder.Services.AddScoped<TransactionFilter>();
builder.Services.AddScoped<TransactionExceptionFilter>();

//User
builder.Services.AddScoped<UserRepository>();

//UserAuth
builder.Services.AddScoped<UserAuthRepository>();

//Auth
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<SocialRedHelper>();

//RefreshToken
builder.Services.AddScoped<RefreshTokenRepository>();

// Add services to the container.
builder.Services.AddControllers()
    .AddApplicationPart(typeof(AuthController).Assembly)
    .AddApplicationPart(typeof(UserController).Assembly);

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
