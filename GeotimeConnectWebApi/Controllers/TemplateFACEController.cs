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
using SourceAFIS;

namespace GeoTimeConnectWebApi.Controllers
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
        public async Task<IEnumerable<cTemplateFACE>> Get(string idnumero) => await _repoGT.GetTemplateFACE(idnumero);

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
