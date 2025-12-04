using com.gsitcr.geotime.Models;
using com.gsitcr.geotime.Models.Response;
using static com.gsitcr.geotime.Models.CalculoPeriodoParam;

namespace com.gsitcr.geotime.Data.Interfaz
{
    public interface IGeoTimeConnectService
    {
        /* Metodos SQL */
        #region SQLMetodes
        public Task<IEnumerable<cPh_Login>> GetPhLogin();
        /// <summary>
        /// GetPhLogin: Método para obtener un usuario de ph_login segun su cuenta de correo
        /// </summary>
        /// <returns>Una instancia de la clase cPhLogin</returns>
        /// ///<param name="id">Id del usuario requerido</param>
        public Task<cPh_Login> GetPhLogin(string id);
        /// <summary>
        /// GetPhLoginByUsuario: Método para obtener un ph_login por nombre de usuario
        /// </summary>
        /// <returns>Una instancia de la clase cPhLogin</returns>
        /// ///<param name="id">Id del usuario requerido</param>
        public Task<cPh_Login> GetPhLoginByUsuario(string id);

        /// <summary>
        /// GetPhLoginById: Método para obtener un ph_login por nombre de usuario
        /// </summary>
        /// <returns>Una instancia de la clase cPhLogin</returns>
        /// ///<param name="id">Id del usuario requerido</param>
        public Task<cPh_Login> GetPhLoginById(int id);

        /// <summary>
        /// PutPhLogin: metodo para actualizar campos de filtros del ph_login
        /// </summary>
        /// <param name="phLogin"></param>
        /// <returns>Instancia de eventresponse con el resultado de la operación</returns>
        public Task<EventResponse> PutPhLogin(cPh_Login phLogin);

        public Task<EventResponse> Elimina_PhLogin(int id);

        public Task<List<cPh_Compania>> GetPhCompania();
        public Task<cPh_Compania> GetPhCompania(string idcomp);

        /// <summary>
        /// PutPhCompania: metodo para actualizar campos de la api en todos los registros de ph_compania
        /// </summary>
        /// <param name="phCompanias"></param>
        /// <returns>EventResponse, con el resultado del proceso</returns>
        public Task<EventResponse> PutPhCompania(cPh_Compania phCompanias);

        /// <summary>
        /// Sincronizar_PhCompania: metodo para sincronizar las compañias 
        /// </summary>
        /// <param name="phCompanias"></param>
        /// <returns>una instancia EventResponse con el resultado de la operacion</returns>
        public Task<EventResponse> Sincronizar_PhCompania(IEnumerable<cPh_Compania> phCompanias);

        /// <summary>
        /// Sincronizar_PhCompania: metodo para sincronizar la compañia inicial 
        /// </summary>
        /// <param name="compania"></param>
        /// <returns>una instancia EventResponse con el resultado de la operacion</returns>
        public Task<EventResponse> Sincronizar_PhCompaniaInicial(cPh_Compania compania);
        public Task<IEnumerable<cPh_Planilla>> GetPhPlanilla();
        public Task<cPh_Planilla> GetPhPlanilla(string idplanilla);
        public Task<cPh_Planilla> GetPhPlanilla(string nomConector, string descPlanilla);
        public Task<EventResponse> Sincronizar_PhPlanilla(IEnumerable<cPh_Planilla> PhPlanillas);
        public Task<List<cPh_Planilla>> GetPhPlanillaByUsuario(string idUsuario);
        public Task<EventResponse> Elimina_PhPlanilla(string idplanilla);
        /// <summary>
        /// GetTipo_Planilla: Método para obtener una lista de planillas 
        /// </summary>
        /// <returns>Lista de cTipo_Planilla</returns>
        public Task<List<cTipo_Planilla>> GetTipo_Planilla();
        /// <summary>
        /// GetDepartamento: obtener lista de departamentos
        /// </summary>
        /// <returns>Lista de departamentos</returns>
        public Task<List<cDepartamento>> GetDepartamento();
        public Task<cDepartamento> GetDepartamento(string idDepart);
        public Task<EventResponse> Sincronizar_Departamento(IEnumerable<cDepartamento> departamentos);
        public Task<EventResponse> Elimina_Departamento(string id);
        public Task<List<cPh_Grupo>> GetGrupo();
        public Task<cPh_Grupo> GetGrupo(int idgrupo);
        public Task<EventResponse> Sincronizar_Grupo(IEnumerable<cPh_Grupo> PhGrupos);
        public Task<EventResponse> Elimina_Grupo(int idgrupo);
        public Task<List<cPh_Grupo>> GetPhGrupoByUsuario(string idUsuario);
        /// <summary>
        /// GetEmpleado: Método para obtener una lista de empleados 
        /// </summary>
        /// <returns>Lista de cEmpleados</returns>
        public Task<List<cEmpleado>> GetEmpleado();

        /// <summary>
        /// GetEmpleadoProgramador: Método para obtener una lista de empleados para programadores en procesos
        /// </summary>
        /// <param name="grupos"></param>
        /// <returns></returns>
        public Task<List<cEmpleado>> GetEmpleadoProgramador(string grupos);

        /// <summary>
        /// GetEmpleadoProgramador: Método para obtener una lista de empleados para programadores en procesos
        /// </summary>
        /// <returns>Lista de cEmpleados</returns>
        public Task<List<cEmpleado>> GetEmpleadoProgramador(string idplanilla, string grupos);

        //Creado por: Marlon Loria
        //Fecha: 2025-08-26
        /// <summary>
        /// GetEmpleadoProgramadorByHorario: Método para obtener una lista de empleados asociados a un horarios y una planilla
        /// </summary>
        /// <returns>Lista de cEmpleados</returns>
        public Task<List<cEmpleado>> GetEmpleadoProgramadorByHorario(string idplanilla, string horarios);

        //Creado por: Marlon Loria Solano
        //Fecha: 2022-10-30
        /// <summary>
        /// GetEmpleadoTotal: Método para obtener la lista total de empleados 
        /// </summary>
        /// <returns>Lista de cEmpleados</returns>
        public Task<List<cEmpleado>> GetEmpleadoTotal();
        public Task<cEmpleado> GetEmpleadoByEmail(string email);

        /// <summary>
        /// GetEmpleado: Método para un empleado específico
        /// </summary>
        /// <returns>Una instancia de la clase cEmpleado</returns>
        /// ///<param name="idNumero">idNumero del empleado requerido</param>
        public Task<cEmpleado> GetEmpleado(string idNumero);
        public Task<List<cEmpleado>> GetEmpleadoFiltrado(string idnumero, string nombre, string iddepartamento);
        public Task<EventResponse> Sincronizar_Empleado(IEnumerable<cEmpleado> empleados);
        public Task<EventResponse> Elimina_Empleado(string idnumero);
        public Task<List<cTurno>> GetTurno();
        public Task<cTurno> GetTurno(int idTurno);
        public Task<EventResponse> Sincronizar_Turno(IEnumerable<cTurno> phTurno);
        /// <summary>
        /// Elimina_Turno:  Metodo borrado de datos de la tabla ph_Turnos
        /// </summary>
        /// <param name="idTurno"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Elimina_Turno(int idTurno);

        /// <summary>
        /// GetHorarios: Método para obtener una lista de registros de la tabla ph_horarios
        /// </summary>
        /// <returns>Un horario</returns>
        public Task<List<cPh_Horarios>> GetHorarios();
        /// <summary>
        /// GetHorarios: Obtener varios registros de la tabla ph_horarios
        /// </summary>
        /// <returns>Lista de horario</returns>
        public Task<cPh_Horarios> GetHorarios(int IDHORARIO);

        /// <summary>
        /// Sincronizar_Horarios: Método para registrar los horarios en la tabla ph_horarios
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="Horarios">Lista de registros de la clase cPh_horarios</param>
        public Task<EventResponse> Sincronizar_Horarios(IEnumerable<cPh_Horarios> Horarios);

        /// <summary>
        /// Elimina_Horarios:  Metodo borrado de datos de la tabla ph_horarios
        /// </summary>
        /// <param name="IDHORARIO"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Elimina_Horarios(string IDHORARIO);

        /// <summary>
        /// GetHorario_Turno: Método para obtener los registros de la tabla ph_horarios_turnos
        /// </summary>
        /// <returns>Listas de horario turno</returns>
        public Task<List<cPh_HorarioTurno>> GetHorario_Turno();

