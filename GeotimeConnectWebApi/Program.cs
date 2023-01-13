using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models.Utils;
using System.Data.Entity.Infrastructure;
using System.Text;
using LibEncripta;


var builder = WebApplication.CreateBuilder(args);

// Build a config object, using env vars and JSON providers.
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

//se desencriptan los datos de conexion a las base de datos y se pasa la cadena de conexion con los datos
//correctos.
var appSettingsSection = config.GetSection("AppSettings");
string SQLConnectionString = config.GetConnectionString("SqlServerDataBaseContext");
//var sr = Encripta.getEncryptTripleDES("Mloria");
//var psr = Encripta.getEncryptTripleDES("Listo123@");

var sPassword = Encripta.getEncryptTripleDES("f0d81b01ad108987352b1cd7c1b9fc9b683635fe6b19598040f250430cbf863a");
var sClientId = Encripta.getEncryptTripleDES("6dc71b09e7f4885aeb3551eae81351f9c81d6dfc82d2042a064bc5165337fd98");

string userSQL = Encripta.getDecryptTripleDES(config.GetConnectionString("UserSQL"));
string passSQL = Encripta.getDecryptTripleDES(config.GetConnectionString("PassSQL"));
string schema = config.GetConnectionString("Schema");
string basedatos = config.GetConnectionString("DBName");

SQLConnectionString = SQLConnectionString.Replace("BaseDatos", basedatos)
                                         .Replace("UsuarioBDSQL", userSQL)
                                         .Replace("PassBDSQL", passSQL);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<SqlServerDataBaseContext>(options => options.UseSqlServer(SQLConnectionString)
                                                                          .ReplaceService<IModelCacheKeyFactory, DbSchemaAwareModelCacheKeyFactory>())
                .AddSingleton<IDbContextSchema>(new DbContextSchema(schema,DateTime.Now,""));
//add configurations
builder.Services.Configure<AppSettings>(appSettingsSection);

//JWT
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);
builder.Services.AddAuthentication(e =>
{
    e.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    e.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(e =>
{
    e.RequireHttpsMetadata = false;
    e.SaveToken = true;
    e.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey=true,
        IssuerSigningKey=new SymmetricSecurityKey(key),
        ValidateIssuer=false,
        ValidateAudience=false
    };
});

//Add Interfaces
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddScoped<IGeoTimeConnectService, GeoTimeConnectService>();
builder.Services.AddScoped<IUserService, UserService>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


app.UseSwagger();
//app.UseSwaggerUI();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeoTimeConnectWebApi"); });
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
