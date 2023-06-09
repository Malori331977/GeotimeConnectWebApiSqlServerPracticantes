
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Update.Internal;
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

            schemaAdmin= config.GetConnectionString("SchemaAdmin");

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

		protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<cPh_Login>().ToTable("PH_LOGIN", schemaAdmin)
                .HasKey(e => new { e.idusuario });
            builder.Entity<cPh_Compania>().ToTable("PH_COMPANIAS", schemaAdmin)
                .HasKey(e => new { e.idcomp });

            builder.Entity<cAccionPersonal>().ToTable("Acciones_Personal", Schema)
                .HasKey(e => new { e.IdRegistro});
            builder.Entity<cCentroCosto>().ToTable("Ph_CCostos", Schema)
                 .HasKey(e => new { e.IdCCosto });
            builder.Entity<cConcepto>().ToTable("Ph_Conceptos", Schema)
                 .HasKey(e => new { e.Concepto });
            builder.Entity<cDepartamento>().ToTable("Ph_Departamento", Schema)
                .HasKey(e => new { e.IdDepart });
            builder.Entity<cEmpleado>().ToTable("Empleados", Schema)
                .HasKey(e => new { e.IdNumero });
            builder.Entity<cIncidencia>().ToTable("Incidencias", Schema)
                .HasKey(e => new { e.Codigo });
            builder.Entity<cMarcaResumen>().ToTable("Marcas_Resumen", Schema)
                .HasKey(e => new { e.IdPlanilla,e.IdNumero,e.IdConcepto,e.IdCCosto });
            builder.Entity<cTurno>().ToTable("Ph_Turnos", Schema)
                .HasKey(e => new { e.IdTurno });
            builder.Entity<cMarca>().ToTable("Marcas", Schema)
                .HasKey(e => new { e.registro });
            builder.Entity<cMarcaMovTurno>().ToTable("Marcas_Mov_Turnos", Schema)
               .HasKey(e => new { e.idregistro });

            builder.Entity<cPh_Grupo>().ToTable("Ph_Grupos", Schema)
                 .HasKey(e => new { e.idgrupo });
			builder.Entity<cPh_Periodos>().ToTable("Ph_Periodos", Schema)
				.HasKey(e => new { e.idperiodo });

		}

      
    }
}