        /// <summary>
        /// GetHorario_Turno: Método para obtener los registros de la tabla ph_horarios_turnos
        /// </summary>
        /// <returns>Un horario turno</returns>
        public Task<cPh_HorarioTurno> GetHorario_Turno(int IDHORARIO);

        /// <summary>
        /// Sincronizar_Horario_Turno: Método para registrar los horarios en la tabla ph_horario_turno
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="Horario_Turno">Lista de registros de la clase cPh_horario_turno</param>
        public Task<EventResponse> Sincronizar_HorarioTurno(IEnumerable<cPh_HorarioTurno> Horario_Turno);

        /// <summary>
        /// Elimina_Horario_Turno:  Metodo borrado de datos de la tabla ph_horario_turno
        /// </summary>
        /// <param name="IDHORARIO"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Elimina_Horario_Turno(string IDHORARIO);

        /// <summary>
        /// GetPhRol: Método para obtener una lista de Roles 
        /// </summary>
        /// <returns>Lista de cPh_Rol</returns>
        public Task<List<cPh_Rol>> GetPhRol();

        /// <summary>
        /// GetPhRol: Método para obtener los registros de la tabla ph_Roles
        /// </summary>
        /// <returns>Un horario turno</returns>
        public Task<cPh_Rol> GetPhRol(int idrol);

        /// <summary>
        /// Sincronizar_PhRol: Método para registrar los Roles en la tabla ph_Roles
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="Roles">Lista de registros de la clase cPh_Roles</param>
        public Task<EventResponse> Sincronizar_PhRol(IEnumerable<cPh_Rol> phRol);

        /// <summary>
        /// Elimina_PhRol:  Metodo borrado de datos de la tabla ph_Roles
        /// </summary>
        /// <param name="idrol"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Elimina_PhRol(int idrol);

        public Task<List<cIncidencia>> GetIncidencia();
        public Task<cIncidencia> GetIncidencia(int id);
        public Task<EventResponse> Sincronizar_Incidencia(IEnumerable<cIncidencia> incidencias);

        /// <summary>
        /// Elimina_Incidencia:  Metodo borrado de datos de la tabla Incidencias
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Elimina_Incidencia(int id);

        /// <summary>
        /// GetPaletaColor: obtiene lista de colores de la paleta
        /// </summary>
        /// <returns>lista de colores de la paleta</returns>
        public Task<List<cPaletaColor>> GetPaletaColor();
        /// <summary>
        /// GetPaletaColor: obtiene un registro de la paleta de colores
        /// </summary>
        /// <param name="colorId">id de color a recuperar</param>
        /// <returns></returns>
        public Task<cPaletaColor> GetPaletaColor(int colorId);
        /// <summary>
        /// Sincronizar_PaletaColor: metodo para sincronizar lista de colores en la Paleta de Colores 
        /// </summary>
        /// <param name="colores"></param>
        /// <returns>una instancia EventResponse con el resultado de la operacion</returns>
        public Task<EventResponse> Sincronizar_PaletaColor(IEnumerable<cPaletaColor> colores);
        /// <summary>
        /// Elimina_PaletaColor:  Metodo borrado de datos de la tabla PaletaColores
        /// </summary>
        /// <param name="colorId"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Elimina_PaletaColor(int colorId);

        /// <summary>
        /// GetNivel: obtiene lista de niveles
        /// </summary>
        /// <returns>lista de Niveles</returns>
        public Task<List<cPh_Nivel>> GetNivel();
        /// <summary>
        /// GetNivel: obtiene un registro de la nivel
        /// </summary>
        /// <param name="IdNivel">id de nivel</param>
        /// <returns></returns>
        public Task<cPh_Nivel> GetNivel(int IdNivel);
        /// <summary>
        /// Sincronizar_Nivel: metodo para sincronizar lista de Niveles
        /// </summary>
        /// <param name="niveles"></param>
        /// <returns>una instancia EventResponse con el resultado de la operacion</returns>
        public Task<EventResponse> Sincronizar_Nivel(IEnumerable<cPh_Nivel> niveles);
        /// <summary>
        /// Elimina_Nivel:  Metodo borrado de datos de la tabla Ph_Niveles
        /// </summary>
        /// <param name="IdNivel"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Elimina_Nivel(int IdNivel);

        public Task<List<cMarcaDistribucionConcepto>> GetMarcaDtnConcepto();
        public Task<cMarcaDistribucionConcepto> GetMarcaDtnConcepto(int idregistro);
        public Task<cMarcaDistribucionConcepto> GetMarcaDtnConcepto(string idnumero, string fecha, string idturno);

        /// <summary>
        /// GetMarcaDtnConcepto: Obtener lista de Marcas_Distribuciones_Conceptos
        /// </summary>
        /// <param name="idnumero"></param>
        /// <param name="fecha"></param>
        /// <param name="idplanilla"></param>
        /// <returns>lista de Marcas_Distribuciones_Conceptos</returns>
        public Task<IEnumerable<cMarcaDistribucionConcepto>> GetMarcaDtnConcepto(string idplanilla, string fechaInicio, string fechaFinal, string idnumero);

        public Task<EventResponse> Sincronizar_MarcaDtnConcepto(IEnumerable<cMarcaDistribucionConcepto> marcasDtnConcepto);

        /// <summary>
        /// Elimina_MarcaDtnConcepto:  Metodo borrado de datos de la tabla Marcas Distribucion Conceptos
        /// </summary>
        /// <param name="idregistro"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Elimina_MarcaDtnConcepto(long idregistro);

        /// <summary>
        /// GetTemplateHID: obtiene lista de TemplateHID (huellas de colaboradores HID)
        /// </summary>
        /// <returns>lista de TemplateHID</returns>
        public Task<List<cTemplateHID>> GetTemplateHID();

        /// <summary>
        /// GetTemplateHID: obtiene una lista de registro de TemplateHID para un empleado especifico
        /// </summary>
        /// <param name="idnumero">id de color a recuperar</param>
        /// <returns></returns>
        public Task<List<cTemplateHID>> GetTemplateHID(string idnumero);

        /// <summary>
        /// Sincronizar_TemplatesHID: metodo para sincronizar lista de TemplateHID
        /// </summary>
        /// <param name="templates"></param>
        /// <returns>una instancia EventResponse con el resultado de los TemplateHID</returns>
        public Task<EventResponse> Sincronizar_TemplateHID(IEnumerable<cTemplateHID> templates);
        /// <summary>
        /// Elimina_TemplateHID:  Metodo borrado de datos de la tabla Ph_Niveles para un empleado especifico
        /// </summary>
        /// <param name="IdNumero"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Elimina_TemplateHID(string IdNumero);

        /// <summary>
        /// Verifica_TemplatesHID: metodo para verificar TemplateHID en lista de TemplateHID registrados 
        /// </summary>
        /// <param name="template"></param>
        /// <returns>una instancia EventResponse con el resultado de los TemplateHID</returns>
        public Task<EventResponseHID> Verifica_TemplateHID(IEnumerable<cTemplateHID> template);



        /// <summary>
        /// GetTemplateFACE: obtiene lista de TemplateFACE (huellas de colaboradores FACE)
        /// </summary>
        /// <returns>lista de TemplateFACE</returns>
        public Task<List<cTemplateFACE>> GetTemplateFACE();

        /// <summary>
        /// GetTemplateFACE: obtiene una lista de registro de TemplateFACE para un empleado especifico
        /// </summary>
        /// <param name="idnumero">id de color a recuperar</param>
        /// <returns></returns>
        public Task<List<cTemplateFACE>> GetTemplateFACE(string idnumero);

        /// <summary>
        /// Sincronizar_TemplatesFACE: metodo para sincronizar lista de TemplateFACE
        /// </summary>
        /// <param name="templates"></param>
        /// <returns>una instancia EventResponse con el resultado de los TemplateFACE</returns>
        public Task<EventResponse> Sincronizar_TemplateFACE(IEnumerable<cTemplateFACE> templates);
        /// <summary>
        /// Elimina_TemplateFACE:  Metodo borrado de datos de la tabla Ph_Niveles para un empleado especifico
        /// </summary>
        /// <param name="IdNumero"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Elimina_TemplateFACE(string IdNumero);

        /// <summary>
        /// GetPhPuesto: Obtener lista de puestos de colaboradores
        /// </summary>
        /// <returns></returns>
        public Task<List<cPh_Puesto>> GetPhPuesto();
        /// <summary>
        /// GetPhPuesto: Obtener un puesto especifico de colaborador
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<cPh_Puesto> GetPhPuesto(string id);

