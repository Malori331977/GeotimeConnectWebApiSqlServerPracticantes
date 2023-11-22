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
    public class HorariosController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;

        public HorariosController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_Horarios>> Get() => await _repoGT.GetHorarios();


        [HttpGet("{IDHORARIO}")]
        public async Task<cPh_Horarios> Get(int IDHORARIO) => await _repoGT.GetHorarios(IDHORARIO);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_Horarios> Horarios)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_Horarios(Horarios);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{IDHORARIO}")]
        public async Task<IActionResult> Delete(string IDHORARIO)
        {
            EventResponse respuesta = await _repoGT.Elimina_Horarios(IDHORARIO);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
