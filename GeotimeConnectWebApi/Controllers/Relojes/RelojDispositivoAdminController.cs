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
    public class RelojDispositivoAdminController : Controller
    {
        private readonly IRelojesServices _repoGT;
        public RelojDispositivoAdminController(IRelojesServices repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cRelojDispositivoAdmin>> Get() => await _repoGT.GetRelojDispositivoAdmin();

        [HttpGet("{compania}/{estado}")]
        public async Task<IEnumerable<cRelojDispositivoAdmin>> Get(string compania, string estado) => await _repoGT.GetRelojDispositivoAdmin(compania, estado);

        [HttpGet("{id}")]
        public async Task<cRelojDispositivoAdmin> Get(int id) => await _repoGT.GetRelojDispositivoAdmin(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cRelojDispositivoAdmin> dispositivos)
        {
            EventResponse respuesta = await _repoGT.PostRelojDispositivoAdmin(dispositivos);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
