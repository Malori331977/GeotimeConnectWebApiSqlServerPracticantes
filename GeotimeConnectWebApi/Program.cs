using com.gsitcr.geotime.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
string WithOrigins = config.GetConnectionString("WithOrigins")!;
string UrlApi = config.GetConnectionString("UrlApi")!;


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
    {
        if (!String.IsNullOrEmpty(WithOrigins))
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.SetIsOriginAllowed(origin =>
                {
                    // Permitir localhost en cualquier puerto
                    return origin.StartsWith("http://localhost") ||
                            origin.StartsWith("https://localhost") ||
                            origin.StartsWith("http://127.0.0.1") ||
                            origin.StartsWith(WithOrigins);
                })
                .WithMethods("GET", "POST", "PUT", "DELETE")
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("Authorization"));
            });
        }
        else
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder
                            .SetIsOriginAllowed(origin => true)
                            .WithMethods("GET", "POST", "PUT", "DELETE")
                            .AllowAnyHeader()
                            .AllowCredentials()                              
                            .WithExposedHeaders("Authorization"));
            });
        }
        
    }
        
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

        app.UseHttpsRedirection();
    }
}

    

app.UseSwagger();

string urlApi = appSettingsSection.GetValue<string>("UrlApi")!;

app.UseSwaggerUI(c => { c.SwaggerEndpoint($"{urlApi}swagger/v1/swagger.json", "com.gsitcr.geotime"); });

if (withCors is not null)
    if (withCors == "S")
        app.UseCors("CorsPolicy");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
