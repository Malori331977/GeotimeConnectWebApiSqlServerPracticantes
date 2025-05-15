using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Biometricos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RelojTemplateController : Controller
    {
        private readonly IRelojesServices _repoGT;
        public RelojTemplateController(IRelojesServices repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cRelojTemplate>> Get() => await _repoGT.GetRelojTemplate();      

        [HttpGet("{id}")]
        public async Task<IEnumerable<cRelojTemplate>> Get(string id) => await _repoGT.GetRelojTemplate(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cRelojTemplate> templates)
        {
            EventResponse respuesta = await _repoGT.PostRelojTemplate(templates);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
