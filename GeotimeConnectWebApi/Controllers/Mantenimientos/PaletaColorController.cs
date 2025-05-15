using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Utils;
using System.Text.Json;
using com.gsitcr.geotime.Models.Request;
using com.gsitcr.geotime.Models.Response;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PaletaColorController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PaletaColorController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cPaletaColor>> Get() => await _repoGT.GetPaletaColor();      

        [HttpGet("{colorid}")]
        public async Task<cPaletaColor> Get(int colorid) => await _repoGT.GetPaletaColor(colorid);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPaletaColor> colores)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_PaletaColor(colores);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{colorid}")]
        public async Task<IActionResult> Delete(int colorid)
        {
            EventResponse respuesta = await _repoGT.Elimina_PaletaColor(colorid);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
