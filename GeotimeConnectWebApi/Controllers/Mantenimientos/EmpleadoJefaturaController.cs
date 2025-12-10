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
    public class EmpleadoJefaturaController : Controller
    {
        private readonly IOrganizacionService _repoGT;
        public EmpleadoJefaturaController(IOrganizacionService repoGT)
        {
            _repoGT = repoGT;
        }

        
        [HttpGet]
        public async Task<IEnumerable<cEmpleadoJefatura>> Get() => await _repoGT.GetEmpleadoJefatura();


        [HttpGet("{id}")]
        public async Task<cEmpleadoJefatura> Get(string id) => await _repoGT.GetEmpleadoJefatura(id);

        [HttpGet("{id}/{esJefe}")]
        public async Task<IEnumerable<cEmpleadoJefatura>> Get(string id,bool esJefe) => await _repoGT.GetEmpleadoJefatura(id,esJefe);


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cEmpleadoJefatura> empleadosJefaturas)
        {
            EventResponse respuesta = await _repoGT.PostEmpleadoJefatura(empleadosJefaturas);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            EventResponse respuesta = await _repoGT.DeleteEmpleadoJefatura(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
