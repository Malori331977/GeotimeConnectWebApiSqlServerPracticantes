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
    public class PhFormulacionController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhFormulacionController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_Formulacion>> Get() => await _repoGT.GetPhFormulacion();      

        [HttpGet("{id}")]
        public async Task<cPh_Formulacion> Get(int id) => await _repoGT.GetPhFormulacion(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_Formulacion> ph_Formulacion)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PhFormulacion(ph_Formulacion);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            EventResponse respuesta = await _repoGT.Elimina_PhFormulacion(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