        #endregion

        #region SPMetodos

        /// <summary>
        /// /Obtener lista de Compañias asociadas al usuario
        /// </summary>
        /// <returns>Lista de Companias de Usuario </returns>
        public Task<List<cPh_CompaniaUsuario>> GetPhCompaniaUsuario(string idnumero);
        public Task<EventResponse> ActivarPeriodoPAAsync(cActivarPeriodo parametros);
        public Task<EventResponse> CierroPeriodo(cCierroPeriodo parametros);

        /// <summary>
        /// EjecutaCalculoPlanillaEmpleado:  ejecuta calculo de planilla para el empleado indicado en el parametro
        /// </summary>
        /// <param name="calculo_periodo_param"></param>
        /// <returns>Intancia de eventresponse con el resultado de la ejecucion del proceso</returns>
        public Task<EventResponse> EjecutaCalculoPlanillaEmpleado(cCalculoPeriodoParam calculo_periodo_param);

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-05-24
        /// <summary>
        /// EjecutaCalculoPlanilla: Recibe una lista de MarcaMovTurno y a partir de ella realiza el calculo de planilla de forma temporal
        /// </summary>
        /// <param name="marcasMovTurnos">lista de MarcaMovTurno</param>
        /// <returns>EventResponse con resultado del proceso</returns>
        public Task<EventResponse> EjecutaCalculoPlanilla(IEnumerable<cMarcaMovTurno> marcasMovTurnos);


        #endregion

        #region WSMetodos
        public Task<EventResponse> Sincronizo_erp(IEnumerable<cSincronizo_erp> parametros);
        public Task<EventResponse> Sincronizo_Acciones(IEnumerable<cSincronizo_Acciones> parametros);
        /// <summary>
        /// EjecutaInitPeriodo:  Se ejecuta el WebService Init_Periodo.
        /// </summary>
        /// <param name="parametros">Recibe una instancia de cInit_Periodo</param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>
        public Task<EventResponse> Init_Periodo(IEnumerable<cInit_Periodo> parametros);
        public Task<EventResponse> Cal_Periodo_Planilla(IEnumerable<cCal_Periodo_Planilla> parametros);
        public Task<IEnumerable<cObtengoConcepto>> Obtener_Conceptos(string compania, string sesion);
        public Task<IEnumerable<cObtengoTipoAccion>> Obtener_TipoAccion(string compania, string sesion);
        /// <summary>
        /// Exporto_Concepto:  Se ejecuta el WebService Exporto_Concepto.
        /// </summary>
        /// <param name="parametros">Recibe los datos de cExporto_Concepto</param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>
        public Task<EventResponse> Exporto_Concepto(IEnumerable<cExporto_Concepto> parametros);
        /// <summary>
        /// EvaluaFormula:  Se ejecuta el WebService Evalua_Formula.
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public Task<EventResponse> EvaluaFormula(cPh_Formulacion formula);

        #endregion

        public Task<cAccionPersonal> GetAccionPersonal(long idregistro);
        public Task<List<cAccionPersonal>> GetAccionPersonal(string IdPlanilla, DateTime FechaInicio, DateTime FechaFin);
        public Task<List<cAccionPersonal>> GetAccionPersonal(string IdPlanilla, DateTime FechaInicio, DateTime FechaFin, string usuario);
        public Task<List<cAccionPersonal>> GetAccionPersonalPorEstado(string IdPlanilla, string usuario, char estado);
        public Task<List<cAccionPersonal>> GetAccionPersonalPorEstado(string IdPlanilla, string FechaInicio, string FechaFin, char estado, int idincidencia, string idgrupo);
        public Task<List<cAccionPersonal>> GetAccionPersonalPorEstado(string IdPlanilla, char estado);

        public Task<EventResponse> Sincronizar_AccionPersonal(IEnumerable<cAccionPersonal> accionPersonal);
        public Task<EventResponse> Sincronizar_AccionPersonalNomConector(IEnumerable<cAccionPersonal> accionPersonal);

        //Creado por: Marlon Loria Solano
        //Fecha: 2024-07-18
        /// <summary>
        /// Sincronizar_AccionPersonal_CA: Sincroniza acciones de personal provenientes de Autogestión.
        /// </summary>
        /// <param name="accionPersonal">Recibe una lista de Acciones de Personal y las crea en GeoTime</param>
        /// <returns>EventResponse: con el resultado de la operación</returns>
        public Task<EventResponse> Sincronizar_AccionPersonal_AutoGestion(IEnumerable<cAccionPersonal> accionPersonal);

        //Creado por: Marlon Loria Solano
        //Fecha: 2024-07-18
        /// <summary>
        /// Sincronizar_AccionPersonal_CA: Sincroniza acciones de personal provenientes de Control de Asistencia en Edicion de Marcas.  Se debe crear registro en marcas incidencias.
        /// </summary>
        /// <param name="accionPersonal">Recibe una lista de Acciones de Personal y las crea en GeoTime</param>
        /// <returns>EventResponse: con el resultado de la operación</returns>
        public Task<EventResponse> Sincronizar_AccionPersonal_PreJustificacion(IEnumerable<cAccionPersonal> accionPersonal);

        //Creado por: Marlon Loria Solano
        //Fecha: 2024-07-18
        /// <summary>
        /// Sincronizar_AccionPersonal_CA: Sincroniza acciones de personal provenientes de Control de Asistencia en Mantenimientos de Acciones.  No se deben aplicar
        /// </summary>
        /// <param name="accionPersonal">Recibe una lista de Acciones de Personal y las crea en GeoTime</param>
        /// <returns>EventResponse: con el resultado de la operación</returns>
        public Task<EventResponse> Sincronizar_AccionPersonal_CA(IEnumerable<cAccionPersonal> accionPersonal);

        //Creado por: Allan Prieto Badilla
        //Fecha: 2024-08-08
        /// <summary>
        /// Sincronizar_AccionPersonal_CAUpdate: Sincroniza acciones de personal provenientes de Control de Asistencia en Mantenimientos de Acciones. Solo cambio de Estado
        /// </summary>
        /// <param name="accionPersonal">Recibe una lista de Acciones de Personal y las crea en GeoTime</param>
        /// <returns>EventResponse: con el resultado de la operación</returns>
        public Task<EventResponse> Sincronizar_AccionPersonal_CAUpdate(IEnumerable<cAccionPersonal> accionPersonal);

        public Task<EventResponse> Elimina_AccionPersonal(Int64 id);

        public Task<List<cCentroCosto>> GetCentroCosto();
        public Task<cCentroCosto> GetCentroCosto(string idCCosto);
        public Task<EventResponse> Sincronizar_Centro_Costo(IEnumerable<cCentroCosto> centrosCosto);
        public Task<List<cConcepto>> GetConcepto();
        public Task<cConcepto> GetConcepto(string concepto);
        public Task<EventResponse> Sincronizar_Concepto(IEnumerable<cConcepto> conceptos);
        public Task<EventResponse> Elimina_Concepto(int id);
 
        
        public Task<cIncidencia> GetIncidenciaByNomConector(string nom_conector);
        public Task<List<cIncidencia>> GetIncidenciaReqAccPer();

        public Task<List<cMarcaResumen>> GetMarcasResumen();
        public Task<List<cMarcaResumen>> GetMarcasResumen(string idPlanilla);
        public Task<List<cMarcaResumen>> GetMarcasResumen(string idPlanilla, string idPeriodo);

        /// <summary>
        /// GetMarcasResumen:  Proceso para determinar resumen de marcas para el periodo que se deben visualizar en el sistema
        /// </summary>
        /// <param name="IdPlanilla">id de planilla por el que se debe filtrar la informacion</param>
        /// <param name="idperiodo">periodo del reporte</param>
        /// <param name="idnumero">id del empleado que se desea obtener, si se envia un -1 trae todos los empleados</param>
        /// <returns>Lista de marcas resumen del periodo</returns>
        public Task<IEnumerable<cMarcaResumen>> GetMarcasResumen(string IdPeriodo, string IdPlanilla, string idnumero);

        public Task<List<cMarcaResumen>> GetMarcasResumenXPeriodo(string idPeriodo);

        /// <summary>
        /// GetMarcasResumenTransferir:   Proceso para determinar resumen de marcas para la planilla y el periodo que se deben enviar en al ERP
        /// </summary>
        /// <param name="idPlanilla"></param>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public Task<List<cMarcaResumen>> GetMarcasResumenATransferir(string idPlanilla, string idPeriodo);
        public Task<EventResponse> Sincronizar_MarcasResumen(IEnumerable<cMarcaResumen> marcasResumen);


