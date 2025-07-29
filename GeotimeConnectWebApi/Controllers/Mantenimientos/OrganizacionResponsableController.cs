using com.gsitcr.geotime.Models;
using GeoTimeConnectWebApi.Data.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoTimeConnectWebApi.Controllers.Mantenimientos
{
    
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OrganizacionResponsableController : Controller
    {
        private readonly IOrganizacionService _repoGT;
        public OrganizacionResponsableController(IOrganizacionService repoGT)
        {
            _repoGT = repoGT;
        }


        [HttpGet("{id}")]
        public async Task<IEnumerable<cOrganizacionResponsable>> Get(int id) => await _repoGT.GetOrganizacionResponsable(id);


        
    }
}
