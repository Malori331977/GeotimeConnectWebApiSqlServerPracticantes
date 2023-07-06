
using Microsoft.EntityFrameworkCore;
using GeoTimeConnectWebApi.Data.Interfaz;
using GeoTimeConnectWebApi.Models;
using System.Data.Entity.Infrastructure;
using System.Reflection;
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

        }

      
    }
}