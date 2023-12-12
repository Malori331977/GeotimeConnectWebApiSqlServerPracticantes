using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Utils;
using System.Text.Json;
using GeotimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
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
