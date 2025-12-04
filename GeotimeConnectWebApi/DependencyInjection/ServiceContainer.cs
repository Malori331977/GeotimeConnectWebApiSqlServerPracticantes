using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Models.Utils;
using JtSegEncrypta;
using LibEncripta;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Data;

namespace RelojesApi.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection Services)
        {
            // Build a config object, using env vars and JSON providers.
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            //se desencriptan los datos de conexion a las base de datos y se pasa la cadena de conexion con los datos
            //correctos.
            var appSettingsSection = config.GetSection("AppSettings");
            string SQLConnectionString = config.GetConnectionString("SqlServerDataBaseContext")!;

            string userSQL = Encripta.getDecryptTripleDES(config.GetConnectionString("UserSQL")!);
            string passSQL = Encripta.getDecryptTripleDES(config.GetConnectionString("PassSQL")!);

            //ctadmin=7kRtaIP/ktY=
            //7ah3xu0$oa=TKbHv5rsQ0LqZRYKmhjE3g==


            string schema = config.GetConnectionString("Schema")!;
            string basedatos = config.GetConnectionString("DBName")!;

            SQLConnectionString = SQLConnectionString.Replace("UsuarioBDSQL", userSQL)
                                                     .Replace("PassBDSQL", passSQL)
                                                     .Replace("BaseDatos", basedatos);

            // Add services to the container.
            Services.AddControllers();
            Services.AddDbContext<SqlServerDataBaseContext>(options => options.UseSqlServer(SQLConnectionString)
                                                                                      .ReplaceService<IModelCacheKeyFactory, DbSchemaAwareModelCacheKeyFactory>())
                            .AddSingleton<IDbContextSchema>(new DbContextSchema(schema, DateTime.Now, ""));

            //add configurations
            Services.Configure<AppSettings>(appSettingsSection);

            //JWT
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            Services.AddAuthentication(e =>
            {
                e.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                e.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(e =>
            {
                e.RequireHttpsMetadata = false;
                e.SaveToken = true;
                e.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //Add Interfaces
            Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            Services.AddSingleton<IMemoryCache, MemoryCache>();
            Services.AddScoped<IGeoTimeConnectService, GeoTimeConnectService>();
            Services.AddScoped<IUserService, UserService>();
            Services.AddScoped<IGraphSendMail, GraphSendMail>();
            Services.AddScoped<IEncriptaService, EncriptaService>();
            Services.AddScoped<IRelojesServices, RelojesServices>();
            Services.AddScoped<IOrganizacionService, OrganizacionService>();
            Services.AddScoped<IFlujosAutorizacionService, FlujosAutorizacionService>();
            Services.AddScoped<ISolicitudesService, SolicitudesService>();
            Services.AddScoped<IMarcasService, MarcasService>();
            Services.AddScoped<IReportesServices, ReportesServices>();
            Services.AddScoped<ICrystalReportService, CrystalReportService>();
            

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();            

            return Services;

        }
    }
}
