
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

Startup startUp = new Startup(builder.Configuration);

startUp.AddJsonSettings(builder.Services);

//startUp.AddAuthentication(builder.Services, builder.Environment.IsProduction());

startUp.AddDomainConfiguration(builder.Services);

//startUp.AddApiExplorer(builder.Services);

//startUp.AddSwaggerGen(builder.Services);

//Log.Logger = new LoggerConfiguration()
//                .ReadFrom.Configuration(builder.Configuration)
//                .Enrich.FromLogContext()
//                .CreateLogger();

//builder.Logging.AddSerilog(Log.Logger);

WebApplication app = builder.Build();

SettingsHelper.Initialize(app.Configuration);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

//startUp.ConfigureSwagger(app);

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

//app.UseSwagger();

app.Run();
