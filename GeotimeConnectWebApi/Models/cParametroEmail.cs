namespace GeoTimeConnectWebApi.Models
{
    public class cParametroEmail
    {
        public int Id { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string DefaultEmail { get; set; }
        public string DefaultPassWord { get; set; }
        public string? UserName { get; set; }
        public int TipoServicio { get; set; }
        public string? ClientId { get; set; }
        public string? TenantId { get; set; }
        public string? ClientSecret { get; set; }
    }
}




