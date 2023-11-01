
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
    public class ParametroEmailController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public ParametroEmailController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }


        [HttpGet("{id}")]
        public async Task<cParametroEmail> Get(int id) => await _repoGT.GetParametroEmail(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cParametroEmail parametrosEmail)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_ParametroEmail(parametrosEmail);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
