using GeoTimeConnectWebApi.Models.Request;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Data.Interfaz
{
    public interface IUserService
    {
        public Task<UserResponse> Auth(UserRequest user);
    }
}