        public Task<List<cMarca>> GetMarcas();
        public Task<List<cMarca>> GetMarcas(string idnumero);

        /// <summary>
        /// GetMarcas: Obtener las marcas de un empleado para el periodo activo
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <returns>Lista de Marcas del periodo</returns>
        public Task<List<cMarca>> GetMarcas(string idnumero, string fecha);

        /// <summary>
        /// GetMarcasDiaria: Obtener las marcas del dia para un empleado 
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha del dia</param>
        /// <returns>Lista de Marcas del dia</returns>
        public Task<List<cMarca>> GetMarcasDiaria(string idnumero, string fecha);
        public Task<EventResponse> Sincronizar_Marca(IEnumerable<cMarca> marcas);

        /* Trea una marca para valdar la hora marcada en el programador Turno */
        public Task<cMarca> GetMarcaProgramador(string idnumero, string idplanilla, string fecha);
        public Task<EventResponse> ValidarClaveEmpleado(cLogin login);

        /// <summary>
        /// ValidarClaveAdm: Validar Clave de Usuario Admin.  Recibe una instancia de empleado, se verifica si existe y se valida contraseña indicada contra la registrada en la base de datos
        /// </summary>
        /// <param name="login">datos del usuario a validar</param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>
        public Task<EventResponse> ValidarClaveAdm(cLogin login);

        // <summary>
        /// ValidarLicencia: validar datos de la licencia
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public Task<EventResponse> ValidarLicencia(cLogin login);
        public Task<EventResponse> CambiarClaveEmpleado(cEmpleado empleado);


        public Task<List<cMarcaMovTurno>> GetMarcaMovTurno();
        public Task<cMarcaMovTurno> GetMarcaMovTurno(int idregistro);
        public Task<cMarcaMovTurno> GetMarcaMovTurno(string idnumero, string fecha, int idturno);
        public Task<List<cMarcaMovTurno>> GetMarcaMovTurno(string idplanilla, string estado, string fechaInicio, string fechaFinal);

        /// <summary>
        /// GetMarcaMovTurno: Método para obtener una lista de Marcas Mov Turnos por empleado 
        /// </summary>
        /// <returns>Lista de cMarcaMovTurno</returns>
        /// <param name="idnumero">Número de Empleado</param>
        /// <param name="fechaPeriodo">Fecha del Periodo para el cual se requieren los turnos</param>
        public Task<List<cMarcaMovTurno>> GetMarcaMovTurno(string idnumero, string fechaPeriodo);

        /// <summary>
        /// GetMarcaMovTurnoByGrupo: Método para obtener una lista de Marcas Mov Turnos de los empleados asignados a un supervisor 
        /// </summary>
        /// <returns>Lista de cMarcaMovTurno</returns>
        /// <param name="fechaPeriodo">Fecha del Periodo para el cual se requieren los turnos</param>
        /// <param name="idgrupo">grupo de empleado</param>
        public Task<List<cMarcaMovTurno>> GetMarcaMovTurnoByGrupo(string fechaPeriodo, string idgrupo);
        public Task<EventResponse> Sincronizar_MarcasMovTurnos(IEnumerable<cMarcaMovTurno> marcasMovTurnos);

        /// <summary>
        /// PutMarcasMovTurnos: actualizar Marcas_MOv_Turnos, Recibe una instancia de MarcaMovTurno, se verifica si existe en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="marcaMovTurno"></param>
        /// <returns>Instancia de eventresponse con el resultado del proceso</returns>
        public Task<EventResponse> PutMarcasMovTurnos(cMarcaMovTurno marcaMovTurno);

        /// <summary>
        /// PutMarcasProceso: Recibe una lista de MarcaMovTurno y a partir de ella realiza la actualizacion del turno en marcas Proceso
        /// </summary>
        /// <param name="marcasMovTurnos">lista de MarcaMovTurno</param>
        /// <returns>EventResponse con resultado del proceso</returns>
        public Task<EventResponse> PutMarcasProceso(IEnumerable<cMarcaMovTurno> marcasMovTurnos);

        public Task<List<cMarcaMovHorario>> GetMarcaMovHorario();
        public Task<cMarcaMovHorario> GetMarcaMovHorario(int idregistro);
        public Task<List<cMarcaMovHorario>> GetMarcaMovHorario(string idplanilla, string estado, string fechaInicio, string fechaFinal);
        public Task<EventResponse> Sincronizar_MarcasMovHorario(IEnumerable<cMarcaMovHorario> marcasMovHorarios);


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
        /// getPeriodo: Método para obtener los periodos della proyeccion anterior 
        /// </summary>
        /// <returns>Lista de cPh_Periodos</returns>
        /// <param name="idperiodo">Fecha del periodo</param>
        /// <param name="proyeccion">Fecha del periodo</param>
        /// <param name="vigente">Periodo está vigente </param>
        public Task<IEnumerable<cPh_Periodos>> GetPeriodo(string idperiodo, string proyeccion, string vigente);


        /// <summary>
        /// GetPeriodoVigenteEmpleado: Método para obtener el periodo vigenta para un empleado  
        /// </summary>
        /// <returns>Un item de cPh_Periodos</returns>
        /// <param name="fecha">Fecha del periodo</param>
        /// <param name="idnumero">Número de empleado</param>
        public Task<cPh_Periodos> GetPeriodoVigenteEmpleado(string idnumero, string fechaPeriodo);

        /// <summary>
        /// GetPeriodoVigenteUsuario: Método para obtener lista de periodos vigentes para un usuario  
        /// </summary>
        /// <returns>Una lista de cPh_Periodos vigentes</returns>
        /// <param name="fecha">Fecha del periodo</param>
        /// <param name="idusuario">id de usuario</param>
        public Task<IEnumerable<cPh_Periodos>> GetPeriodoVigenteUsuario(int idusuario, string fechaPeriodo);

        public Task<EventResponse> Sincronizar_Periodo(IEnumerable<cPh_Periodos> periodos);
        public Task<EventResponse> Elimina_Periodo(string id);



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
        /// GetMarcaExtraApb: Método para obtener todos los datos de la tabla Marcas_Extras_Apb para un empleado y un periodo especifico
        /// </summary>
        /// <param name="idnumero">id del empleado</param>
        /// <param name="fecha">fecha para determinar el periodo vigente</param>
        /// <param name="idplanilla">id planilla</param>
        /// <param name="PeriodoVigente">Si es verdadero trae marcas extras del periodo, sino, trae marcas del dia</param>
        /// <returns>Lista de registros de la clase cMarcaExtraApb</returns>
        public Task<List<cMarcaExtraApb>> GetMarcaExtraApb(string idnumero, string fecha, string idplanilla, bool byPeriodo);

        /// <summary>
        /// GetMarcaExtraApb: Método para obtener un registro de la tabla Marcas_Extras_Apb con un identificador específico
        /// </summary>
        /// <returns>Registro de la clase Marcas_Extras_Apb</returns>
        /// <param name="idregistro">Número identificador del registro</param>
        public Task<cMarcaExtraApb> GetMarcaExtraApb(long idregistro);

        /// <summary>
        /// GetMarcaExtraApb: Método para obtener una lista de Horas Extras pendientes de Aprobación de los empleados asignados a un supervisor 
        /// </summary>
        /// <returns>Lista de cMarcaExtraApb</returns>
        /// <param name="fechaPeriodo">Fecha del Periodo para el cual se requieren las extras</param>
        /// <param name="idgrupo">grupo de empleado</param>
        public Task<List<cMarcaExtraApb>> GetMarcaExtraApb(string fechaPeriodo, string idgrupo);

        /// <summary>
        ///  GetMarcaExtraApb: Método para obtener una lista de Horas Extras en un rango de fechas para empleados de los grupos indicados en los parametros 
        /// </summary>
        /// <param name="idsgrupos"></param>
        /// <param name="idplanilla"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns>Lista de marcas por horas extras del periodo</returns>
        public Task<List<cMarcaExtraApb>> GetMarcaExtraApb(string idsgrupos, string idplanilla, string fechaInicio, string fechaFinal, char estado);

        /// <summary>
        /// Sincronizar_MarcaExtraApb: Método para registrar las marcas de horas extras de los colaboradores en las tablas Marcas_Extras_Apb y Marcas_Proceso
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="marcasExtraApb">Lista de registros de la clase cMarcaExtraApb</param>
        public Task<EventResponse> Sincronizar_MarcaExtraApb(IEnumerable<cMarcaExtraApb> marcasExtraApb);

