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
    public class PortalDocMarcaController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PortalDocMarcaController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPortal_DocMarca>> Get() => await _repoGT.GetPortalDocMarca();

        [HttpGet("{idnumero}/{fecha}")]
        public async Task<IEnumerable<cPortal_DocMarca>> Get(string idnumero, string fecha) => await _repoGT.GetPortalDocMarca(idnumero,fecha);

        [HttpGet("{id}")]
        public async Task<cPortal_DocMarca> Get(long id) => await _repoGT.GetPortalDocMarca(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPortal_DocMarca> portalDocMarca)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PortalDocMarca(portalDocMarca);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
