using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Data.Interfaz;

public interface ISincronizaErpFactory
{
    public SincronizaErp Create(GeoTimeConnectService geoConnect, IErpConnectService erpConnect);
}
