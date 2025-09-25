
using Microsoft.EntityFrameworkCore;
using com.gsitcr.geotime.Data.Interfaz;
using com.gsitcr.geotime.Models;
using GeotimeModelsLib.Models;
using System.Reflection.Emit;
using System.Xml;
using System.Runtime.InteropServices.Marshalling;

namespace com.gsitcr.geotime.Data
{

    public class SqlServerDataBaseContext:DbContext
    {
        public string Schema { get; }
        public DateTime? _IModelChanged { get; set; }
        public string _AssemblyName { get; set; }

        private readonly IDbContextSchema _dbContextSchema1;


        private string schemaAdmin;

        public SqlServerDataBaseContext(DbContextOptions<SqlServerDataBaseContext> options,
                                        IDbContextSchema? schema = null) : base(options)
        {
            // Build a config object, using env vars and JSON providers.
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            schemaAdmin = config.GetConnectionString("SchemaAdmin");

            if (schema is null)
            {
                Schema = config.GetConnectionString("Schema");
            }
            else
            {
                Schema = schema.Schema;
                _dbContextSchema1 = schema;

            }
            if (Schema is null || Schema=="")
            {
                Schema = config.GetConnectionString("Schema");
            }

            

        }

        public DbSet<cAccionPersonal> Acciones_Personal { get; set; }
        public DbSet<cCentroCosto> Ph_CCostos { get; set; }
        public DbSet<cConcepto> Ph_Conceptos { get; set; }
        public DbSet<cDepartamento> Ph_Departamento { get; set; }
        public DbSet<cEmpleado> Empleados { get; set; }
        public DbSet<cIncidencia> Incidencias { get; set; }
        public DbSet<cMarcaResumen> Marcas_Resumen { get; set; }
        public DbSet<cTurno> Ph_Turnos { get; set; }
        public DbSet<cMarca> Marcas { get; set; }
        public DbSet<cPh_Login> PH_LOGIN { get; set; }
        public DbSet<cPh_Compania> PH_COMPANIAS { get; set; }
        public DbSet<cMarcaMovTurno> Marcas_Mov_Turnos { get; set; }
        public DbSet<cMarcaMovHorario> Marcas_Mov_Horarios { get; set; }
        public DbSet<cMarcaReporte> Marcas_Reportes { get; set; }
        public DbSet<cPh_Grupo> Ph_Grupos { get; set; }
		public DbSet<cPh_Periodos> Ph_Periodos { get; set; }
        public DbSet<cPh_Planilla> Ph_Planilla { get; set; }
        public DbSet<cMarcaIn> Marcas_In { get; set; }
        public DbSet<cMarcaExtraApb> Marcas_Extras_Apb { get; set; }
        public DbSet<cMarcaProceso> Marcas_Proceso { get; set; }
        public DbSet<cPh_Proyecto> Ph_Proyecto { get; set; }
        public DbSet<cPh_FaseProyecto> Ph_FaseProyecto { get; set; }
        public DbSet<cMarcaAudit> Marcas_Audit { get; set; }
        public DbSet<cMarcaDescanso> Marcas_Descansos { get; set; }
        public DbSet<cMarcaIncidencia> Marcas_Incidencias { get; set; }
        public DbSet<cMarcaDistribucion> Marcas_Distribuciones { get; set; }
        public DbSet<cPh_Usuario> Ph_Usuarios { get; set; }
        public DbSet<cPh_Sistema> Ph_Sistema { get; set; }
        public DbSet<cPortal_Config> Portal_Config { get; set; }
        public DbSet<cPortal_Menu> Portal_Menu { get; set; }
        public DbSet<cPortal_Opcion> Portal_Opciones { get; set; }
        public DbSet<cPh_Formulacion> Ph_Formulacion { get; set; }
        public DbSet<cParametroEmail> ParametrosEmail { get; set; }
        public DbSet<cPh_Horarios> Ph_Horarios { get; set; }
        public DbSet<cPh_HorarioTurno> Ph_Horario_Turnos { get; set; }
        public DbSet<cTipo_Planilla> TIPOS_PLANILLA { get; set; }
        public DbSet<cPh_Transformacion> Ph_Transformacion { get; set; }
        public DbSet<cPh_Rol> Ph_Roles { get; set; }
        public DbSet<cPh_RolTurno> Ph_Roles_Turnos { get; set; }
        public DbSet<cTransformacion> Transformaciones { get; set; }
        public DbSet<cTransformacionGlobal> TransformacionesGlobales { get; set; }
        public DbSet<cTransformacionTipoMarca> TransformacionesTipoMarca { get; set; }
        public DbSet<cTransformacionTipoMarcaDet> TransformacionesTipoMarcaDet { get; set; }
        public DbSet<cIncidencia_Conf_Pago> Incidencias_Conf_Pago { get; set; }
        public DbSet<cPortal_Rol> Portal_Rol { get; set; }
        public DbSet<cPortal_RolDet> Portal_RolDet { get; set; }
        public DbSet<cPh_DescansoTurno> Ph_Descansos_Turnos { get; set; }
        public DbSet<cPh_Opciones> Ph_Opciones { get; set; }
        public DbSet<cPortal_Empleado> Portal_Empleado { get; set; }
        public DbSet<cPortal_DocMarca> Portal_DocsMarcas { get; set; }
        public DbSet<cPaletaColor> PaletaColores { get; set; }
        public DbSet<cPh_Nivel> Ph_Niveles { get; set; }
        public DbSet<cPh_CatalogoGenerico> Ph_Catalogo_Generico { get; set; }
        public DbSet<cPh_Puesto> Ph_Puestos { get; set; }

