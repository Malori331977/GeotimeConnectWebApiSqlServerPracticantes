using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace com.gsitcr.geotime.Controllers.ConexionServicioWeb
{
    [ApiController]
    [Route("[controller]")]

    public class WSNivelesSeguridadController : Controller
    {
        private readonly IGeoTimeConnectService _repoGT;
        public WSNivelesSeguridadController(IGeoTimeConnectService repoGT)
        {

            _repoGT = repoGT;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] cPh_Compania compania)
        {
            EventResponse respuesta = new();

            try
            {
                if (compania == null || string.IsNullOrEmpty(compania.IDCOMP))
                {
                    respuesta.Id = "1";
                    respuesta.Descripcion = "La información de la compañía es inválida.";
                    return BadRequest(respuesta);
                }

                await _repoGT.CreaNivelesSeguridad(compania.IDCOMP);
            }
            catch (Exception ex)
            {
                respuesta.Id = "1";
                respuesta.Descripcion = "Error al procesar la solicitud: " + ex.Message;
                return BadRequest(respuesta);

            }
            if (respuesta.Id != "0")
                return BadRequest(respuesta);

            return Ok(respuesta);
        }
    }
}
