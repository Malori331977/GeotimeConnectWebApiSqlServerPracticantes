using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models.Request;
using com.gsitcr.geotime.Models.Response;
using com.gsitcr.geotime.Models.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibEncripta;

namespace com.gsitcr.geotime.Data
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly IGeoTimeConnectService _repoGT;
        private ILogger<UserService> _logger;
        public UserService(IOptions<AppSettings> appSettings, IGeoTimeConnectService repoGT, ILogger<UserService> logger)
        {
            _appSettings = appSettings.Value;
            _repoGT = repoGT;
            _logger = logger;
        }
        public async Task<UserResponse> Auth(UserRequest user)
        {

            UserResponse respuesta= new UserResponse();

            try
            {
                UserRequest? usuario = (await UsuariosAutorizados())
                .Where(e => e.User == user.User && e.ClientId == user.ClientId
                        && e.Password == user.Password)
                .FirstOrDefault();

                if (usuario is not null)
                {
                    respuesta.User = usuario.User;
                    respuesta.Token = GetToken(user);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Auth: {ex.Message}");
            }
            
            return respuesta;

        }
        private async Task<List<UserRequest>> UsuariosAutorizados()
        {
            List<UserRequest> lista = new List<UserRequest>();
            try
            {
                var companias = await _repoGT.GetPhCompania();

                if (companias is not null && companias.Count() > 0)
                {
                    foreach (var compania in companias)
                    {
                        if (!string.IsNullOrEmpty(compania.APIUSER) && !string.IsNullOrEmpty(compania.APICLIENTID)
                         && !string.IsNullOrEmpty(compania.APIPASSWORD) && !string.IsNullOrEmpty(compania.APIDATABASE)
                         && !string.IsNullOrEmpty(compania.APIURL))
                        {
                            lista.Add(new UserRequest
                            {
                                User = compania.APIUSER!,
                                Password = compania.APIPASSWORD!,
                                ClientId = compania.APICLIENTID!
                            });
                        }

                    }
                }

                lista.Add(new UserRequest
                {
                    User = "GSITCR",
                    Password = "c5bbf3d10de5c6dfdad016e6e948a27d343b5e22f35471324388460c4e14a27c",
                    ClientId = "197ac2e4bd0843c3974725a6544e1089c4a7dcae59087543ba6428c9914c35d9"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en UsuariosAutorizados: {ex.Message}");
            }
            

            return lista;
        }

        private string GetToken(UserRequest user)
        {
            try
            {
                // Build a config object, using env vars and JSON providers.
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

                var ExpirationTime = config.GetConnectionString("TokenExpire");

                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                            new Claim[]
                            {
                            new Claim(ClaimTypes.Name, user.User),
                            new Claim(ClaimTypes.UserData, user.ClientId),
                            new Claim(ClaimTypes.GivenName, user.Schema),
                            new Claim(ClaimTypes.Spn, user.BDName)
                            }
                        ),
                    Expires = DateTime.UtcNow.AddMinutes(ExpirationTime != null ? int.Parse(ExpirationTime) : 60),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en GetToken: {ex.Message}");
            }
            return "";

        }
    }

    
}
