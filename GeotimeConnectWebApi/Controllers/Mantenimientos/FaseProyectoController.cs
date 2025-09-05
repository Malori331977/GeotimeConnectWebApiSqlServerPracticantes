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
    public class FaseProyectoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;

        public FaseProyectoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_FaseProyecto>> Get() => await _repoGT.GetFaseProyecto();

        [HttpGet("{idproyecto}")]
        public async Task<IEnumerable<cPh_FaseProyecto>> Get(string idproyecto) => await _repoGT.GetFaseProyecto(idproyecto);


        [HttpGet("{idproyecto}/{fase}")]
        public async Task<cPh_FaseProyecto> Get(string idproyecto, string fase) => await _repoGT.GetFaseProyecto(idproyecto,fase);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_FaseProyecto> phFasesProyectos)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_FaseProyectos(phFasesProyectos);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
