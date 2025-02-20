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
    public class RelojTemplateFaceController : Controller
    {
        private readonly IRelojesServices _repoGT;
        public RelojTemplateFaceController(IRelojesServices repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cRelojTemplateFace>> Get() => await _repoGT.GetRelojTemplateFace();      

        [HttpGet("{id}")]
        public async Task<IEnumerable<cRelojTemplateFace>> Get(string id) => await _repoGT.GetRelojTemplateFace(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cRelojTemplateFace> templates)
        {
            EventResponse respuesta = await _repoGT.PostRelojTemplateFace(templates);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
