using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GrupoController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public GrupoController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPh_Grupo>> Get() => await _repoGT.GetGrupo();      

        [HttpGet("{idgrupo}")]
        public async Task<cPh_Grupo> Get(int idgrupo) => await _repoGT.GetGrupo(idgrupo);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_Grupo> phGrupo)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_Grupo(phGrupo);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{idgrupo}")]
        public async Task<IActionResult> Delete(int idgrupo)
        {
            EventResponse respuesta = await _repoGT.Elimina_Grupo(idgrupo);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }


    }
}
