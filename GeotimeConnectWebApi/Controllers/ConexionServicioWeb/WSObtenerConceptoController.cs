using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace com.gsitcr.geotime.Controllers.ConexionServicioWeb
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WSObtenerConceptoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public WSObtenerConceptoController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet("{compania}/{session}")]
        public async Task<IEnumerable<cObtengoConcepto>> Get(string compania, string session) => await _repoGT.Obtener_Conceptos(compania, session);

    }
}
