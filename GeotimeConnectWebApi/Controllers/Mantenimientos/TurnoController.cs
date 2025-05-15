using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;


namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TurnoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;

        public TurnoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cTurno>> Get() => await _repoGT.GetTurno();

        [HttpGet("{idturno}")]
        public async Task<cTurno> Get(int idturno) => await _repoGT.GetTurno(idturno);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cTurno> phTurno)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_Turno(phTurno);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{idturno}")]
        public async Task<IActionResult> Delete(int idturno)
        {
            EventResponse respuesta = await _repoGT.Elimina_Turno(idturno);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
