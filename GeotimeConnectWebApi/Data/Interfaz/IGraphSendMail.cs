

using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Data.Interfaz
{
    public interface IGraphSendMail
    {
        public EventResponse SendMailMSGraph(IEnumerable<Email> Mensajes, cParametroEmail parametrosCorreo);
        public EventResponse SendMailSMTP(IEnumerable<Email> correos, cParametroEmail parametrosCorreo);
    }
}
