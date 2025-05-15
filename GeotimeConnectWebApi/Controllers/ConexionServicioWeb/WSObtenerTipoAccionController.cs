using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace com.gsitcr.geotime.Controllers.ConexionServicioWeb
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
