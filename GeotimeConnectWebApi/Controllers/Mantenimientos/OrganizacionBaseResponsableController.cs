using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using com.gsitcr.geotime.Data.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OrganizacionBaseResponsableController : Controller
    {
        private readonly IOrganizacionService _repoGT;
        public OrganizacionBaseResponsableController(IOrganizacionService repoGT)
        {
            _repoGT = repoGT;
        }


        [HttpGet("{id}")]
        public async Task<IEnumerable<cOrganizacionBaseResponsable>> Get(string id) => await _repoGT.GetOrganizacionBaseResponsable(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cOrganizacionBaseResponsable> model)
        {
            EventResponse respuesta = await _repoGT.PostOrganizacionBaseResponsable(model);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }


    }
}
