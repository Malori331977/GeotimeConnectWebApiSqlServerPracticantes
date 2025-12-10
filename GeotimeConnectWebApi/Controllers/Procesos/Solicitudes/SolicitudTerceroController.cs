using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Data.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Solicitudes
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SolicitudTerceroController : Controller
    {
        private readonly ISolicitudesService _repoGT;
        public SolicitudTerceroController(ISolicitudesService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idnumero}/{fechainicial}/{fechafinal}/{tipoSolicitud}")]
        public async Task<IEnumerable<cSolicitud>> Get(string idnumero,string fechainicial,string fechafinal, int tipoSolicitud) => await _repoGT.GetSolicitudTercero(idnumero,fechainicial,fechafinal, tipoSolicitud);


    }
}
