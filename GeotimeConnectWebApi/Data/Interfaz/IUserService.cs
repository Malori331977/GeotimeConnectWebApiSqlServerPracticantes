using com.gsitcr.geotime.Models.Request;
using com.gsitcr.geotime.Models.Response;

namespace com.gsitcr.geotime.Data.Interfaz
{
    public interface IUserService
    {
        public Task<UserResponse> Auth(UserRequest user);
    }
}