        //Creado por: Marlon Loria Solano
        //Fecha: 2023-08-10
        /// <summary>
        /// Autorizar_MarcaExtraApb: Método para autorizar las marcas de horas extras de los colaboradores en las tablas Marcas_Extras_Apb y Marcas_Proceso
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="marcasExtraApb">Lista de registros de la clase cMarcaExtraApb</param>
        public Task<EventResponse> Autorizar_MarcaExtraApb(IEnumerable<cMarcaExtraApb> marcasExtraApb);

        /// <summary>
        /// GetProyecto: Obtener lista de Proyectos
        /// </summary>
        /// <returns>Lista de objetos del tipo cPh_Proyecto</returns>

        public Task<List<cPh_Proyecto>> GetProyecto();

        /// <summary>
        /// GetProyectoByCCosto: Obtener lista de Proyectos asociados a un Centro de Costo
        /// </summary>
        /// <param name="idccosto"></param>
        /// <returns></returns>
        public Task<List<cPh_Proyecto>> GetProyectoByCCosto(string idccosto);

        /// <summary>
        /// GetFaseProyecto: Obtener un Proyecto especifica
        /// </summary>
        /// <param name="idproyecto"> identificador del proyecto</param>
        /// <returns>Una Instancia del objeto del tipo cPh_Proyecto</returns>
        public Task<cPh_Proyecto> GetProyecto(string idproyecto);

        /// <summary>
        /// Sincronizar_Proyectos: Sincronizar proyectos, se verifica si existe el elemento, en cuyo caso actualiza el registro, de lo contrario se crea.
        /// </summary>
        /// <param name="phProyectos">Recibe una instancia del tipo cProyecto</param>
        /// <returns>Una instancia del tipo EventResponse con las respuesta del proceso</returns>.
        public Task<EventResponse> Sincronizar_Proyectos(IEnumerable<cPh_Proyecto> phProyectos);

        /// <summary>
        /// GetFaseProyecto: Obtener lista de Fases de Proyectos
        /// </summary>
        /// <returns>Una lista de objetos del tipo cPh_FaseProyecto</returns>
        public Task<List<cPh_FaseProyecto>> GetFaseProyecto();

        /// <summary>
        /// GetFaseProyecto: Obtener lista de Fase de Proyecto para un proyecto.
        /// </summary>
        /// <param name="idproyecto"> identificador del proyecto</param>
        /// <returns>Una Instancia del objeto del tipo cPh_FaseProyecto</returns>
        public Task<List<cPh_FaseProyecto>> GetFaseProyecto(string idproyecto);

        /// <summary>
        /// GetFaseProyecto: Obtener una Fase de Proyecto especifica
        /// </summary>
        /// <param name="idproyecto"> identificador del proyecto</param>
        /// <param name="fase">fase del proyecto</param>
        /// <returns>Una Instancia del objeto del tipo cPh_FaseProyecto</returns>
        public Task<cPh_FaseProyecto> GetFaseProyecto(string idproyecto, string fase);

        /// <summary>
        /// Sincronizar_FaseProyectos: Sincronizar fases de proyectos, se verifica si existe el elemento, en cuyo caso actualiza el registro, de lo contrario se crea.
        /// </summary>
        /// <param name="phFaseProyectos">Recibe una instancia del tipo cFaseProyecto</param>
        /// <returns>Una instancia del tipo EventResponse con las respuesta del proceso</returns>
        public Task<EventResponse> Sincronizar_FaseProyectos(IEnumerable<cPh_FaseProyecto> phFaseProyectos);

        /// <summary>
        /// GetMarcas: Obtener las marcas proceso para el periodo 
        /// </summary>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <returns>Lista de Marcas del periodo</returns>

        public Task<List<cMarcaProceso>> GetMarcasProceso(string fecha);

        /// <summary>
        /// GetMarcas: Obtener las marcas proceso de un empleado para el periodo 
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <returns>Lista de Marcas del periodo</returns>
        public Task<List<cMarcaProceso>> GetMarcasProceso(string idnumero, string fecha);
        public Task<List<cMarcaProceso>> GetMarcasProceso(string IdPlanilla, string FechaInicio, string FechaFin, string idgrupo);

        /// <summary>
        /// GetMarcasProcesoHorasExtrasPendientes: Marcas Proceso con detalle de Horas Extras Pendientes
        /// </summary>
        /// <param name="IdPlanilla"></param>
        /// <param name="FechaInicio"></param>
        /// <param name="FechaFin"></param>
        /// <returns></returns>
        public Task<List<cMarcaProceso>> GetMarcasProcesoHorasExtrasPendientes(string IdPlanilla, string FechaInicio, string FechaFin);

        /// <summary>
        /// GetMarcasTiempoAdicional: Obtener marca tiempo adicional 
        /// </summary>
        /// <param name="IdRegistro">Planilla</param>
        /// <returns>Un registro en particular de Marca Tiempo Adicional</returns>
        public Task<cMarcaTiempoAdicional> GetMarcasTiempoAdicional(long IdRegistro);


        /// <summary>
        /// GetMarcasTiempoAdicional: Obtener las marcas tiempo adicional 
        /// </summary>
        /// <param name="IdPlanilla">Planilla</param>
        /// <param name="idPeriodo">Planilla</param>
        /// <param name="Fecha">fecha para determinar periodo</param>
        /// <param name="idconcepto">fecha para determinar periodo</param>
        /// <param name="idgrupo">Grupos a lo que pertenecen los empleados</param>
        /// <returns>Lista de Marcas Tiempos adicionales</returns>
        public Task<List<cMarcaTiempoAdicional>> GetMarcasTiempoAdicional(string IdPlanilla, string idPeriodo, string FechaInicio, string FechaFin, int idconcepto, string idgrupo);

        /// <summary>
        /// GetMarcasTiempoAdicional: Obtener las marcas tiempo adicional para un empleado, planilla y un periodo especifico
        /// </summary>
        /// <param name="IdPlanilla"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="idnumero"></param>
        /// <returns></returns>
        public Task<List<cMarcaTiempoAdicional>> GetMarcasTiempoAdicional(string IdPlanilla, string idPeriodo, string idnumero);

        /// <summary>
        /// Sincronizar_MarcasTiempoAdicional: Sincroniza las marcas tiempo adicional
        /// </summary>
        /// <param name="marcasTiempoAdicional"></param>
        /// <returns></returns>
        public Task<EventResponse> Sincronizar_MarcasTiempoAdicional(IEnumerable<cMarcaTiempoAdicional> marcasTiempoAdicional);

        /// <summary>
        /// Elimina_MarcasTiempoAdicional: Elimina un registro de Marca Tiempo Adicional
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<EventResponse> Elimina_MarcasTiempoAdicional(int id);

        /// <summary>
        /// GetMarcasAudit: Obtener las marcas_audit para un empleado, planilla y un periodo especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <returns>Lista de Marcas Audit</returns>

        public Task<List<cMarcaAudit>> GetMarcasAudit(string idnumero, string fecha, string idplanilla);


        /// <summary>
        /// GetMarcasAudit: obtiene las modificaciones a las marcas realizadas en el sistema
        /// </summary>
        /// <param name="idnumero"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idplanilla"></param>
        /// <returns></returns>
        public Task<List<cMarcaAudit>> GetMarcasAudit(string idnumero, string fechaInicio, string fechaFinal, string idplanilla);

        // <summary>
        /// GetMarcasAudit: Obtener las Marcas Descansos para un empleado, planilla y un periodo especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <returns>Lista de Marcas Descansos</returns>

        public Task<List<cMarcaDescanso>> GetMarcasDescansos(string idnumero, string fecha, string idplanilla);

        /// <summary>
        /// GetAccionPersonalPorPeriodo: Acciones de Personal 
        /// </summary>
        /// <param name="IdPlanilla"></param>
        /// <param name="usuario"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        //Obtener lista de Acciones de Personal
        public Task<List<cAccionPersonal>> GetAccionPersonalPorPeriodo(string idnumero, string fecha, string IdPlanilla);


        /// <summary>
        /// GetMarcaIncidencia: Obtener una Marca Incidencia especifica segun el id indicado en el parámetro
        /// </summary>
        /// <param name="id">numero de marca incidencia</param>
        /// <returns>Una instancia de Marcas Incidencias</returns>
        public Task<cMarcaIncidencia> GetMarcaIncidencia(long id);

