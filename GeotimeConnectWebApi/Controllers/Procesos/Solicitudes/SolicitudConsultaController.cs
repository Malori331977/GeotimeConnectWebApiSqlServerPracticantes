using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using GeoTimeConnectWebApi.Data.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Solicitudes
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SolicitudConsultaController : Controller
    {
        private readonly ISolicitudesService _repoGT;
        public SolicitudConsultaController(ISolicitudesService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{estado}/{tipoSolicitud}/{fechainicial}/{fechafinal}")]
        public async Task<IEnumerable<cSolicitud>> Get(int estado, int tipoSolicitud, string fechainicial, string fechafinal) => await _repoGT.GetSolicitud(estado, tipoSolicitud, fechainicial, fechafinal);

       

    }
}
