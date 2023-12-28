using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoTimeConnectWebApi.Data;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using System.Text.Json;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PhRolTurnoController : ControllerBase
    {
        private readonly IGeoTimeConnectService _repoGT;

        public PhRolTurnoController(IGeoTimeConnectService repoGT)
        {
            _repoGT = repoGT;
        }

        [HttpGet("{idrol}")]
        public async Task<IEnumerable<cPh_RolTurno>> Get(int idrol) => await _repoGT.GetRolTurno(idrol);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<cPh_RolTurno> rolesTurno)
        {
            EventResponse respuesta = await _repoGT.Sincronizar_RolTurno(rolesTurno);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{idregistro}")]
        public async Task<IActionResult> Delete(int idregistro)
        {
            EventResponse respuesta = await _repoGT.Elimina_RolTurno(idregistro);

            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }


    }
}
