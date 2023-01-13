namespace GeoTimeConnectWebApi.Data.Interfaz
{
    public interface IDbContextSchema
    {
        string? Schema { get; }
        DateTime? _IModelChanged { get; set; }
        string _AssemblyName { get; set; }
    }
}
