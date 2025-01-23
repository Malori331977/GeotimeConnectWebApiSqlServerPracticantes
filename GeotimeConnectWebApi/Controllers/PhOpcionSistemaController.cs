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
    public class PhOpcionSistemaController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhOpcionSistemaController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_OpcionSistema>> Get() => await _repoGT.GetPh_OpcionSistema();      

        [HttpGet("{id}")]
        public async Task<cPh_OpcionSistema> Get(string id) => await _repoGT.GetPh_OpcionSistema(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_OpcionSistema> portalOpcion)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PhOpcionSistema(portalOpcion);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
