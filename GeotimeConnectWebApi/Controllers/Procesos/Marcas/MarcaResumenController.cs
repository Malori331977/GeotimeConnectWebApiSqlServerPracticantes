using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaResumenController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public MarcaResumenController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idplanilla}/{idperiodo}")]
        public async Task<IEnumerable<cMarcaResumen>> Get(string idplanilla, string idperiodo) => await _repoGT.GetMarcasResumen(idplanilla, idperiodo);

        [HttpGet("{idperiodo}/{idPlanilla}/{idnumero}")]
        public async Task<IEnumerable<cMarcaResumen>> Get(string idperiodo, string idPlanilla, string idnumero) => await _repoGT.GetMarcasResumen(idperiodo,idPlanilla,idnumero);

        [HttpPost]
		public async Task<IActionResult> Post([FromBody] IEnumerable<cMarcaResumen> marcasResumen)
		{
			EventResponse respuesta = await _repoGT.Sincronizar_MarcasResumen(marcasResumen);

			if (respuesta.Id != "0")
				return BadRequest(respuesta);

			return Ok(respuesta);
		}
	}
}
