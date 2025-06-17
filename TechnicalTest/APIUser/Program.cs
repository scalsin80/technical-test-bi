using APIUser.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configuration Serilog
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.File("log/APIUser.txt", rollingInterval: RollingInterval.Day)//File by day
        .CreateLogger();
    builder.Host.UseSerilog();

    builder.Services.AddDbContext<dbdataContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("dbdata")));

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //Configure JWT
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
         options.TokenValidationParameters = new TokenValidationParameters
         {
             // Who generate
             ValidateIssuer = false,
             ValidIssuer = jwtSettings["validIssuer"],

             // who use
             ValidateAudience = false,
             ValidAudience = jwtSettings["validAudience"],

             // Validate the signature wont was changed
             ValidateIssuerSigningKey = true,
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["secretKey"]!)),

             // Valida el tiempo de vida del token
             ValidateLifetime = true
         };
    });

    builder.Services.AddAuthorization();


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

    try
    {
        Log.Information("Started API - Ok");
        app.Run();
    }
    catch (Exception ex)
    {
        Log.Error("Started API - ERROR");
        Log.Error(ex.Message);
        Log.Error(ex.StackTrace??"");
        throw;
    }

    
}
catch (ApplicationException)
{
	throw;
}


