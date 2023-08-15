using GeotimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;

namespace GeoTimeConnectWebApi.Data.Interfaz
{
    public interface IGeoTimeConnectService
    {   
        public Task<List<cAccionPersonal>> GetAccionPersonal(string IdPlanilla, DateTime FechaInicio, DateTime FechaFin);
        public Task<List<cAccionPersonal>> GetAccionPersonal(string IdPlanilla, DateTime FechaInicio, DateTime FechaFin, string usuario);
        public Task<List<cAccionPersonal>> GetAccionPersonalPorEstado(string IdPlanilla, string usuario, char estado);
        public Task<List<cAccionPersonal>> GetAccionPersonalPorEstado(string IdPlanilla, char estado);

        public Task<EventResponse> Sincronizar_AccionPersonal(IEnumerable<cAccionPersonal> accionPersonal);
        public Task<EventResponse> Sincronizar_AccionPersonalNomConector(IEnumerable<cAccionPersonal> accionPersonal);
        public Task<EventResponse> Sincronizar_AccionPersonal_AutoGestion(IEnumerable<cAccionPersonal> accionPersonal);
        public Task<EventResponse> Sincronizar_AccionPersonal_PreJustificacion(IEnumerable<cAccionPersonal> accionPersonal);        

        public Task<List<cCentroCosto>> GetCentroCosto();
        public Task<cCentroCosto> GetCentroCosto(string idCCosto);
        public Task<EventResponse> Sincronizar_Centro_Costo(IEnumerable<cCentroCosto> centrosCosto);
        public Task<List<cConcepto>> GetConcepto();
        public Task<cConcepto> GetConcepto(string concepto);
        public Task<EventResponse> Sincronizar_Concepto(IEnumerable<cConcepto> conceptos);
        public Task<List<cDepartamento>> GetDepartamento();
        public Task<cDepartamento> GetDepartamento(string idDepart);
        public Task<EventResponse> Sincronizar_Departamento(IEnumerable<cDepartamento> departamentos);

        /// <summary>
        /// GetEmpleado: Método para obtener una lista de empleados 
        /// </summary>
        /// <returns>Lista de cEmpleados</returns>
        public Task<List<cEmpleado>> GetEmpleado();
        public Task<cEmpleado> GetEmpleadoByEmail(string email);

        /// <summary>
        /// GetEmpleado: Método para un empleado específico
        /// </summary>
        /// <returns>Una instancia de la clase cEmpleado</returns>
        /// ///<param name="idNumero">idNumero del empleado requerido</param>
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
        
        public Task<List<cMarcaMovTurno>> GetMarcaMovTurno();
        public Task<cMarcaMovTurno> GetMarcaMovTurno(int idregistro);
        public Task<cMarcaMovTurno> GetMarcaMovTurno(string idnumero, string fecha, int idturno);

        /// <summary>
        /// GetMarcaMovTurno: Método para obtener una lista de Marcas Mov Turnos por empleado 
        /// </summary>
        /// <returns>Lista de cMarcaMovTurno</returns>
        /// <param name="idnumero">Número de Empleado</param>
        /// <param name="fechaPeriodo">Fecha del Periodo para el cual se requieren los turnos</param>
        public Task<List<cMarcaMovTurno>> GetMarcaMovTurno(string idnumero, string fechaPeriodo);
        public Task<EventResponse> Sincronizar_MarcasMovTurnos(IEnumerable<cMarcaMovTurno> marcasMovTurnos);
        public Task<List<cPh_Grupo>> GetGrupo();
        public Task<cPh_Grupo> GetGrupo(int idgrupo);

        /// <summary>
        /// getPeriodo: Método para obtener una lista de Periodos 
        /// </summary>
        /// <returns>Lista de cPh_Periodos</returns>
        public Task<IEnumerable<cPh_Periodos>> GetPeriodo();

        /// <summary>
        /// getPeriodo: Método para un periodo específico
        /// </summary>
        /// <returns>Una instancia de la clase cPh_Periodos</returns>
        /// ///<param name="idperiodo">idperiodo del periodo requerido</param>
        public Task<cPh_Periodos> GetPeriodo(string idperiodo);

        /// <summary>
        /// getPeriodo: Método para obtener una lista de Periodos 
        /// </summary>
        /// <returns>Lista de cPh_Periodos</returns>
        /// <param name="fecha">Fecha del periodo</param>
        /// <param name="vigente">Periodo está vigente </param>
        public Task<IEnumerable<cPh_Periodos>> GetPeriodo(string fecha, string vigente);

        /// <summary>
        /// GetPeriodoVigenteEmpleado: Método para obtener el periodo vigenta para un empleado  
        /// </summary>
        /// <returns>Un item de cPh_Periodos</returns>
        /// <param name="fecha">Fecha del periodo</param>
        /// <param name="idnumero">Número de empleado</param>
        public Task<cPh_Periodos> GetPeriodoVigenteEmpleado(string idnumero, string fechaPeriodo);

        public Task<IEnumerable<cPh_Planilla>> GetPhPlanilla();
        public Task<cPh_Planilla> GetPhPlanilla(string idplanilla);
        public Task<cPh_Planilla> GetPhPlanilla(string nomConector, string descPlanilla);


        /// <summary>
        /// GetMarcaIn: Método para obtener todos los datos de la tabla Marcas_In 
        /// </summary>
        /// <returns>Lista de registros de la clase cMarcaIn</returns>
        public Task<List<cMarcaIn>> GetMarcaIn();

        /// <summary>
        /// GetMarcaIn: Método para obtener todos los datos de la tabla Marcas_In de un colaborador
        /// </summary>
        /// <returns>Lista de registros de la clase cMarcaIn para un colaborador en específico</returns>
        /// <param name="idtarjeta">Id de Tarjeta del colaborador</param>
        public Task<List<cMarcaIn>> GetMarcaIn(string idtarjeta);

        /// <summary>
        /// Sincronizar_MarcaIn: Método para registrar las marcas de ingreso, salida y descanso de los colaboradores en la tabla Marcas_In 
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="marcasIn">Lista de registroS de la clase cMarcasIn</param>
        public Task<EventResponse> Sincronizar_MarcaIn(IEnumerable<cMarcaIn> marcasIn);

        /// <summary>
        /// GetMarcaExtraApb: Método para obtener todos los datos de la tabla Marcas_Extras_Apb 
        /// </summary>
        /// <returns>Lista de registros de la clase cMarcaExtraApb</returns>
        public Task<List<cMarcaExtraApb>> GetMarcaExtraApb();

        /// <summary>
        /// GetMarcaExtraApb: Método para obtener un registro de la tabla Marcas_Extras_Apb con un identificador específico
        /// </summary>
        /// <returns>Registro de la clase Marcas_Extras_Apb</returns>
        /// <param name="idregistro">Número identificador del registro</param>
        public Task<cMarcaExtraApb> GetMarcaExtraApb(long idregistro);

        /// <summary>
        /// Sincronizar_MarcaExtraApb: Método para registrar las marcas de horas extras de los colaboradores en las tablas Marcas_Extras_Apb y Marcas_Proceso
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="marcasExtraApb">Lista de registros de la clase cMarcaExtraApb</param>
        public Task<EventResponse> Sincronizar_MarcaExtraApb(IEnumerable<cMarcaExtraApb> marcasExtraApb);
    }
}