        public DbSet<cPh_Distribucion_CCosto> Ph_Distribuciones_CCosto { get; set; }
        public DbSet<cMarcaDistribucionConcepto> Marcas_Distribuciones_Conceptos { get; set; }
        public DbSet<cMarcaTiempoAdicional> Marcas_Tiempo_Adicional { get; set; }
        public DbSet<cTemplateHID> TemplatesHID { get; set; }
        public DbSet<cTemplateFACE> TemplatesFACES { get; set; }

        public DbSet<cPh_MenuSistema> Ph_Menus_Sistema { get; set; }
        public DbSet<cPh_OpcionSistema> Ph_Opciones_Sistema { get; set; }

        public DbSet<cPh_RolSistema> Ph_Roles_Sistema { get; set; }
        public DbSet<cPh_RolSistemaDet> Ph_Roles_SistemaDet { get; set; }
        public DbSet<cPh_UsuarioRol> Ph_Usuarios_Roles { get; set; }


        public DbSet<cRelojDispositivoAdmin> RelojDispositivoAdmin { get; set; }
        public DbSet<cRelojDispositivo> RelojDispositivo { get; set; }
        public DbSet<cRelojTemplate> RelojTemplate { get; set; }
        public DbSet<cRelojTemplateFace> RelojTemplateFace { get; set; }
        public DbSet<cRelojUsuario> RelojUsuario { get; set; }

        /*organizacion y niveles de autorizacion*/
        public DbSet<cOrganizacionNivel> OrganizacionNiveles { get; set; }
        public DbSet<cOrganizacion> Organizacion { get; set; }
        public DbSet<cOrganizacionResponsable> OrganizacionResponsables { get; set; }
        public DbSet<cFlujoAutorizacion> FlujosAutorizacion { get; set; }
        public DbSet<cFlujoAutorizacionDetalle> FlujosAutorizacionDetalle { get; set; }
        public DbSet<cEstado> Estados { get; set; }
        public DbSet<cTipoSolicitud> TiposSolicitudes { get; set; }
        public DbSet<cSolicitudConfiguracion> SolicitudConfiguracion { get; set; }
        public DbSet<cSolicitud> Solicitudes { get; set; }
        public DbSet<cSolicitudDetalle> SolicitudesDetalles { get; set; }
        public DbSet<cSolicitudAutorizacion> SolicitudesAutorizacion { get; set; }
        public DbSet<cEmpleadoJefatura> EmpleadosJefaturas { get; set; }
        public DbSet<cOrganizacionBaseResponsable> OrganizacionBaseResponsables { get; set; }

        //storeprocedure
        public virtual DbSet<cInMarcaWeb> InMarcaWeb { get; set; }

        //vistas para reportes

