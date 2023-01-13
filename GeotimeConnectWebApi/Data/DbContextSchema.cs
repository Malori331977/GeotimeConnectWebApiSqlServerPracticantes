using GeoTimeConnectWebApi.Data.Interfaz;
using System;
using System.Reflection;

namespace GeoTimeConnectWebApi.Data
{
	public class DbContextSchema : IDbContextSchema
	{
		public string Schema { get; }
        public DateTime? _IModelChanged { get; set; }
        public string _AssemblyName { get; set; }

        public DbContextSchema(string schema, DateTime IModelChanged, string AssemblyName)
		{
			Schema = schema ?? throw new ArgumentNullException(nameof(schema));
            _IModelChanged = IModelChanged;
            _AssemblyName = AssemblyName;
        }
    }
}