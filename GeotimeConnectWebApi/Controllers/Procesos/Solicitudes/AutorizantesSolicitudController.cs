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
    public class AutorizantesSolicitudController : Controller
    {
        private readonly ISolicitudesService _repoGT;
        public AutorizantesSolicitudController(ISolicitudesService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idGrupo}/{tipoSolicitudId}/{idNumero}/{id}")]
        public async Task<IEnumerable<cAutorizante>> Get(int IdGrupo, int TipoSolicitudId, string IdNumero, long Id) => await _repoGT.GetAutorizantesSolicitud(IdGrupo, TipoSolicitudId, IdNumero, Id);
    }
}
