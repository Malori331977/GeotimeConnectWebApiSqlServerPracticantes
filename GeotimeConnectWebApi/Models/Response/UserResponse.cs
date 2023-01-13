namespace GeoTimeConnectWebApi.Models.Response
{
    public class UserResponse
    {
        
        public string? User { get; set; }
        public string? Token { get; set; }

        public UserResponse()
        {
            User = "";
            Token = "";
        }
    }
}
