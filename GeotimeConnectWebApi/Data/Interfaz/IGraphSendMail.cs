

using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;

namespace com.gsitcr.geotime.Data.Interfaz
{
    public interface IGraphSendMail
    {
        public EventResponse SendMailMSGraph(IEnumerable<Email> Mensajes, cParametroEmail parametrosCorreo);
        public EventResponse SendMailSMTP(IEnumerable<Email> correos, cParametroEmail parametrosCorreo);

    }
}