        public virtual DbSet<cVHoraExtraXTurno> VHorasExtrasXTurno { get; set; }
        public virtual DbSet<cVHoraLaboradaEmpleado> VHorasLaboradasEmpleado { get; set; }
        public virtual DbSet<cMarcaMovTurnoBitacora> Marcas_Mov_Turnos_Bitacora { get; set; }
        public virtual DbSet<cVMarcaComedor> VMarcas_Comedor { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(schemaAdmin);
           

            #region Objetos del CTAADMIN

            builder.Entity<cPh_Login>().ToTable("PH_LOGIN", schemaAdmin)
               .HasKey(e => new { e.idusuario });
            builder.Entity<cPh_Compania>().ToTable("PH_COMPANIAS", schemaAdmin)
                .HasKey(e => new { e.IDCOMP });
            builder.Entity<cPh_Sistema>().ToTable("PH_SISTEMA", schemaAdmin)
                .HasNoKey();
            builder.Entity<cPh_MenuSistema>().ToTable("PH_MENUS_SISTEMA", schemaAdmin)
                .HasKey(e => new { e.ID });
            builder.Entity<cPh_OpcionSistema>().ToTable("PH_OPCIONES_SISTEMA", schemaAdmin)
                .HasKey(e => new { e.ID });
            builder.Entity<cPh_RolSistema>().ToTable("PH_ROLES_SISTEMA", schemaAdmin)
                .HasKey(e => new { e.ID });
            builder.Entity<cPh_RolSistemaDet>().ToTable("PH_ROLES_SISTEMADET", schemaAdmin)
               .HasKey(e => new { e.ROLSISTEMAID,e.MENUSISTEMAID,e.OPCIONSISTEMAID });
            builder.Entity<cPh_UsuarioRol>().ToTable("PH_USUARIOS_ROLES", schemaAdmin)
                .HasKey(e => new { e.IDUSUARIO,e.IDREGISTRO });

            //INTEGRACION CON REOLJES
            builder.Entity<cRelojDispositivoAdmin>().ToTable("RELOJES_DISPOSITIVO", schemaAdmin)
               .HasKey(e => new { e.CLOCK_ID });
            builder.Entity<cRelojDispositivo>().ToTable("RELOJES_DISPOSITIVO", Schema)
               .HasKey(e => new { e.CLOCK_ID });
            builder.Entity<cRelojUsuario>().ToTable("RELOJ_USUARIO", Schema)
               .HasKey(e => new { e.FP_ENROLLID });
            builder.Entity<cRelojTemplate>().ToTable("RELOJ_TEMPLATES", Schema)
               .HasKey(e => new { e.FP_ENROLLID, e.FP_INDEXID });
            builder.Entity<cRelojTemplateFace>().ToTable("RELOJ_TEMPLATES_FACES", Schema)
               .HasKey(e => new { e.FACE_PIN, e.FACE_INDEX });

            builder.Entity<cInMarcaWeb>().HasNoKey();

            builder.Entity<cPh_CatalogoGenerico>().ToTable("PH_CATALOGO_GENERICO", schemaAdmin)
                .HasKey(e => new { e.NombreCatalogo, e.Id });

            #endregion

            #region Objetos Estandar Geotime

            builder.Entity<cAccionPersonal>().ToTable("ACCIONES_PERSONAL", Schema)
                .HasKey(e => new { e.IdRegistro });
            builder.Entity<cCentroCosto>().ToTable("PH_CCOSTOS", Schema)
                 .HasKey(e => new { e.IdCCosto });
            builder.Entity<cConcepto>().ToTable("PH_CONCEPTOS", Schema)
                 .HasKey(e => new { e.id });
            builder.Entity<cDepartamento>().ToTable("PH_DEPARTAMENTO", Schema)
                .HasKey(e => new { e.IDDEPART });
            builder.Entity<cEmpleado>().ToTable("EMPLEADOS", Schema)
                .HasKey(e => new { e.IdNumero });
            builder.Entity<cIncidencia>().ToTable("INCIDENCIAS", Schema)
                .HasKey(e => new { e.Id });
            builder.Entity<cMarcaResumen>().ToTable("MARCAS_RESUMEN", Schema)
                .HasKey(e => new { e.IdPlanilla, e.IdNumero, e.IdConcepto, e.IdCCosto });
            builder.Entity<cTurno>().ToTable("PH_TURNOS", Schema)
                .HasKey(e => new { e.IdTurno });
            builder.Entity<cMarca>().ToTable("MARCAS", Schema)
                .HasKey(e => new { e.registro });
            builder.Entity<cMarcaMovTurno>()
                .ToTable("MARCAS_MOV_TURNOS", Schema, e => e.HasTrigger("MARCAS_MOV_TURNOSIup"))
                .ToTable("MARCAS_MOV_TURNOS", Schema, e => e.HasTrigger("MARCAS_MOV_TURNOSdel"))
                .HasKey(e => new { e.idregistro });
            builder.Entity<cMarcaMovTurnoBitacora>().ToTable("MARCAS_MOV_TURNOS_BITACORA", Schema)
               .HasKey(e => new { e.idbitacora });
            builder.Entity<cMarcaMovHorario>().ToTable("MARCAS_MOV_HORARIOS", Schema)
               .HasKey(e => new { e.IDREGISTRO });
            builder.Entity<cPh_Grupo>().ToTable("PH_GRUPOS", Schema)
                .HasKey(e => new { e.idgrupo });
            builder.Entity<cPh_Periodos>().ToTable("PH_PERIODOS", Schema)
                .HasKey(e => new { e.idperiodo });
            builder.Entity<cPh_Planilla>().ToTable("PH_PLANILLA", Schema)
                .HasKey(e => new { e.idplanilla });
            builder.Entity<cMarcaIn>().ToTable("MARCAS_IN", Schema)
                .HasKey(e => new { e.idtarjeta, e.fecha, e.hora, e.idnumero, e.tipo, e.fecha_reg });
            builder.Entity<cMarcaExtraApb>().ToTable("MARCAS_EXTRAS_APB", Schema)
                .HasKey(e => new { e.idregistro });
            builder.Entity<cMarcaProceso>().ToTable("MARCAS_PROCESO", Schema)
                .HasKey(e => new { e.idregistro });
            builder.Entity<cMarcaReporte>().ToTable("MARCAS_REPORTES", Schema)
                .HasKey(e => new { e.idregistro });
            builder.Entity<cPh_Proyecto>().ToTable("PH_PROYECTO", Schema)
                .HasKey(e => new { e.PROYECTO });
            builder.Entity<cPh_FaseProyecto>().ToTable("PH_FASEPROYECTO", Schema)
                .HasKey(e => new { e.PROYECTO, e.FASE });
            builder.Entity<cMarcaAudit>().ToTable("MARCAS_AUDIT", Schema)
                .HasKey(e => new { e.IDREGISTRO });
            builder.Entity<cMarcaDescanso>().ToTable("MARCAS_DESCANSOS", Schema)
                .HasKey(e => new { e.IDREGISTRO });
            builder.Entity<cMarcaIncidencia>().ToTable("MARCAS_INCIDENCIAS", Schema)
                .HasKey(e => new { e.INDICE });
            builder.Entity<cMarcaDistribucion>().ToTable("MARCAS_DISTRIBUCIONES", Schema)
               .HasKey(e => new { e.IDREGISTRO });
            builder.Entity<cPh_Usuario>().ToTable("PH_USUARIO", Schema)
               .HasKey(e => new { e.IDUSUARIO });
            builder.Entity<cPh_Formulacion>().ToTable("PH_FORMULACION", Schema)
                .HasKey(e => new { e.ID });
            builder.Entity<cPh_Horarios>().ToTable("PH_HORARIOS", Schema)
                .HasKey(e => new { e.IDHORARIO });
            builder.Entity<cPh_HorarioTurno>().ToTable("PH_HORARIO_TURNO", Schema)
                .HasKey(e => new { e.IDHORARIO, e.ID_DIA });
            builder.Entity<cTipo_Planilla>().ToTable("TIPOS_PLANILLA", Schema)
                .HasKey(e => new { e.TIPO_PLANILLA });
            builder.Entity<cPh_Transformacion>().ToTable("PH_TRANSFORMACION", Schema)
                .HasKey(e => new { e.ID_TRANSFORMACION });
            builder.Entity<cTransformacion>().ToTable("TRANSFORMACIONES", Schema)
                .HasKey(e => new { e.ID });
            builder.Entity<cTransformacionGlobal>().ToTable("TRANSFORMACIONES_GLOBALES", Schema)
                .HasKey(e => new { e.ID });
            builder.Entity<cTransformacionTipoMarca>().ToTable("TRANSFORMACIONES_TIPO_MARCA", Schema)
                .HasKey(e => new { e.TRANSFORMACIONID });
            builder.Entity<cTransformacionTipoMarcaDet>().ToTable("TRANSFORMACIONES_TIPO_MARCA_DET", Schema)
                .HasKey(e => new { e.TRANSFORMACIONID,  e.IDREGISTRO });
            builder.Entity<cIncidencia_Conf_Pago>().ToTable("INCIDENCIAS_CONF_PAGO", Schema)
                .HasKey(e => new { e.ID });
            builder.Entity<cPh_DescansoTurno>().ToTable("PH_DESCANSOS_TURNOS", Schema)
                .HasKey(e => new { e.IDTURNO, e.IDTIEMPO });
            builder.Entity<cPh_Opciones>().ToTable("PH_OPCIONES", Schema)
                .HasKey(e => new { e.IDOPCION});
            builder.Entity<cPaletaColor>().ToTable("PALETACOLORES", Schema)
               .HasKey(e => new { e.COLORID });
            builder.Entity<cPh_Nivel>().ToTable("PH_NIVELES", Schema)
               .HasKey(e => new { e.IDNIVEL });
            builder.Entity<cMarcaDistribucionConcepto>().ToTable("MARCAS_DISTRIBUCIONES_CONCEPTOS", Schema)
               .HasKey(e => new { e.IDREGISTRO });
            builder.Entity<cMarcaTiempoAdicional>().ToTable("MARCAS_TIEMPO_ADICIONAL", Schema)
               .HasKey(e => new { e.IDREGISTRO });
            builder.Entity<cPh_Distribucion_CCosto>().ToTable("PH_DISTRIBUCIONES_CCOSTO", Schema)
              .HasKey(e => new { e.idregistro });
            builder.Entity<cPh_Puesto>().ToTable("PH_PUESTOS", Schema)
             .HasKey(e => new { e.Puesto });

            builder.Entity<cTemplateHID>().ToTable("TEMPLATESHID", Schema)
              .HasKey(e => new { e.IDNUMERO,e.INDEXID });
            builder.Entity<cTemplateFACE>().ToTable("TEMPLATESFACES", Schema)
              .HasKey(e => new { e.IDNUMERO, e.INDEXID });

            #endregion

            #region Objetos Seguridad y de Portal 



            builder.Entity<cPortal_Config>().ToTable("PORTAL_CONFIG", schemaAdmin)
                .HasKey(e => new { e.IDAPLICACION });
            builder.Entity<cPortal_Menu>().ToTable("PORTAL_MENU", Schema)
                .HasKey(e => new { e.ID });
            builder.Entity<cPortal_Opcion>().ToTable("PORTAL_OPCIONES", Schema)
                .HasKey(e => new { e.ID });
            builder.Entity<cParametroEmail>().ToTable("PARAMETROSEMAIL", Schema)
                .HasKey(e => new { e.Id });
            builder.Entity<cPh_Rol>().ToTable("PH_ROLES", Schema)
                .HasKey(e => new { e.IDROL });
            builder.Entity<cPh_RolTurno>().ToTable("PH_ROLES_TURNOS", Schema)
                .HasKey(e => new { e.IDREGISTRO, e.IDROL });
            builder.Entity<cPortal_Rol>().ToTable("PORTAL_ROLES", Schema)
               .HasKey(e => new { e.ID });
            builder.Entity<cPortal_RolDet>().ToTable("PORTAL_ROLESDET", Schema)
               .HasKey(e => new { e.PORTALROLID, e.PORTALMENUID, e.PORTALOPCIONID });
            builder.Entity<cPh_Opciones>().ToTable("PH_OPCIONES", Schema)
               .HasKey(e => new { e.IDOPCION });
            builder.Entity<cPortal_Empleado>().ToTable("PORTAL_EMPLEADO", Schema)
               .HasKey(e => new { e.IDNUMERO });
            builder.Entity<cPortal_DocMarca>().ToTable("PORTAL_DOCSMARCAS", Schema)
               .HasKey(e => new { e.IDREGISTRO,e.IDNUMERO,e.FECHA });

            #endregion


            #region Organizazion y flujos de Autorizacion

            builder.Entity<cOrganizacionNivel>().ToTable("OrganizacionNiveles", Schema)
              .HasKey(e => new { e.Id });
            builder.Entity<cOrganizacion>().ToTable("Organizacion", Schema)
                .HasKey(e => new { e.Id });
            builder.Entity<cOrganizacionResponsable>().ToTable("OrganizacionResponsables", Schema)
               .HasKey(e => new { e.OrganizacionId, e.OrdenJerarquia });
            builder.Entity<cFlujoAutorizacion>().ToTable("FlujosAutorizacion", Schema)
               .HasKey(e => new { e.Id });
            builder.Entity<cFlujoAutorizacionDetalle>().ToTable("FlujosAutorizacionDetalle", Schema)
               .HasKey(e => new { e.FlujoAutorizacionId, e.NivelOrganizacionId, e.OrdenPrioridad });
            builder.Entity<cEstado>().ToTable("Estados", Schema)
                   .HasKey(e => new { e.Id });
            builder.Entity<cTipoSolicitud>().ToTable("TiposSolicitudes", Schema)
                  .HasKey(e => new { e.Id });
            builder.Entity<cEmpleadoJefatura>().ToTable("EmpleadosJefaturas", Schema)
                  .HasKey(e => new { e.IdNumero });
            builder.Entity<cOrganizacionBaseResponsable>().ToTable("OrganizacionBaseResponsables", Schema)
               .HasKey(e => new { e.IdOrgBase, e.OrdenJerarquia });

            #endregion

            #region Solicitudes y autorizaciones
            builder.Entity<cSolicitud>().ToTable("SOLICITUDES", Schema)
                  .HasKey(e => new { e.Id });
            builder.Entity<cSolicitudDetalle>().ToTable("SOLICITUDESDETALLES", Schema)
                  .HasKey(e => new { e.SolicitudId, e.IdRegistro });
            builder.Entity<cSolicitudAutorizacion>().ToTable("SOLICITUDESAUTORIZACIONES", Schema)
                  .HasKey(e => new { e.SolicitudId,e.EstadoId });
            builder.Entity<cSolicitudConfiguracion>().ToTable("SOLICITUDESCONFIGURACION", Schema)
                 .HasKey(e => new { e.Id });
            #endregion

            #region Vistas para reportes

            builder.Entity<cVHoraExtraXTurno>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("V_HORAS_EXTRA_X_TURNO", Schema);
            });

