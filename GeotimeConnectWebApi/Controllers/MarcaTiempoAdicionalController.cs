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
using GeoTimeConnectWebApi.Models.Request;
using GeoTimeConnectWebApi.Models.Response;
using GeotimeConnectWebApi.Models;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MarcaTiempoAdicionalController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public MarcaTiempoAdicionalController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idRegistro}")]
        public async Task<IEnumerable<cMarcaTiempoAdicional>> Get(string idRegistro) => await _repoGT.GetMarcasTiempoAdicional(idRegistro);

        // GET para Edición Condiciones especiales
        [HttpGet("{idplanilla}/{idperiodo}/{fecha}/{idconcepto}/{idgrupo}")]
        public async Task<IEnumerable<cMarcaTiempoAdicional>> Get(string idplanilla, string idperiodo, string fecha, int idconcepto , string idgrupo) => await _repoGT.GetMarcasTiempoAdicional(idplanilla, idperiodo, fecha, idconcepto, idgrupo);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cMarcaTiempoAdicional> marcasTiempoAdicional)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_MarcasTiempoAdicional(marcasTiempoAdicional);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            EventResponse respuesta = await _repoGT.Elimina_MarcasTiempoAdicional(id);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
