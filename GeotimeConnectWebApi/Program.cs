using com.gsitcr.geotime.Data;
using Microsoft.EntityFrameworkCore;
using RelojesApi.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);
// Build a config object, using env vars and JSON providers.
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

//se desencriptan los datos de conexion a las base de datos y se pasa la cadena de conexion con los datos
//correctos.
var appSettingsSection = config.GetSection("AppSettings");
string withCors = config.GetConnectionString("WithCors")!;
string Modo = config.GetConnectionString("Modo")!;

builder.Services.AddApplicationService();

/*ajustes de Seguridad*/
if (Modo == "PRD")
{
    // Configure HSTS
    // https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?WT.mc_id=DT-MVP-5003978#http-strict-transport-security-protocol-hsts
    // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Strict-Transport-Security
    builder.Services.AddHsts(options =>
    {
        options.MaxAge = TimeSpan.FromDays(180);
        options.IncludeSubDomains = true;
        options.Preload = true;
    });
    // Configure HTTPS redirection
    builder.Services.AddHttpsRedirection(options =>
    {
        options.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
        options.HttpsPort = 443;
    });
}

if (withCors is not null)
{
    if (withCors == "S")
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .WithMethods("GET", "POST", "PUT", "DELETE")
                .AllowAnyHeader());
        });
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    if (Modo == "PRD")
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();

        // Add other security headers
        app.UseMiddleware<SecurityHeadersMiddleware>();
    }
}

app.UseSwagger();

string urlApi = appSettingsSection.GetValue<string>("UrlApi")!;

app.UseSwaggerUI(c => { c.SwaggerEndpoint($"{urlApi}swagger/v1/swagger.json", "com.gsitcr.geotime"); });
app.UseHttpsRedirection();


if (withCors is not null)
    if (withCors == "S")
        app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
