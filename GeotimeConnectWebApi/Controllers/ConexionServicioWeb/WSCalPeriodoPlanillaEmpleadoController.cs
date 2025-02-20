using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GeoTimeConnectWebApi.Models.CalculoPeriodoParam;

namespace GeoTimeConnectWebApi.Controllers.ConexionServicioWeb
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WSCalPeriodoPlanillaEmpleadoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public WSCalPeriodoPlanillaEmpleadoController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cCalculoPeriodoParam parametros)
        {
            EventResponse respuesta = await _repoGT.EjecutaCalculoPlanillaEmpleado(parametros);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
