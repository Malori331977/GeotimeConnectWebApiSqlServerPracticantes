using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models.Utils;
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

string userSQL = Encripta.getDecryptTripleDES(config.GetConnectionString("UserSQL"));
string passSQL = Encripta.getDecryptTripleDES(config.GetConnectionString("PassSQL"));

//ctadmin=7kRtaIP/ktY=
//7ah3xu0$oa=TKbHv5rsQ0LqZRYKmhjE3g==


string schema = config.GetConnectionString("Schema");
string basedatos = config.GetConnectionString("DBName");
string withCors = config.GetConnectionString("WithCors");

SQLConnectionString = SQLConnectionString.Replace("UsuarioBDSQL", userSQL)
                                         .Replace("PassBDSQL", passSQL)
                                         .Replace("BaseDatos", basedatos);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<SqlServerDataBaseContext>(options => options.UseSqlServer(SQLConnectionString)
                                                                          .ReplaceService<IModelCacheKeyFactory, DbSchemaAwareModelCacheKeyFactory>())
                .AddSingleton<IDbContextSchema>(new DbContextSchema(schema, DateTime.Now, ""));

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

if (withCors is not null)
    if (withCors == "S")
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


app.UseSwagger();

string urlApi = appSettingsSection.GetValue<string>("UrlApi");

app.UseSwaggerUI(c => { c.SwaggerEndpoint($"{urlApi}swagger/v1/swagger.json", "GeoTimeConnectWebApi"); });
app.UseHttpsRedirection();


if (withCors is not null)
    if (withCors == "S")
        app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
