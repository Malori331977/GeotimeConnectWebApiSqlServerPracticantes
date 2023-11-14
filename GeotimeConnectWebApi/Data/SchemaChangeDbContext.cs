using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models.Utils;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LibEncripta;

namespace GeoTimeConnectWebApi.Data
{
    public class SchemaChangeDbContext
    {
        public static SqlServerDataBaseContext GetSchemaChangeDbContext(string? schema = null, string? DBName = null)
        {

            // Build a config object, using env vars and JSON providers.
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            //se desencriptan los datos de conexion a las base de datos y se pasa la cadena de conexion con los datos
            //correctos.

            string SQLConnectionString = config.GetConnectionString("SqlServerDataBaseContext");
            string userSQL = Encripta.getDecryptTripleDES(config.GetConnectionString("UserSQL"));
            string passSQL = Encripta.getDecryptTripleDES(config.GetConnectionString("PassSQL"));

            //string userSQL = config.GetConnectionString("UserSQL");
            //string passSQL = config.GetConnectionString("PassSQL");

            string basedatos = (DBName is null || DBName == "") ? config.GetConnectionString("DBName") : DBName;

            SQLConnectionString = SQLConnectionString.Replace("UsuarioBDSQL", userSQL)
                                                     .Replace("PassBDSQL", passSQL)
                                                     .Replace("BaseDatos", basedatos);

            var services = new ServiceCollection()
               .AddDbContext<SqlServerDataBaseContext>(
                    builder => builder.UseSqlServer(SQLConnectionString)
                                      .ReplaceService<IModelCacheKeyFactory, DbSchemaAwareModelCacheKeyFactory>()
                                      .EnableServiceProviderCaching(false));

            if (schema != null)
                services.AddSingleton<IDbContextSchema>(new DbContextSchema(schema, DateTime.Now, ""));

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetRequiredService<SqlServerDataBaseContext>();
        }
    }
}
