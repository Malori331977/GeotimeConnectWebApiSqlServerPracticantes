using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using com.gsitcr.geotime.Data.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Solicitudes
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SolicitudAprobadaUsuarioController : Controller
    {
        private readonly ISolicitudesService _repoGT;
        public SolicitudAprobadaUsuarioController(ISolicitudesService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idnumero}")]
        public async Task<IEnumerable<cSolicitud>> Get(string idnumero) => await _repoGT.GetSolicitudAprobadaUsuario(idnumero);


    }
}
