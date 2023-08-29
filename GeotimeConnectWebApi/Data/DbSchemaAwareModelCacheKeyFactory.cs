using GeoTimeConnectWebApi.Data.Interfaz;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GeoTimeConnectWebApi.Data
{
    public class DbSchemaAwareModelCacheKeyFactory : IModelCacheKeyFactory
    {

        public object Create(DbContext context, bool designTime)
        {
            return new
            {
                Type = context.GetType(),
                IModelChanged = context is IDbContextSchema schema ? schema._IModelChanged : null,
                AssemblyName = context is IDbContextSchema schema2 ? schema2._AssemblyName : null,
                Schema = context is IDbContextSchema schema3 ? schema3.Schema : null
            };
        }
    }
}
