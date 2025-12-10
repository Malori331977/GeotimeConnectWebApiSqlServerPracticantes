using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Reportes
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class HistoricoConceptoResumenController : Controller
    {
        private readonly IReportesServices _repoGT;
        public HistoricoConceptoResumenController(IReportesServices repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpPut]
        public async Task<IActionResult> Post([FromBody] cFiltroReporte filtro)
        {
            EventResponse respuesta = await _repoGT.GetHistoricoConceptoResumen(filtro);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
