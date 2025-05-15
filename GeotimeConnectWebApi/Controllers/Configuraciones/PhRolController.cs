using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Configuraciones
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PhRolController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhRolController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_Rol>> Get() => await _repoGT.GetPhRol();

        [HttpGet("{idrol}")]
        public async Task<cPh_Rol> Get(int idrol) => await _repoGT.GetPhRol(idrol);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_Rol> phRol)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PhRol(phRol);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{idrol}")]
        public async Task<IActionResult> Delete(int idrol)
        {
            EventResponse respuesta = await _repoGT.Elimina_PhRol(idrol);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
