using GeotimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Data.Interfaz
{
    public interface IGeoTimeConnectService
    {   public Task<List<cAccionPersonal>> GetAccionPersonal(string IdPlanilla, DateTime FechaInicio, DateTime FechaFin);
        public Task<List<cAccionPersonal>> GetAccionPersonal(string IdPlanilla, DateTime FechaInicio, DateTime FechaFin, string usuario);
        public Task<List<cAccionPersonal>> GetAccionPersonalPorEstado(string IdPlanilla, string usuario, char estado);
        public Task<List<cAccionPersonal>> GetAccionPersonalPorEstado(string IdPlanilla, char estado);

        public Task<EventResponse> Sincronizar_AccionPersonal(IEnumerable<cAccionPersonal> accionPersonal);
        public Task<List<cCentroCosto>> GetCentroCosto();
        public Task<cCentroCosto> GetCentroCosto(string idCCosto);
        public Task<EventResponse> Sincronizar_Centro_Costo(IEnumerable<cCentroCosto> centrosCosto);
        public Task<List<cConcepto>> GetConcepto();
        public Task<cConcepto> GetConcepto(string concepto);
        public Task<EventResponse> Sincronizar_Concepto(IEnumerable<cConcepto> conceptos);
        public Task<List<cDepartamento>> GetDepartamento();
        public Task<cDepartamento> GetDepartamento(string idDepart);
        public Task<EventResponse> Sincronizar_Departamento(IEnumerable<cDepartamento> departamentos);
        public Task<List<cEmpleado>> GetEmpleado();
        public Task<cEmpleado> GetEmpleadoByEmail(string email);
        public Task<cEmpleado> GetEmpleado(string idNumero);
        public Task<List<cEmpleado>> GetEmpleadoFiltrado(string idnumero, string nombre, string iddepartamento);
        public Task<EventResponse> Sincronizar_Empleado(IEnumerable<cEmpleado> empleados);
        public Task<List<cIncidencia>> GetIncidencia();
        public Task<cIncidencia> GetIncidencia(int id);
        public Task<cIncidencia> GetIncidenciaByNomConector(string nom_conector);
        public Task<List<cIncidencia>> GetIncidenciaReqAccPer();
        public Task<EventResponse> Sincronizar_Incidencia(IEnumerable<cIncidencia> incidencias);
        public Task<List<cMarcaResumen>> GetMarcasResumen(string idPlanilla, string idPeriodo);
		public Task<EventResponse> Sincronizar_MarcasResumen(IEnumerable<cMarcaResumen> marcasResumen);
        public Task<List<cTurno>> GetTurno();
        public Task<cTurno> GetTurno(int idTurno);
        public Task<List<cMarca>> GetMarcas();
        public Task<List<cMarca>> GetMarcas(string idnumero);
        public Task<EventResponse> Sincronizar_Marca(IEnumerable<cMarca> marcas);
        public Task<EventResponse> ValidarClaveEmpleado(cLogin login);
        public Task<EventResponse> CambiarClaveEmpleado(cEmpleado empleado);
        public Task<cPh_Login> GetPhLogin(string id);
        public Task<List<cPh_Compania>> GetPhCompania();
        public Task<cPh_Compania> GetPhCompania(string idcomp);
        public Task<EventResponse> Sincronizar_AccionPersonalNomConector(IEnumerable<cAccionPersonal> accionPersonal);
        public Task<List<cMarcaMovTurno>> GetMarcaMovTurno();
        public Task<cMarcaMovTurno> GetMarcaMovTurno(int idregistro);
        public Task<cMarcaMovTurno> GetMarcaMovTurno(string idnumero, string fecha, int idturno);
        public Task<EventResponse> Sincronizar_MarcasMovTurnos(IEnumerable<cMarcaMovTurno> marcasMovTurnos);
        public Task<List<cPh_Grupo>> GetGrupo();
        public Task<cPh_Grupo> GetGrupo(int idgrupo);
        public Task<IEnumerable<cPh_Periodos>> GetPeriodo();
        public Task<cPh_Periodos> GetPeriodo(string idperiodo);
        public Task<IEnumerable<cPh_Periodos>> GetPeriodo(string fecha, string vigente);
        public Task<IEnumerable<cPh_Planilla>> GetPhPlanilla();
        public Task<cPh_Planilla> GetPhPlanilla(string idplanilla);
        public Task<cPh_Planilla> GetPhPlanilla(string nomConector, string descPlanilla);



    }
}
