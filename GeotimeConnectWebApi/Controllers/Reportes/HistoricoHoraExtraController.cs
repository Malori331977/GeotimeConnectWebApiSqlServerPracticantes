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
    public class HistoricoHoraExtraController : Controller
    {
        private readonly IReportesServices _repoGT;
        public HistoricoHoraExtraController(IReportesServices repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpPut]
        public async Task<IActionResult> Post([FromBody] cFiltroReporte filtro)
        {
            EventResponse respuesta = await _repoGT.GetReporteHoraExtra(filtro);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
