using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using com.gsitcr.geotime.Data.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OrganizacionBaseByJefeController : Controller
    {
        private readonly IOrganizacionService _repoGT;
        public OrganizacionBaseByJefeController(IOrganizacionService repoGT)
        {
            _repoGT = repoGT;
        }


        [HttpGet("{id}")]
        public async Task<IEnumerable<cOrganizacionBaseResponsable>> Get(string id) => await _repoGT.GetOrganizacionBaseByJefe(id);

        

    }
}
