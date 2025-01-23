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
    public class PhMenuSistemaController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhMenuSistemaController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_MenuSistema>> Get() => await _repoGT.GetPhMenuSistema();      

        [HttpGet("{id}")]
        public async Task<cPh_MenuSistema> Get(string id) => await _repoGT.GetPhMenuSistema(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_MenuSistema> portalMenu)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PhMenuSistema(portalMenu);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
