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
    public class PhLoginController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhLoginController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet] // Lista de elementos de cPh_Login
        public async Task<IEnumerable<cPh_Login>> Get() => await _repoGT.GetPhLogin(); 

        [HttpGet("{id}")] // Obtiene un elemento por medio del id ==> en este caso es el (correo)
        public async Task<cPh_Login> Get(string id) => await _repoGT.GetPhLogin(id); 

        [HttpPut]
        public async Task<IActionResult> Post([FromBody] cPh_Login phLogin)
        {
            EventResponse respuesta = await _repoGT.PutPhLogin(phLogin);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            EventResponse respuesta = await _repoGT.Elimina_PhLogin(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
