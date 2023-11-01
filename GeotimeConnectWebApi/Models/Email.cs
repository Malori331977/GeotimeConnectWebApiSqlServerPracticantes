
namespace GeoTimeConnectWebApi.Models
{
    public class Email
    {
        public string De { get; set; }
        public string Para { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public string Adjunto { get; set; }
        public byte[] StreamAdjunto { get; set; }
        public string CC { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }

    }
}
