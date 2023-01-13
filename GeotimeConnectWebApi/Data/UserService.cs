using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models.Request;
using GeoTimeConnectWebApi.Models.Response;
using GeoTimeConnectWebApi.Models.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GeoTimeConnectWebApi.Data
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public UserResponse Auth(UserRequest user)
        {
            UserResponse respuesta= new UserResponse();

            UserRequest? usuario = UsuariosAutorizados()
                .Where(e => e.User == user.User && e.ClientId == user.ClientId 
                        && e.Password == user.Password)
                .FirstOrDefault();

            if (usuario is not null)
            {
                respuesta.User = usuario.User;
                respuesta.Token = GetToken(user);
            }
        
            return respuesta;

        }
        private List<UserRequest> UsuariosAutorizados()
        {
            List<UserRequest> lista = new List<UserRequest>();

            lista.Add(new UserRequest
            {
                User = "GSITCR",
                Password = "c5bbf3d10de5c6dfdad016e6e948a27d343b5e22f35471324388460c4e14a27c",
                ClientId = "197ac2e4bd0843c3974725a6544e1089c4a7dcae59087543ba6428c9914c35d9"
            });

            lista.Add(new UserRequest
            {
                User = "FARMANOVA",
                Password = "f0d81b01ad108987352b1cd7c1b9fc9b683635fe6b19598040f250430cbf863a",
                ClientId = "6dc71b09e7f4885aeb3551eae81351f9c81d6dfc82d2042a064bc5165337fd98"
            });

            lista.Add(new UserRequest
            {
                User = "DELOITTE",
                Password = "9A4B229E90E51274B442B85D326882A3D3D6CE39A69081DFB8EF855074EEBA56",
                ClientId = "73ED37A7547BC82D03A3E6AD5ED137677824BB59089FDC123A4D32892B4BE1B0"
            });



            //CLAVE = C022367703rPCn

            return lista;
        }

        private string GetToken(UserRequest user)
        {
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
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }

    
}