        /// <summary>
        /// GetMarcasIncidencias: Obtener las Marcas Incidencias para un empleado, planilla y un periodo especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <returns>Lista de Marcas Incidencias</returns>
        public Task<List<cMarcaIncidencia>> GetMarcaIncidencia(string idnumero, string fecha, string idplanilla);       

        /// <summary>
        /// GetMarcaIncidencia: Obtener las Marcas Incidencias para un empleado, planilla y para un rango de fechas especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <param name="fechaInicio">Fecha de Inicio</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <returns>Lista de Marcas Incidencias</returns>

        public Task<List<cMarcaIncidencia>> GetMarcaIncidencia(string idnumero, string idplanilla, DateTime fechaInicio, DateTime fechaFinal);

        /// <summary>
        /// GetMarcaIncidencia: Obtener las Marcas Incidencias para un tipo planilla y un rango de fechas especifico
        /// </summary>
        /// <param name="idplanilla"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns>Lista de incidencias del periodo</returns>
        public Task<List<cMarcaIncidencia>> GetMarcaIncidenciaPeriodo(string idplanilla, string fechaInicio, string fechaFinal);

        /// <summary>
        /// GetMarcaIncidenciaPeriodo: Obtener las Marcas Incidencias para un tipo planilla y un rango de fechas especifico
        /// </summary>
        /// <param name="idplanilla"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idnumero"></param>
        /// <returns>Lista de incidencias del periodo</returns>
        public Task<List<cMarcaIncidencia>> GetMarcaIncidenciaPeriodo(string idplanilla, string fechaInicio, string fechaFinal, string idnumero);

        /// <summary>
        /// GetMarcaIncidenciaProgramador: Obtener las Marcas Incidencias para un empleado, planilla y un periodo especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <returns>Lista de Marcas Incidencias</returns>
        public Task<List<cMarcaIncidencia>> GetMarcaIncidenciaProgramador(string idnumero, string fecha, string idplanilla);


        /// <summary>
        /// GetPhUsuarioById: Obtener datos de usuario por su ID 
        /// </summary>
        /// <param name="id">id numero del empleado</param>
        /// <returns>Instancia de phusuario con los datos del usuario </returns>
        public Task<cPh_Usuario> GetPhUsuarioById(int id);

        /// <summary>
        /// GetPhUsuario: Obtener datos de usuario 
        /// </summary>
        /// <param name="idnumero">id numero del empleado</param>
        /// <returns>Instancia de phusuario con los datos del usuario </returns>
        public Task<cPh_Usuario> GetPhUsuario(string idnumero);

        /// <summary>
        /// PostPhUsuario: crear o actualizar registro de phUsuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Task<EventResponse> PostPhUsuario(cPh_Usuario usuario);

        /// <summary>
        /// PutPhUsuario: utilizado para actualizar variables globales del usuario para los filtros
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Task<EventResponse> PutPhUsuario(cPh_Usuario usuario);

        /// <summary>
        /// PutPhUsuario: utilizado para actualizar variables globales del usuario para los filtros
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Task<EventResponse> PutActualizarPhUsuario(cPh_Usuario usuario);

        /// <summary>
        /// GetPhSistema: Obtener datos de Sistema 
        /// </summary>
        /// <returns>Instancia de cPh_Sistema con los datos del sistema </returns>
        public Task<cPh_Sistema> GetPhSistema();

        /// <summary>
        /// GetPortalConfig: Obtener datos de Configuración del Portal 
        /// </summary>
        /// <returns>Instancia de cPortal_Config con los datos del sistema </returns>
        public Task<cPortal_Config> GetPortalConfig();

        /// <summary>
        /// Sincronizar_PortalConfig: Sincronizar las configuraciones del Portal de Empleados, se verifica si existe el elemento, en cuyo caso actualiza el registro, de lo contrario se crea.
        /// </summary>
        /// <param name="portalConfig">Recibe una instancia del tipo cPortal_Config</param>
        /// <returns>Una instancia del tipo EventResponse con las respuesta del proceso</returns>
        public Task<EventResponse> Sincronizar_PortalConfig(cPortal_Config portalConfig);

        /// <summary>
        /// GetPortalOpcion: Obtener lista de opciones del menu de Portal 
        /// </summary>
        /// <returns>Lista de Opciones del sistema</returns>
        public Task<List<cPortal_Opcion>> GetPortalOpcion();
        /// <summary>
        /// GetPortalOpcion: Obtener datos de una opcion de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPortal_Opcion con los datos de la opción </returns>
        public Task<cPortal_Opcion> GetPortalOpcion(string id);

        /// <summary>
        /// Sincronizar_MarcaExtraApb: Método para registrar las marcas de horas extras de los colaboradores en las tablas Marcas_Extras_Apb y Marcas_Proceso
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="marcasExtraApb">Lista de registros de la clase cMarcaExtraApb</param>
        public Task<EventResponse> Sincronizar_PortalOpcion(IEnumerable<cPortal_Opcion> portalOpcion);

        /// <summary>
        /// GetPhFormulacion: Obtener lista de registros de la tabla PH_FROMULACION
        /// </summary>
        /// <returns>Lista de cPh_Formulacion </returns>
        public Task<List<cPh_Formulacion>> GetPhFormulacion();

        /// <summary>
        /// GetPhFormulacion: Obtener datos de una opcion de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPortal_Opcion con los datos de la opción </returns>
        public Task<cPh_Formulacion> GetPhFormulacion(int id);

        /// <summary>
        /// Sincronizar_PhFormulacion: Método para registrar los registros en la tabla Ph_Formulacion
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="cPh_Formulacion">Lista de registros de la clase cPh_Formulacion</param>
        public Task<EventResponse> Sincronizar_PhFormulacion(IEnumerable<cPh_Formulacion> ph_Formulacion);

        /// <summary>
        /// Elimina_PhFormulacion:  Metodo boorado de datos de la tabla Ph_Formulacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Elimina_PhFormulacion(string id);

        /// <summary>
        /// CambiarCodigoSeguridadEmpleado: Actualizar codigo de seguridad, para cambios de contraseñas
        /// </summary>
        /// <param name="empleado">instancia de empleado a actualizar</param>
        /// <returns>instancia de EventResponse con el resultado de la operacion</returns>
        public Task<EventResponse> CambiarCodigoSeguridadEmpleado(cEmpleado empleado);


        public Task<cParametroEmail> GetParametroEmail(int id);
        public Task<EventResponse> Sincronizar_ParametroEmail(cParametroEmail parametroEmail);

        


        /// <summary>
        /// GetPh_Transformacion: Método para obtener una lista de tranformaciones 
        /// </summary>
        /// <returns>Lista de cPh_Transformacion</returns>
        public Task<List<cPh_Transformacion>> GetPhTransformacion();

        

        /// <summary>
        /// GetRolTurno: Método para la tabla Rol Turno
        /// </summary>
        /// <returns>Una instancia de la clase cPh_RolTurno</returns>
        /// ///<param name="idNumero">idNumero del empleado requerido</param>
        //public Task<List<cPh_RolTurno>> GetRolTurno(int idrol);

        /// <summary>
        /// Sincronizar_RolTurno: Método para registrar los Roles Turno en la tabla ph_roles_turnos
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="rolesTurno">Lista de registros de la clase cPh_horario_turno</param>
        public Task<EventResponse> Sincronizar_RolTurno(IEnumerable<cPh_RolTurno> rolesTurno);

        /// <summary>
        /// Elimina_RolTurnoo:  Metodo borrado de datos de la tabla ph_roles_turnos
        /// </summary>
        /// <param name="idregistro"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Elimina_RolTurno(int idregistro);

        // Metodos TRANSFORMACIONES
        public Task<IEnumerable<cTransformacion>> GetTransformacion();
        public Task<cTransformacion> GetTransformacion(int id);
        /// <summary>
        /// Sincronizar_Transformacion: Método para registrar las Transformaciones Turno en la tabla Tranformaciones
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="transformaciones">Lista de registros de la clase cTransformacion</param>
        public Task<EventResponse> Sincronizar_Transformacion(IEnumerable<cTransformacion> transformaciones);
        /// <summary>
        /// Elimina_Transformacion:  Metodo borrado de datos de la tabla Transformacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Elimina_Transformacion(int id);