            builder.Entity<cVHoraLaboradaEmpleado>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("V_HORAS_LABORADAS_EMPLEADOS_RESUMEN", Schema);
            });

            builder.Entity<cVMarcaComedor>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("VMARCAS_COMEDOR", Schema);
            });
            #endregion

            #region LLaves foraneas

            builder.Entity<cEmpleado>()
                .ToTable("EMPLEADOS", Schema)
                .HasOne(e => e.Departamento)
                .WithMany(d => d.Empleado)
                .HasForeignKey(e => new { e.IdDepartamento });
            builder.Entity<cEmpleado>()
                 .ToTable("EMPLEADOS", Schema)
                 .HasOne(e => e.CentroCosto)
                 .WithMany(d => d.Empleado)
                 .HasForeignKey(e => new { e.IdCCosto });
            builder.Entity<cEmpleado>()
                 .ToTable("EMPLEADOS", Schema)
                 .HasOne(e => e.Ph_Planilla)
                 .WithMany(d => d.Empleado)
                 .HasForeignKey(e => new { e.IdPlanilla });
            builder.Entity<cEmpleado>()
                 .ToTable("EMPLEADOS", Schema)
                 .HasOne(e => e.Ph_Grupo)
                 .WithMany(d => d.Empleado)
                 .HasForeignKey(e => new { e.IdGrupo });

            builder.Entity<cPh_HorarioTurno>()
                .ToTable("PH_HORARIO_TURNO", Schema)
                .HasOne(e => e.Ph_Horarios)
                .WithMany(d => d.Ph_HorarioTurno)
                .HasForeignKey(e => new { e.IDHORARIO });

            builder.Entity<cPortal_Opcion>()
               .ToTable("PORTAL_OPCIONES", Schema)
               .HasOne(e => e.cPortal_Menu)
               .WithMany(d => d.cPortal_Opcion)
               .HasForeignKey(e => new { e.PARENTID });

            builder.Entity<cPortal_RolDet>()
                .ToTable("PORTAL_ROLESDET", Schema)
                .HasOne(e => e.cPortal_Rol)
                .WithMany(d => d.cPortal_RolDet)
                .HasForeignKey(e => new { e.PORTALROLID });

            builder.Entity<cTurno>()
                .ToTable("PH_TURNOS", Schema)
                .HasOne(e => e.PaletaColor)
                .WithMany(d => d.Turno)
                .HasForeignKey(e => new { e.ColorId });

           
            builder.Entity<cMarcaIncidencia>()
               .ToTable("MARCAS_INCIDENCIAS", Schema)
               .HasOne(e => e.cIncidencia)
               .WithMany(d => d.cMarcaIncidencias)
               .HasForeignKey(e => new { e.IDINCIDENCIA });

            builder.Entity<cMarcaIncidencia>()
              .ToTable("MARCAS_INCIDENCIAS", Schema)
              .HasOne(e => e.cIncidenciaJust)
              .WithMany(d => d.cMarcaIncidenciasJust)
              .HasForeignKey(e => new { e.INCIDENCIA_JUST });


            builder.Entity<cMarcaDistribucion>()
             .ToTable("MARCAS_DISTRIBUCIONES", Schema)
             .HasOne(e => e.cConcepto)
             .WithMany(d => d.cMarcaDistribucion)
             .HasForeignKey(e => new { e.IDCONCEPTO });

            builder.Entity<cMarcaResumen>()
                .ToTable("MARCAS_RESUMEN", Schema)
                .HasOne(e => e.cConcepto)
                .WithMany(d => d.cMarcaResumen)
                .HasForeignKey(e => new { e.IdConcepto });

            builder.Entity<cAccionPersonal>()
               .ToTable("ACCIONES_PERSONAL", Schema)
               .HasOne(e => e.cEmpleado)
               .WithMany(d => d.cAccionPersonal)
               .HasForeignKey(e => new { e.IdNumero });

            builder.Entity<cAccionPersonal>()
               .ToTable("ACCIONES_PERSONAL", Schema)
               .HasOne(e => e.cIncidencia)
               .WithMany(d => d.cAccionPersonal)
               .HasForeignKey(e => new { e.IdIncidencia });

            builder.Entity<cMarcaProceso>()
               .ToTable("MARCAS_PROCESO", Schema)
               .HasOne(e => e.cEmpleado)
               .WithMany(d => d.cMarcaProcesos)
               .HasForeignKey(e => new { e.idnumero });

            builder.Entity<cMarcaProceso>()
               .ToTable("MARCAS_PROCESO", Schema)
               .HasOne(e => e.cTurno)
               .WithMany(d => d.cMarcaProcesos)
               .HasForeignKey(e => new { e.idturno });

            builder.Entity<cMarcaTiempoAdicional>()
               .ToTable("MARCAS_TIEMPO_ADICIONAL", Schema)
               .HasOne(e => e.cEmpleado)
               .WithMany(d => d.cMarcaTiempoAdicionals)
               .HasForeignKey(e => new { e.IDNUMERO });

            builder.Entity<cMarcaTiempoAdicional>()
               .ToTable("MARCAS_TIEMPO_ADICIONAL", Schema)
               .HasOne(e => e.cCentroCosto)
               .WithMany(d => d.cMarcaTiempoAdicionals)
               .HasForeignKey(e => new { e.CENTRO_COSTO });

            builder.Entity<cMarcaTiempoAdicional>()
               .ToTable("MARCAS_TIEMPO_ADICIONAL", Schema)
               .HasOne(e => e.cConcepto)
               .WithMany(d => d.cMarcaTiempoAdicionals)
               .HasForeignKey(e => new { e.IDCONCEPTO });

            builder.Entity<cPh_OpcionSistema>()
              .ToTable("PH_OPCIONES_SISTEMA", schemaAdmin)
              .HasOne(e => e.cPh_MenuSistema)
              .WithMany(d => d.cPh_OpcionSistema)
              .HasForeignKey(e => new { e.PARENTID });

            builder.Entity<cPh_RolSistemaDet>()
              .ToTable("PH_ROLES_SISTEMADET", schemaAdmin)
              .HasOne(e => e.cPh_RolSistema)
              .WithMany(d => d.cPh_RolSistemaDet)
              .HasForeignKey(e => new { e.ROLSISTEMAID });

            builder.Entity<cMarcaMovTurno>()
                .ToTable("MARCAS_MOV_TURNOS", Schema)
                .HasOne(e => e.cEmpleado)
                .WithMany(d => d.cMarcaMovTurno)
                .HasForeignKey(e => new { e.idnumero });            

            builder.Entity<cMarcaMovTurno>()
                .ToTable("MARCAS_MOV_TURNOS", Schema)
                .HasOne(e => e.cTurno)
                .WithMany(d => d.cMarcaMovTurno)
                .HasForeignKey(e => new { e.turno });

            builder.Entity<cMarcaMovTurno>()
                .ToTable("MARCAS_MOV_TURNOS", Schema)
                .HasOne(e => e.cPh_Planilla)
                .WithMany(d => d.cMarcaMovTurno)
                .HasForeignKey(e => new { e.idplanilla });


            builder.Entity<cMarcaMovTurnoBitacora>()
                .ToTable("MARCAS_MOV_TURNOS_BITACORA", Schema)
                .HasOne(e => e.cEmpleado)
                .WithMany(d => d.cMarcaMovTurnoBitacora)
                .HasForeignKey(e => new { e.idnumero });
            builder.Entity<cMarcaMovTurnoBitacora>()
                .ToTable("MARCAS_MOV_TURNOS_BITACORA", Schema)
                .HasOne(e => e.cTurno)
                .WithMany(d => d.cMarcaMovTurnoBitacora)
                .HasForeignKey(e => new { e.turno });
            builder.Entity<cMarcaMovTurnoBitacora>()
                .ToTable("MARCAS_MOV_TURNOS_BITACORA", Schema)
                .HasOne(e => e.cPh_Planilla)
                .WithMany(d => d.cMarcaMovTurnoBitacora)
                .HasForeignKey(e => new { e.idplanilla });           


            builder.Entity<cOrganizacion>()
                .ToTable("Organizacion", Schema)
                .HasOne(e => e.cOrganizacionNivel)
                .WithMany(d => d.cOrganizacion)
                .HasForeignKey(e => new { e.NivelOrganizacionId });

            builder.Entity<cOrganizacionResponsable>()
                .ToTable("OrganizacionResponsables", Schema)
                .HasOne(e => e.cOrganizacion)
                .WithMany(d => d.cOrganizacionResponsable)
                .HasForeignKey(e => new { e.OrganizacionId });

            builder.Entity<cFlujoAutorizacionDetalle>()
               .ToTable("FlujosAutorizacionDetalle", Schema)
               .HasOne(e => e.cFlujoAutorizacion)
               .WithMany(d => d.cFlujoAutorizacionDetalle)
               .HasForeignKey(e => new { e.FlujoAutorizacionId });
            builder.Entity<cFlujoAutorizacionDetalle>()
               .ToTable("FlujosAutorizacionDetalle", Schema)
               .HasOne(e => e.cOrganizacionNivel)
               .WithMany(d => d.cFlujoAutorizacionDetalle)
               .HasForeignKey(e => new { e.NivelOrganizacionId });

            builder.Entity<cTipoSolicitud>()
             .ToTable("TiposSolicitudes", Schema)
             .HasOne(e => e.cSolicitudConfiguracion)
             .WithMany(d => d.cTipoSolicitud)
             .HasForeignKey(e => new { e.TipoConfiguracion });

            builder.Entity<cSolicitud>()
              .ToTable("SOLICITUDES", Schema)
              .HasOne(e => e.cEstado)
              .WithMany(d => d.cSolicitud)
              .HasForeignKey(e => new { e.EstadoId });
            builder.Entity<cSolicitud>()
              .ToTable("SOLICITUDES", Schema)
              .HasOne(e => e.cTipoSolicitud)
              .WithMany(d => d.cSolicitud)
              .HasForeignKey(e => new { e.TipoSolicitudId });
            builder.Entity<cSolicitud>()
              .ToTable("SOLICITUDES", Schema)
              .HasOne(e => e.cCentroCosto)
              .WithMany(d => d.cSolicitud)
              .HasForeignKey(e => new { e.IdCCosto });

            builder.Entity<cSolicitudDetalle>()
              .ToTable("SOLICITUDESDETALLES", Schema)
              .HasOne(e => e.cSolicitud)
              .WithMany(d => d.cSolicitudDetalle)
              .HasForeignKey(e => new { e.SolicitudId });

            builder.Entity<cSolicitudAutorizacion>()
             .ToTable("SOLICITUDESAUTORIZACIONES", Schema)
             .HasOne(e => e.cSolicitud)
             .WithMany(d => d.cSolicitudAutorizacion)
             .HasForeignKey(e => new { e.SolicitudId });


            #endregion





        }

    }
}