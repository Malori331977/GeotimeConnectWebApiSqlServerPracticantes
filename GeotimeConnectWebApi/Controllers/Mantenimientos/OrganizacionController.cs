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
    public class OrganizacionController : Controller
    {
        private readonly IOrganizacionService _repoGT;
        public OrganizacionController(IOrganizacionService repoGT)
        {
            _repoGT = repoGT;
        }

        
        [HttpGet]
        public async Task<IEnumerable<cOrganizacion>> Get() => await _repoGT.GetOrganizacion();


        [HttpGet("{id}")]
        public async Task<cOrganizacion> Get(int id) => await _repoGT.GetOrganizacion(id);

       
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cOrganizacion> model)
        {
            EventResponse respuesta = await _repoGT.PostOrganizacion(model);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            EventResponse respuesta = await _repoGT.DeleteOrganizacion(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
