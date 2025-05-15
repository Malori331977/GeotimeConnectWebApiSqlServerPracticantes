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
    public class TemplateFACEController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public TemplateFACEController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet]
        public async Task<IEnumerable<cTemplateFACE>> Get() => await _repoGT.GetTemplateFACE();      

        [HttpGet("{id}")]
        public async Task<IEnumerable<cTemplateFACE>> Get(string id) => await _repoGT.GetTemplateFACE(id);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cTemplateFACE> templates)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_TemplateFACE(templates);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

   
        [HttpDelete("{idnumero}")]
        public async Task<IActionResult> Delete(string idnumero)
        {
            EventResponse respuesta = await _repoGT.Elimina_TemplateFACE(idnumero);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
