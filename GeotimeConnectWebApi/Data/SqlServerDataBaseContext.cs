
using Microsoft.EntityFrameworkCore;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using GeotimeConnectWebApi.Models;


namespace GeoTimeConnectWebApi.Data
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
        public DbSet<cIncidencia_Conf_Pago> Incidencias_Conf_Pago { get; set; }
        public DbSet<cPortal_Rol> Portal_Rol { get; set; }
        public DbSet<cPortal_RolDet> Portal_RolDet { get; set; }
        public DbSet<cPh_DescansoTurno> Ph_Descansos_Turnos { get; set; }
        public DbSet<cPh_Opciones> Ph_Opciones { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(schemaAdmin);

            builder.Entity<cPh_Login>().ToTable("PH_LOGIN", schemaAdmin)
                .HasKey(e => new { e.idusuario });
            builder.Entity<cPh_Compania>().ToTable("PH_COMPANIAS", schemaAdmin)
                .HasKey(e => new { e.IDCOMP });

            builder.Entity<cAccionPersonal>().ToTable("ACCIONES_PERSONAL", Schema)
                .HasKey(e => new { e.IdRegistro});
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
                .HasKey(e => new { e.IdPlanilla,e.IdNumero,e.IdConcepto,e.IdCCosto });
            builder.Entity<cTurno>().ToTable("PH_TURNOS", Schema)
                .HasKey(e => new { e.IdTurno });
            builder.Entity<cMarca>().ToTable("MARCAS", Schema)
                .HasKey(e => new { e.registro });
            builder.Entity<cMarcaMovTurno>().ToTable("MARCAS_MOV_TURNOS", Schema)
               .HasKey(e => new { e.idregistro });
            builder.Entity<cPh_Grupo>().ToTable("PH_GRUPOS", Schema)
                 .HasKey(e => new { e.idgrupo });
			builder.Entity<cPh_Periodos>().ToTable("PH_PERIODOS", Schema)
				.HasKey(e => new { e.idperiodo });
            builder.Entity<cPh_Planilla>().ToTable("PH_PLANILLA", Schema)
                .HasKey(e => new { e.idplanilla });
            builder.Entity<cMarcaIn>().ToTable("MARCAS_IN", Schema)
                .HasKey(e => new { e.idtarjeta, e.fecha,e.hora,e.idnumero,e.tipo });
            builder.Entity<cMarcaExtraApb>().ToTable("MARCAS_EXTRAS_APB", Schema)
                .HasKey(e => new {e.idregistro});
            builder.Entity<cMarcaProceso>().ToTable("MARCAS_PROCESO", Schema)
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
                .HasNoKey();
            builder.Entity<cPh_Usuario>().ToTable("PH_USUARIO", Schema)
                .HasKey(e => new { e.IDUSUARIO });
            builder.Entity<cPh_Sistema>().ToTable("PH_SISTEMA", schemaAdmin)
                .HasNoKey();
            builder.Entity<cPortal_Config>().ToTable("PORTAL_CONFIG", Schema)
                .HasKey(e => new { e.ID });
            builder.Entity<cPortal_Menu>().ToTable("PORTAL_MENU", Schema)
                .HasKey(e => new { e.ID });
            builder.Entity<cPortal_Opcion>().ToTable("PORTAL_OPCIONES", Schema)
                .HasKey(e => new { e.ID });
            builder.Entity<cPh_Formulacion>().ToTable("PH_FORMULACION", Schema)
                .HasKey(e => new { e.ID });
            builder.Entity<cParametroEmail>().ToTable("PARAMETROSEMAIL", Schema)
                .HasKey(e => new { e.Id });
            builder.Entity<cPh_Horarios>().ToTable("PH_HORARIOS", Schema)
                .HasKey(e => new { e.IDHORARIO });
            builder.Entity<cPh_HorarioTurno>().ToTable("PH_HORARIO_TURNO", Schema)
                .HasKey(e => new { e.IDHORARIO, e.ID_DIA });
            builder.Entity<cTipo_Planilla>().ToTable("TIPOS_PLANILLA", Schema)
                .HasKey(e => new { e.TIPO_PLANILLA });
            builder.Entity<cPh_Transformacion>().ToTable("PH_TRANSFORMACION", Schema)
                .HasKey(e => new { e.ID_TRANSFORMACION });
            builder.Entity<cPh_Rol>().ToTable("PH_ROLES", Schema)
                .HasKey(e => new { e.IDROL });
            builder.Entity<cPh_RolTurno>().ToTable("PH_ROLES_TURNOS", Schema)
                .HasKey(e => new { e.IDREGISTRO, e.IDROL });
            builder.Entity<cTransformacion>().ToTable("TRANSFORMACIONES", Schema)
                .HasKey(e => new { e.ID });
            builder.Entity<cTransformacionGlobal>().ToTable("TRANSFORMACIONES_GLOBALES", Schema)
                .HasKey(e => new { e.ID });
            builder.Entity<cIncidencia_Conf_Pago>().ToTable("INCIDENCIAS_CONF_PAGO", Schema)
                .HasKey(e => new { e.ID });
            builder.Entity<cPortal_Opcion>().ToTable("PORTAL_ROL", Schema)
                .HasKey(e => new { e.ID});
            builder.Entity<cPortal_RolDet>().ToTable("PORTAL_ROLDET", Schema)
               .HasKey(e => new { e.PORTALROLID,e.PORTALMENUID,e.PORTALOPCIONID });
            builder.Entity<cPh_DescansoTurno>().ToTable("PH_DESCANSOS_TURNOS", Schema)
                .HasKey(e => new { e.IDTURNO, e.IDTIEMPO });
            builder.Entity<cPh_Opciones>().ToTable("PH_OPCIONES", Schema)
                .HasNoKey();


            //llaves foraneas
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
                .ToTable("PORTAL_ROLDET", Schema)
                .HasOne(e => e.cPortal_Rol)
                .WithMany(d => d.cPortal_RolDet)
                .HasForeignKey(e => new { e.PORTALROLID });

        }

    }
}