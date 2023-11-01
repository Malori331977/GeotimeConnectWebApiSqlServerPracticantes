
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
        public DbSet<cPortal_Opcion> Portal_Opciones { get; set; }
        public DbSet<cPh_Formulacion> Ph_Formulacion { get; set; }
        public DbSet<cParametroEmail> ParametrosEmail { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(schemaAdmin);

            builder.Entity<cPh_Login>().ToTable("PH_LOGIN", schemaAdmin)
                .HasKey(e => new { e.idusuario });
            builder.Entity<cPh_Compania>().ToTable("PH_COMPANIAS", schemaAdmin)
                .HasKey(e => new { e.idcomp });

            builder.Entity<cAccionPersonal>().ToTable("ACCIONES_PERSONAL", Schema)
                .HasKey(e => new { e.IdRegistro});
            builder.Entity<cCentroCosto>().ToTable("PH_CCOSTOS", Schema)
                 .HasKey(e => new { e.IdCCosto });
            builder.Entity<cConcepto>().ToTable("PH_CONCEPTOS", Schema)
                 .HasKey(e => new { e.Concepto });
            builder.Entity<cDepartamento>().ToTable("PH_DEPARTAMENTO", Schema)
                .HasKey(e => new { e.IDDEPART });
            builder.Entity<cEmpleado>().ToTable("EMPLEADOS", Schema)
                .HasKey(e => new { e.IdNumero });
            builder.Entity<cIncidencia>().ToTable("INCIDENCIAS", Schema)
                .HasKey(e => new { e.Codigo });
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
            builder.Entity<cPortal_Opcion>().ToTable("PORTAL_OPCIONES", Schema)
           .HasKey(e => new { e.ID });
            builder.Entity<cPh_Formulacion>().ToTable("PH_FORMULACION", Schema)
           .HasKey(e => new { e.ID });
            builder.Entity<cParametroEmail>().ToTable("PARAMETROSEMAIL", Schema)
            .HasKey(e => new { e.Id });


        }


    }
}