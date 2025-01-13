using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers
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
