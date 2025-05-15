using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using System.Text.Json;
using com.gsitcr.geotime.Models.Response;

namespace GeoTimeConnectWebApi.Controllers.Configuraciones
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

        /*
        [HttpGet("{idrol}")]
        public async Task<IEnumerable<cPh_RolTurno>> Get(int idrol) => await _repoGT.GetRolTurno(idrol);
        */

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
