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
    public class PhDescansoTurnoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhDescansoTurnoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idTurno}/{idTiempo}")]
        public async Task<IEnumerable<cPh_DescansoTurno>> Get(int idTurno, int idTiempo) => await _repoGT.GetPhDescansoTurno(idTurno, idTiempo);      


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_DescansoTurno> ph_DescansoTurno)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PhDescansoTurno(ph_DescansoTurno);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
