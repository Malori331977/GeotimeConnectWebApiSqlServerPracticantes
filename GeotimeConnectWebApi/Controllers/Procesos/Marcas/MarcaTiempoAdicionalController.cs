using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Procesos.Marcas
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
        public async Task<cMarcaTiempoAdicional> Get(long idRegistro) => await _repoGT.GetMarcasTiempoAdicional(idRegistro);

        [HttpGet("{idplanilla}/{idPeriodo}/{fecha}/{idconcepto}/{idgrupo}")]
        public async Task<IEnumerable<cMarcaTiempoAdicional>> Get(string idplanilla, string idPeriodo, string fecha, int idconcepto, string idgrupo) => await _repoGT.GetMarcasTiempoAdicional(idplanilla, idPeriodo, fecha, idconcepto, idgrupo);



        [HttpGet("{idplanilla}/{idPeriodo}/{idnumero}")]
        public async Task<IEnumerable<cMarcaTiempoAdicional>> Get(string idplanilla, string idPeriodo, string idnumero) => await _repoGT.GetMarcasTiempoAdicional(idplanilla, idPeriodo , idnumero);

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