        /// <summary>
        /// GetTransformacionGlobal: obtener lista de Transformaciones Globales
        /// </summary>
        /// <returns>Lista de Transformacion Global</returns>
        public Task<IEnumerable<cTransformacionGlobal>> GetTransformacionGlobal();
        public Task<cTransformacionGlobal> GetTransformacionGlobal(int id);
        public Task<EventResponse> Sincronizar_TransformacionGlobal(IEnumerable<cTransformacionGlobal> departamentos);
        public Task<EventResponse> Elimina_TransformacionGlobal(int id);

        /// <summary>
        /// GetTransformacionTipoMarca: obtener lista de Transformaciones Tipo Marca
        /// </summary>
        /// <returns>Lista de Transformacion Tipo Marca</returns>
        public Task<IEnumerable<cTransformacionTipoMarca>> GetTransformacionTipoMarca();
        public Task<cTransformacionTipoMarca> GetTransformacionTipoMarca(int id);
        public Task<EventResponse> Sincronizar_TransformacionTipoMarca(IEnumerable<cTransformacionTipoMarca> transformacionTM);
        public Task<EventResponse> Elimina_TransformacionTipoMarca(int id);
        public Task<EventResponse> Elimina_TransformacionTipoMarcaDet(string id);

        /// <summary>
        /// GetTransformacionTipoMarcaDet: obtener lista de Transformaciones Tipo Marca Detalle
        /// </summary>
        /// <returns>Lista de Transformacion Tipo Marca Detalle</returns>
        public Task<IEnumerable<cTransformacionTipoMarcaDet>> GetTransformacionTipoMarcaDet(int transformacionID);
        public Task<EventResponse> Sincronizar_TransformacionTipoMarcaDet(IEnumerable<cTransformacionTipoMarcaDet> transformacionTMD);

        /// <summary>
        /// GetIncidencia_Conf_Pago: obtener lista de Incidencias Conf Pago
        /// </summary>
        /// <returns>Lista de Incidencias Conf Pago</returns>
        public Task<IEnumerable<cIncidencia_Conf_Pago>> GetIncidencia_Conf_Pago();
        public Task<cIncidencia_Conf_Pago> GetIncidencia_Conf_Pago(int id);
        public Task<EventResponse> Sincronizar_Incidencia_Conf_Pago(IEnumerable<cIncidencia_Conf_Pago> incidenciasConfPago);
        public Task<EventResponse> Elimina_Incidencia_Conf_Pago(int id);

        /// <summary>
        /// GetPortalMenu: Obtener lista de menus de sistema 
        /// </summary>
        /// <returns>Lista de lista de menus del sistema</returns>
        public Task<List<cPortal_Menu>> GetPortalMenu();
        /// <summary>
        /// GetPortalMenu: Obtener datos de una opcion de menu de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPortal_Menu </returns>
        public Task<cPortal_Menu> GetPortalMenu(string id);
        /// <summary>
        /// Sincronizar_PortalMenu: Método para registrar los menus del sistema
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="portalOpcion">Lista de registros de cPortal_Menu </param>
        public Task<EventResponse> Sincronizar_PortalMenu(IEnumerable<cPortal_Menu> portalMenu);

        /// <summary>
        /// GetPortalPol: Obtener lista de roles del Portal de empleado y Geotime.net 
        /// </summary>
        /// <returns>Lista de lista de roles del Portal de empleado y Geotime.net </returns>
        public Task<List<cPortal_Rol>> GetPortalRol();
        /// <summary>
        /// GetPortalRol: Obtener datos de un rol del portal especifico
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPortal_Rol </returns>
        public Task<cPortal_Rol> GetPortalRol(string id);
        /// <summary>
        /// Sincronizar_PortalRol: Método para registrar los roles del portal
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="portalOpcion">Lista de registros de cPortal_Rol </param>
        public Task<EventResponse> Sincronizar_PortalRol(IEnumerable<cPortal_Rol> portalRol);

        /* Descanso Turno Allan 4/3/2024 */
        /// <summary>
        /// GetPhDescansoTurno: Obtener datos de Descanso Turno 
        /// </summary>
        /// <param name="idTurno">idTurno de Descanso Turno</param>
        /// <param name="idTiempo">idTiempo de Descanso Turno</param>
        /// <returns>Instancia de cPh_DescansoTurno </returns>
        public Task<List<cPh_DescansoTurno>> GetPhDescansoTurno(int idTurno, int idTiempo);

        /// <summary>
        /// Sincronizar_PhDescansoTurno: Método para registrar los Descanso de Turno
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="ph_DescansoTurno">Lista de registros de la clase cPh_DescansoTurno</param>
        public Task<EventResponse> Sincronizar_PhDescansoTurno(IEnumerable<cPh_DescansoTurno> ph_DescansoTurno);

        /// <summary>
        /// GetPhOpciones: Obtener datos de Sistema 
        /// </summary>
        /// <returns>Instancia de cPh_Opciones con las Opciones del sistema </returns>
        public Task<cPh_Opciones> GetPhOpciones();

        /// <summary>
        /// Sincronizar_PhOpciones: Método para Registrar las opciones del Sistema
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="ph_Opciones">Lista de registros de la clase cPh_Opciones</param>
        public Task<EventResponse> Sincronizar_PhOpciones(IEnumerable<cPh_Opciones> ph_Opciones);

        /// <summary>
        /// GetPortalEmpleado: Lista de empleados con acceso al portal de marcas web
        /// </summary>
        /// <returns>Lista de empleados con acceso al portal de marcas web</returns>
        public Task<List<cPortal_Empleado>> GetPortalEmpleado();
        /// <summary>
        /// GetPortalEmpleado: Un empleado con acceso al portal de marcas web
        /// </summary>
        /// <param name="id">id de empleado a buscar</param>
        /// <returns>Un empleado con acceso al portal de marcas web</returns>
        public Task<cPortal_Empleado> GetPortalEmpleado(string id);
        /// <summary>
        /// Sincronizar_PortalEmpleado:  Crear o actualizar la lista de empleados con acceso a marcar web.  Se verifica cada elemento si existe en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="portalEmpleados">Recibe una instancia de cPortal_Empleado</param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>
        public Task<EventResponse> Sincronizar_PortalEmpleado(IEnumerable<cPortal_Empleado> portalEmpleados);

        /// <summary>
        /// PutPortalEmpleado:  Actualizar la lista de empleados con acceso a marcar web.  Se verifica cada elemento si existe en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="portalEmpleado">Recibe una instancia de cPortal_Empleado</param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>
        public Task<EventResponse> PutPortalEmpleado(cPortal_Empleado portalEmpleado);



        /// <summary>
        /// GetPortalDocMarca: Lista de documentos total de Documentos Marcas
        /// </summary>
        /// <returns>Lista de documentos total de Documentos Marcas</returns>
        public Task<List<cPortal_DocMarca>> GetPortalDocMarca();
        /// <summary>
        /// GetPortalDocMarca: Lista de documentos asociados al empleado en una fecha especifica
        /// </summary>
        /// <param name="idnumero">numero de empleado</param>
        /// <param name="fecha">fecha del documento</param>
        /// <returns>Lista de documentos asociados al empleado en una fecha especifica</returns>
        public Task<List<cPortal_DocMarca>> GetPortalDocMarca(string idnumero, string fecha);
        /// <summary>
        /// GetPortalDocMarca: Obtiene un documento especifico de Portal DocsMarcas
        /// </summary>
        /// <param name="idregistro">id del registro</param>
        /// <returns>Un documento especifico de Portal DocsMarcas</returns>
        public Task<cPortal_DocMarca> GetPortalDocMarca(long idregistro);
        /// <summary>
        /// Sincronizar_PortalDocMarca:  Crear o actualizar la lista de documentos asociados a las marcas.  Se verifica cada elemento si existe en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="portalDocsMarcas">Recibe una instancia de cPortal_DocMarca</param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>
        public Task<EventResponse> Sincronizar_PortalDocMarca(IEnumerable<cPortal_DocMarca> portalDocsMarcas);

        /// <summary>
        /// ConsultarMarcasPeriodo:  Proceso paradeterminar marcas del periodo que se deben visualizar en el sistema
        /// </summary>
        /// <param name="IdsGrupos">Listado de grupos separados por coma , que por los que se debe filtrar la informacion</param>
        /// <param name="IdPlanilla">id de planilla por el que se debe filtrar la informacion</param>
        /// <param name="FechaInicio">fecha de inicio del reporte</param>
        /// <param name="FechaFin">fecha final del reporte</param>
        /// <param name="idnumero">id del empleado que se desea obtener, si se envia un -1 trae todos los empleados</param>
        /// <returns>Lista de marcas del periodo</returns>
        public Task<IEnumerable<cMarcaPeriodo>> GetMarcasPeriodo(string IdsGrupos, string IdPlanilla, string FechaInicio, string FechaFin, string idnumero);


