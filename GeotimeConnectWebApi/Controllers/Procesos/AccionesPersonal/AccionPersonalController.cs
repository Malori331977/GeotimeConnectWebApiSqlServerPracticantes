using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.AccionesPersonal
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AccionPersonalController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public AccionPersonalController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idregistro}")]
        public async Task<cAccionPersonal> Get(long idregistro) => await _repoGT.GetAccionPersonal(idregistro);

        [HttpGet("{idplanilla}/{fechainicio}/{fechafin}")]
        public async Task<IEnumerable<cAccionPersonal>> Get(string idplanilla, DateTime fechainicio, DateTime fechafin) => await _repoGT.GetAccionPersonal(idplanilla, fechainicio, fechafin);

        [HttpGet("{idplanilla}/{fechainicio}/{fechafin}/{usuario}")]
        public async Task<IEnumerable<cAccionPersonal>> Get(string idplanilla, DateTime fechainicio, DateTime fechafin,string usuario) => await _repoGT.GetAccionPersonal(idplanilla, fechainicio, fechafin,usuario);


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cAccionPersonal> accionPersonal)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_AccionPersonal(accionPersonal);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
