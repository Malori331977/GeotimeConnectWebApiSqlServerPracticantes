using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PhOpcionesController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhOpcionesController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<cPh_Opciones> Get() => await _repoGT.GetPhOpciones();
   

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_Opciones> ph_Opciones)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PhOpciones(ph_Opciones);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