        /// <summary>
        /// GetMarcaDistribucion: Obtener las Marcas distribuciones para un tipo planilla, un empleado y un rango de fechas especifico
        /// </summary>
        /// <param name="idplanilla"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idnumero"></param>
        /// <returns>Lista de marcas distribuciones</returns>
        public Task<List<cMarcaDistribucion>> GetMarcaDistribucion(string idplanilla, string fechaInicio, string fechaFinal, string idnumero, string hora);


        /// <summary>
        /// AutorizarExtrasPeriodo: proceso para autorizar extras del periodo de empleados.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public Task<EventResponse> AutorizarExtrasPeriodo(cExtraAprobacion parametros);

        /// <summary>
        /// PreAutorizarExtrasMasiva: proceso para pre-autorizar extras del periodo de empleados
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public Task<EventResponse> PreAutorizarExtrasMasiva(IEnumerable<cExtraAprobacion> parametros);

        /// <summary>
        /// AutorizarExtrasMasiva: proceso para autorizar extras de varios empleados.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public Task<EventResponse> AutorizarExtrasMasiva(IEnumerable<cExtraAprobacion> parametros);

        /// <summary>
        /// AutorizarExtras: proceso para autorizar un registro de extras
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public Task<EventResponse> AutorizarExtras(cExtraAprobacion parametros);

        /// <summary>
        /// EditarMarca: ejecuta procedimiento almacenado para la actualizacion de Marcas
        /// </summary>
        /// <param name="marcas"></param>
        /// <returns>Devuelve una instancia de eventresponse con el resultado de la operacion</returns>
        public Task<EventResponse> EditarMarca(cMarcaEditParam marcasEdit);

        /// <summary>
        /// Sincronizar_MarcasIncidencias: Método para agregar o actualizar marcas incidencias
        /// </summary>
        /// <param name="indice"></param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>

        public Task<EventResponse> Sincronizar_MarcasIncidencias(IEnumerable<cMarcaIncidencia> marcasIncidencias);

        /// <summary>
        /// Elimina_MarcasIncidencias: Método para eliminar marcas incidencias
        /// </summary>
        /// <param name="indice"></param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>
        public Task<EventResponse> Elimina_MarcasIncidencias(string indice);

        /// <summary>
        /// GetPhMenuSistema: Obtener lista de menus de sistema 
        /// </summary>
        /// <returns>Lista de lista de menus del sistema</returns>
        public Task<List<cPh_MenuSistema>> GetPhMenuSistema();

        /// <summary>
        /// GetPhMenuSistema: Obtener datos de una opcion de menu de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPh_MenuSistema </returns>
        public Task<cPh_MenuSistema> GetPhMenuSistema(string id);

        /// <summary>
        /// Sincronizar_PortalMenu: Método para registrar los menus del sistema
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="portalOpcion">Lista de registros de cPortal_Menu </param>
        public Task<EventResponse> Sincronizar_PhMenuSistema(IEnumerable<cPh_MenuSistema> portalMenu);

        /// <summary>
        /// GetPh_OpcionSistema: Obtener lista de opciones del menu de CA 
        /// </summary>
        /// <returns>Lista de Opciones del sistema</returns>
        public Task<List<cPh_OpcionSistema>> GetPh_OpcionSistema();

        /// <summary>
        /// GetPortalOpcion: Obtener datos de una opcion de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPortal_Opcion con los datos de la opción </returns>
        public Task<cPh_OpcionSistema> GetPh_OpcionSistema(string id);

        /// <summary>
        /// Sincronizar_PortalOpcion: Método para registrar las opciones del sistema Portal de empleados
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="portalOpcion">Lista de registros de cPortal_Opcion </param>
        public Task<EventResponse> Sincronizar_PhOpcionSistema(IEnumerable<cPh_OpcionSistema> portalOpcion);

        // creando por Marlon Loria Solano 23-01-2025
        /// <summary>
        /// GetPhRolSistema: Obtener lista de Roles de sistema 
        /// </summary>
        /// <returns>Lista de lista de menus del sistema</returns>
        public Task<List<cPh_RolSistema>> GetPhRolSistema();

        // creando por Marlon Loria Solano 23-01-2025
        /// <summary>
        /// GetPhRolSistema: Obtener datos de un rol de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>Instancia de cPh_RolSistema </returns>
        public Task<cPh_RolSistema> GetPhRolSistema(string id);

        // creando por Marlon Loria Solano 23-01-2025
        /// <summary>
        /// Sincronizar_PhRolSistema: Método para registrar los roles del sistema
        /// </summary>
        /// <returns>Una instancia de la Clase EventResponse, con el resultado del proceso</returns>
        /// <param name="roles">Lista de registros de cPh_RolSistema </param>
        public Task<EventResponse> Sincronizar_PhRolSistema(IEnumerable<cPh_RolSistema> roles);

        // creando por Marlon Loria Solano 23-01-2025
        /// <summary>
        /// GetPhRolSistemaDet: Obtener lista de Detalle de Roles de sistema 
        /// </summary>
        /// <returns>Lista de lista de detalle de roles del sistema</returns>
        public Task<List<cPh_RolSistemaDet>> GetPhRolSistemaDet();

        // creando por Marlon Loria Solano 23-01-2025
        /// <summary>
        /// GetPhRolSistemaDet: Obtener datos de detalle de un rol de sistema 
        /// </summary>
        /// <param name="id">id de la opcion</param>
        /// <returns>lista de opciones asociadas al rol cPh_RolSistemaDet </returns>
        public Task<List<cPh_RolSistemaDet>> GetPhRolSistemaDet(string id);

        // creando por Marlon Loria Solano 23-01-2025
        /// <summary>
        /// GetPhUsuarioRol: Obtener datos de detalle roles de sistema para un usuario especifico 
        /// </summary>
        /// <param name="id">id de usuario</param>
        /// <returns>lista de roles asociados al usuario</returns>
        public Task<List<cPh_UsuarioRol>> GetPhUsuarioRol(int id);

        /// <summary>
        /// Sincronizar_PhUsuarioRol:  Crear o actualizar la lista de usuarios y roles del sistema.  Se verifica cada elemento si existe en cuyo caso actualiza el registro, de lo contrario lo crea.
        /// </summary>
        /// <param name="usuariosRoles">Recibe una lista de cPh_UsuarioRol</param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>
        public Task<EventResponse> Sincronizar_PhUsuarioRol(IEnumerable<cPh_UsuarioRol> usuariosRoles);
        
        /// <summary>
        /// EnviarCorreo: Notificaciones por correo
        /// </summary>
        /// <param name="correo"></param>
        /// <returns></returns>
        public Task<EventResponse> EnviarCorreo(IEnumerable<Email> correo);

        /// <summary>
        /// VerificaUsuariosActivosPMW:  Verifica cantidad de usuarios habilitados en la base de datos para el portal de marcas web
        /// </summary>
        /// <returns>cantidad de usuarios habilitados para el portal de marcas web</returns>
        public Task<int> VerificaUsuariosActivosPMW();

        /// <summary>
        /// ActualizarCompaniaBD:  Metodo que realiza la actualizacion de la estructura de la base de datos para una compañía
        /// </summary>
        /// <param name="compania"></param>
        /// <returns>EventResponse con el detalle de la actualización</returns>
        public Task<EventResponse> ActualizarCompaniaBD(cPh_Compania compania, bool migrate);

        /// <summary>
        /// GetPhCatalogoGenerico: lista de catalogos genericos
        /// </summary>
        /// <returns>lista de catalogos genericos</returns>
        public Task<List<cPh_CatalogoGenerico>> GetPhCatalogoGenerico();

        /// <summary>
        /// GetPhCatalogoGenerico: lista de datos para un catalogo especifico
        /// </summary>
        /// <param name="id">id de catalogo</param>
        /// <returns>lista de catalogos genericos</returns>
        public Task<List<cPh_CatalogoGenerico>> GetPhCatalogoGenerico(string nombre);

        public Task<cPh_CatalogoGenerico> GetPhCatalogoGenerico(string nombre, string id);

        public Task<EventResponse> ActualizaPwdsUsuariosBD();
        public Task CreaNivelesSeguridad(string compania);

    }
}
