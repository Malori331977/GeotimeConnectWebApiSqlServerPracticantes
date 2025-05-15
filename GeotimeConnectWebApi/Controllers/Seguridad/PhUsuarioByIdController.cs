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

namespace GeoTimeConnectWebApi.Controllers.Seguridad
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PhUsuarioByIdController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public PhUsuarioByIdController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }     

        [HttpGet("{id}")]
        public async Task<cPh_Usuario> Get(int id) => await _repoGT.GetPhUsuarioById(id);


        [HttpPut]// Actualiza de el mantenimiento de configuracion, Usuarios compañia
        public async Task<IActionResult> Post([FromBody] cPh_Usuario usuario)
        {
            EventResponse respuesta = await _repoGT.PutActualizarPhUsuario(usuario);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

    }
}
