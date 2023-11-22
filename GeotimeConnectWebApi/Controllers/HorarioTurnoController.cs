using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class HorarioTurnoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;

        public HorarioTurnoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_HorarioTurno>> Get() => await _repoGT.GetHorario_Turno();


        [HttpGet("{IDHORARIO}")]
        public async Task<cPh_HorarioTurno> Get(int IDHORARIO) => await _repoGT.GetHorario_Turno(IDHORARIO);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_HorarioTurno> Horario_Turno)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_HorarioTurno(Horario_Turno);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{IDHORARIO}")]

        public async Task<IActionResult> Delete(string IDHORARIO)
        {
            EventResponse respuesta = await _repoGT.Elimina_Horario_Turno(IDHORARIO);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
