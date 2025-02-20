using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.ConexionServicioWeb
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WSObtenerTipoAccionController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public WSObtenerTipoAccionController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet("{compania}/{session}")]
        public async Task<IEnumerable<cObtengoTipoAccion>> Get(string compania, string session) => await _repoGT.Obtener_TipoAccion(compania, session);

    }
}
