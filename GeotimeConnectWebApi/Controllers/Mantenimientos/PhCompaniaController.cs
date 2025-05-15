using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    
    public class PhCompaniaController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhCompaniaController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_Compania>> Get() => await _repoGT.GetPhCompania();

        [Authorize]
        [HttpGet("{idcomp}")]
        public async Task<cPh_Compania> Get(string idcomp) => await _repoGT.GetPhCompania(idcomp);

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_Compania> phCompanias)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PhCompania(phCompanias);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
