namespace com.gsitcr.geotime.Data.Interfaz
{
    public interface IDbContextSchema
    {
        string? Schema { get; }
        DateTime? _IModelChanged { get; set; }
        string _AssemblyName { get; set; }
    }
}
