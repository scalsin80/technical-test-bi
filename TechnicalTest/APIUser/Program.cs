using Serilog;

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


