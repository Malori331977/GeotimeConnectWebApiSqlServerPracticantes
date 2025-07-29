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
    public class FlujoAutorizacionController : Controller
    {
        private readonly IFlujosAutorizacionService _repo;
        public FlujoAutorizacionController(IFlujosAutorizacionService repo)
        {
            _repo = repo;
        }

        
        [HttpGet]
        public async Task<IEnumerable<cFlujoAutorizacion>> Get() => await _repo.GetFlujoAutorizacion();


        [HttpGet("{id}")]
        public async Task<cFlujoAutorizacion> Get(int id) => await _repo.GetFlujoAutorizacion(id);

       
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cFlujoAutorizacion> flujoAutorizaciones)
        {
            EventResponse respuesta = await _repo.PostFlujoAutorizacion(flujoAutorizaciones);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            EventResponse respuesta = await _repo.DeleteFlujoAutorizacion(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
