﻿using GeotimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models;
using GeoTimeConnectWebApi.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace GeoTimeConnectWebApi.Data.Interfaz
{
    public interface IGeoTimeConnectService
    {
        public Task<EventResponse> ActivarPeriodoPAAsync(cActivarPeriodo parametros);

        /* Metodos SQL */
        #region SQLMetodes
        public Task<IEnumerable<cPh_Login>> GetPhLogin();
        public Task<cPh_Login> GetPhLogin(string id);
        public Task<List<cPh_Compania>> GetPhCompania();
        public Task<cPh_Compania> GetPhCompania(string idcomp);

        /// <summary>
        /// Sincronizar_PhCompania: metodo para sincronizar las compañias 
        /// </summary>
        /// <param name="phCompanias"></param>
        /// <returns>una instancia EventResponse con el resultado de la operacion</returns>
        public Task<EventResponse> Sincronizar_PhCompania(IEnumerable<cPh_Compania> phCompanias);


        #endregion

        #region SPMetodos

        /// <summary>
        /// /Obtener lista de Compañias asociadas al usuario
        /// </summary>
        /// <returns>Lista de Companias de Usuario </returns>
        public Task<List<cPh_CompaniaUsuario>> GetPhCompaniaUsuario(string idnumero);


        #endregion

        public Task<cAccionPersonal> GetAccionPersonal(long idregistro);
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
        public Task<EventResponse> Elimina_Concepto(int id);
        /// <summary>
        /// GetDepartamento: obtener lista de departamentos
        /// </summary>
        /// <returns>Lista de departamentos</returns>
        public Task<List<cDepartamento>> GetDepartamento();
        public Task<cDepartamento> GetDepartamento(string idDepart);
        public Task<EventResponse> Sincronizar_Departamento(IEnumerable<cDepartamento> departamentos);
        //public Task<EventResponse> Elimina_Concepto(int id);

        /// <summary>
        /// GetEmpleado: Método para obtener una lista de empleados 
        /// </summary>
        /// <returns>Lista de cEmpleados</returns>
        public Task<List<cEmpleado>> GetEmpleado();

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

        public Task<List<cIncidencia>> GetIncidencia();
        public Task<cIncidencia> GetIncidencia(int id);
        public Task<cIncidencia> GetIncidenciaByNomConector(string nom_conector);
        public Task<List<cIncidencia>> GetIncidenciaReqAccPer();
        public Task<EventResponse> Sincronizar_Incidencia(IEnumerable<cIncidencia> incidencias);

        /// <summary>
        /// Elimina_Incidencia:  Metodo borrado de datos de la tabla Incidencias
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Elimina_Incidencia(int id);

        public Task<List<cMarcaResumen>> GetMarcasResumen(string idPlanilla, string idPeriodo);
		public Task<EventResponse> Sincronizar_MarcasResumen(IEnumerable<cMarcaResumen> marcasResumen);

        public Task<List<cTurno>> GetTurno();
        public Task<cTurno> GetTurno(int idTurno);
        public Task<EventResponse> Sincronizar_Turno(IEnumerable<cTurno> phTurno);

        /// <summary>
        /// Elimina_Turno:  Metodo borrado de datos de la tabla ph_Turnos
        /// </summary>
        /// <param name="idTurno"></param>
        /// <returns>EventResponse</returns>
        public Task<EventResponse> Elimina_Turno(int idTurno);

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
        public Task<EventResponse> ValidarClaveEmpleado(cLogin login);

        /// <summary>
        /// ValidarClaveAdm: Validar Clave de Usuario Admin.  Recibe una instancia de empleado, se verifica si existe y se valida contraseña indicada contra la registrada en la base de datos
        /// </summary>
        /// <param name="login">datos del usuario a validar</param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>
        public Task<EventResponse> ValidarClaveAdm(cLogin login);
        public Task<EventResponse> CambiarClaveEmpleado(cEmpleado empleado);


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

        /// <summary>
        /// GetMarcaMovTurnoByGrupo: Método para obtener una lista de Marcas Mov Turnos de los empleados asignados a un supervisor 
        /// </summary>
        /// <returns>Lista de cMarcaMovTurno</returns>
        /// <param name="fechaPeriodo">Fecha del Periodo para el cual se requieren los turnos</param>
        /// <param name="idgrupo">grupo de empleado</param>
        public Task<List<cMarcaMovTurno>> GetMarcaMovTurnoByGrupo(string fechaPeriodo, string idgrupo);
        public Task<EventResponse> Sincronizar_MarcasMovTurnos(IEnumerable<cMarcaMovTurno> marcasMovTurnos);
        public Task<List<cPh_Grupo>> GetGrupo();
        public Task<cPh_Grupo> GetGrupo(int idgrupo);
        public Task<EventResponse> Sincronizar_Grupo(IEnumerable<cPh_Grupo> PhGrupos);
        public Task<EventResponse> Elimina_Grupo(int idgrupo);

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

        public Task<EventResponse> Sincronizar_Periodo(IEnumerable<cPh_Periodos> periodos);
        public Task<EventResponse> Elimina_Periodo(string id);

        public Task<IEnumerable<cPh_Planilla>> GetPhPlanilla();
        public Task<cPh_Planilla> GetPhPlanilla(string idplanilla);
        public Task<cPh_Planilla> GetPhPlanilla(string nomConector, string descPlanilla);
        public Task<EventResponse> Sincronizar_PhPlanilla(IEnumerable<cPh_Planilla> PhPlanillas);
        public Task<EventResponse> Elimina_PhPlanilla(string idplanilla);


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

        /// <summary>
        /// GetMarcasAudit: Obtener las marcas_audit para un empleado, planilla y un periodo especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <returns>Lista de Marcas Audit</returns>

        public Task<List<cMarcaAudit>> GetMarcasAudit(string idnumero, string fecha, string idplanilla);

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
        /// GetMarcasIncidencias: Obtener las Marcas Incidencias para un empleado, planilla y un periodo especifico
        /// </summary>
        /// <param name="idnumero">numero de empleado a buscar</param>
        /// <param name="fecha">fecha para determinar periodo</param>
        /// <param name="idplanilla">id de planilla</param>
        /// <returns>Lista de Marcas Incidencias</returns>
        public Task<List<cMarcaIncidencia>> GetMarcaIncidencia(string idnumero, string fecha, string idplanilla);

        /// <summary>
        /// GetMarcaIncidencia: Obtener una Marca Incidencia especifica segun el id indicado en el parámetro
        /// </summary>
        /// <param name="id">numero de marca incidencia</param>
        /// <returns>Una instancia de Marcas Incidencias</returns>
        public Task<cMarcaIncidencia> GetMarcaIncidencia(long id);

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
        /// GetPhUsuario: Obtener datos de usuario 
        /// </summary>
        /// <param name="idnumero">id numero del empleado</param>
        /// <returns>Instancia de phusuario con los datos del usuario </returns>
        public Task<cPh_Usuario> GetPhUsuario(string idnumero);

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

        public Task<EventResponse> EnviarCorreo(Email correo);
        public Task<cParametroEmail> GetParametroEmail(int id);
        public Task<EventResponse> Sincronizar_ParametroEmail(cParametroEmail parametroEmail);

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
        /// GetTipo_Planilla: Método para obtener una lista de planillas 
        /// </summary>
        /// <returns>Lista de cTipo_Planilla</returns>
        public Task<List<cTipo_Planilla>> GetTipo_Planilla();

        /// <summary>
        /// GetPh_Transformacion: Método para obtener una lista de tranformaciones 
        /// </summary>
        /// <returns>Lista de cPh_Transformacion</returns>
        public Task<List<cPh_Transformacion>> GetPhTransformacion();

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
        public Task<EventResponse> Sincronizar_PhOpciones(cPh_Opciones ph_Opciones);

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
        /// EjecutaInitPeriodo:  Se ejecuta el WebService Init_Periodo.
        /// </summary>
        /// <param name="parametros">Recibe una instancia de cInit_Periodo</param>
        /// <returns>Instancia de EventResponse con el resultado de la operación</returns>
        public Task<EventResponse> EjecutaInitPeriodo(IEnumerable<cInit_Periodo> parametros);
    }
}
