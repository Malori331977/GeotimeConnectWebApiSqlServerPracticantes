using com.gsitcr.geotime.Data;

public interface IErpConnectServiceFactory
{
    public ErpConnectServices Create(string schema, string bdname);
}
