using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PhPlanillaController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhPlanillaController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_Planilla>> Get() => await _repoGT.GetPhPlanilla();      

        [HttpGet("{idplanilla}")]
        public async Task<cPh_Planilla> Get(string idplanilla) => await _repoGT.GetPhPlanilla(idplanilla);

        [HttpGet("{nomconector}/{descplanilla}")]
        public async Task<cPh_Planilla> Get(string nomconector, string descplanilla) => await _repoGT.GetPhPlanilla(nomconector, descplanilla);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_Planilla> phPlanillas)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PhPlanilla(phPlanillas);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{idplanilla}")]
        public async Task<IActionResult> Delete(string idplanilla)
        {
            EventResponse respuesta = await _repoGT.Elimina_PhPlanilla(idplanilla);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
