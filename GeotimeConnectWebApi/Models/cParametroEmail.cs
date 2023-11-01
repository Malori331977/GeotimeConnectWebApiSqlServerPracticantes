namespace GeoTimeConnectWebApi.Models
{
    public class cParametroEmail
    {
        public int Id { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string DefaultEmail { get; set; }
        public string DefaultPassWord { get; set; }
    }
}
