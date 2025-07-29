using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using GeoTimeConnectWebApi.Data.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OrganizacionNivelController : Controller
    {
        private readonly IOrganizacionService _repoGT;
        public OrganizacionNivelController(IOrganizacionService repoGT)
        {
            _repoGT = repoGT;
        }

        
        [HttpGet]
        public async Task<IEnumerable<cOrganizacionNivel>> Get() => await _repoGT.GetOrganizacionNivel();


        [HttpGet("{id}")]
        public async Task<cOrganizacionNivel> Get(string id) => await _repoGT.GetOrganizacionNivel(id);

       
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cOrganizacionNivel> model)
        {
            EventResponse respuesta = await _repoGT.PostOrganizacionNivel(model);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            EventResponse respuesta = await _repoGT.DeleteOrganizacionNivel(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
