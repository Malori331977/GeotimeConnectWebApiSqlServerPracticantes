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
using SourceAFIS;

namespace GeoTimeConnectWebApi.Controllers.Biometricos.TemplateRostrosHuellas
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TemplateHIDController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public TemplateHIDController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cTemplateHID>> Get() => await _repoGT.GetTemplateHID();      

        [HttpGet("{id}")]
        public async Task<IEnumerable<cTemplateHID>> Get(string idnumero) => await _repoGT.GetTemplateHID(idnumero);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cTemplateHID> templates)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_TemplateHID(templates);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] IEnumerable<cTemplateHID> templates)
        {
            EventResponseHID? respuesta = await _repoGT.Verifica_TemplateHID(templates);

            if (respuesta is null)
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{idnumero}")]
        public async Task<IActionResult> Delete(string idnumero)
        {
            EventResponse respuesta = await _repoGT.Elimina_TemplateHID(idnumero);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
