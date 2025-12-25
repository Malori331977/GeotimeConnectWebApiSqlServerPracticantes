using com.gsitcr.geotime.Data;

public interface IGeoTimeConnectServiceFactory
{
    public GeoTimeConnectService Create(string schema, string bdname);
}
