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
    public class MarcaResumenController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public MarcaResumenController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idplanilla}/{idperiodo}")]
        public async Task<IEnumerable<cMarcaResumen>> Get(string idplanilla, string idperiodo) => await _repoGT.GetMarcasResumen(idplanilla, idperiodo);

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
