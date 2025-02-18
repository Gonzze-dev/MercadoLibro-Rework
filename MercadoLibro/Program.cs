using MercadoLibro.Features.Filters;
using MercadoLibro.Features.UserFeature;
using MercadoLibroDB;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MercadoLibroContext>(options =>
{
    var connection = builder.Configuration.GetConnectionString("MercadoLibroDB"); 
    options.UseNpgsql(connection);
});

//User
builder.Services.AddScoped<TransactionFilter>(); //Filter
builder.Services.AddScoped<UserRepository>(); //Repository
builder.Services.AddScoped<UserService>(); //Service



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

app.UseAuthorization();

app.MapControllers();

app.Run();
