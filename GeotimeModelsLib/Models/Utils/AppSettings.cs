namespace com.gsitcr.geotime.Models.Utils
{
    public class AppSettings
    {
        public string? Secret { get; set; }
        public string? UrlApi { get; set; }
        public string? WSEndPoint { get; set; }
        public string? WSTimeOut { get; set; }
        public string? ERPTimeOut { get; set; }
        public string? ApiErpUrl { get; set; }
        public string? ApiErpDataBase { get; set; }
        public string? ApiErpSchema { get; set; }
        public string? ApiClientId { get; set; }
        public string? ApiPassword { get; set; }
        public string? ApiUser { get; set; }

        public override string ToString()
        {
            return $"UrlApi: {UrlApi}, WSEndPoint: {WSEndPoint}, WSTimeOut: {WSTimeOut}, ERPTimeOut: {ERPTimeOut}, ApiErpUrl: {ApiErpUrl}, ApiErpDataBase: {ApiErpDataBase}, ApiErpSchema: {ApiErpSchema}, ApiClientId: {ApiClientId}, ApiPassword: {ApiPassword}, ApiUser: {ApiUser}";
        }

    }
}
