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
    public class ProyectoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;

        public ProyectoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_Proyecto>> Get() => await _repoGT.GetProyecto();


        [HttpGet("{idproyecto}")]
        public async Task<cPh_Proyecto> Get(string idproyecto) => await _repoGT.GetProyecto(idproyecto);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_Proyecto> phProyectos)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_Proyectos(phProyectos);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
