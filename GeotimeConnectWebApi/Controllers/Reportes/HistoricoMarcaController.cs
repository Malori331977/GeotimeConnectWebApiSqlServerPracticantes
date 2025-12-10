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
    public class HistoricoMarcaController : Controller
    {
        private readonly IReportesServices _repoGT;
        public HistoricoMarcaController(IReportesServices repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpPut]
        public async Task<IActionResult> Post([FromBody] cFiltroReporte filtro)
        {
            EventResponse respuesta = await _repoGT.GetReporteHistoricoMarca(filtro);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
