using com.gsitcr.geotime.Models.ErpClases;

namespace com.gsitcr.geotime.Data.Interfaz
{
    public interface IErpConnectService
    {
        public Task<IEnumerable<cDepartamentoErp>> GetDepartamentoErp();
        public Task<IEnumerable<cCentroCostoErp>> GetCentroCostoErp();
        public Task<IEnumerable<cConceptoErp>> GetConceptoErp();
        public Task<IEnumerable<cEmpleadoErp>> GetEmpleadoErp();
        public Task<IEnumerable<cPuestoErp>> GetPuestoErp();

    }
}
