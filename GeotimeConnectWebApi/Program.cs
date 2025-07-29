using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models.Utils;
using System.Text;
using LibEncripta;
using JtSegEncrypta;
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

builder.Services.AddApplicationService();

var app = builder.Build();

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
