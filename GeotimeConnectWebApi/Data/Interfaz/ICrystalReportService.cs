public interface ICrystalReportService
{
    /// <summary>
    /// Genera un reporte PDF a partir de un archivo .rpt y parámetros opcionales.
    /// </summary>
    /// <param name="rptPath">Ruta física del archivo .rpt</param>
    /// <param name="parameters">Diccionario de parámetros para el reporte</param>
    /// <returns>Arreglo de bytes del PDF generado</returns>
    byte[] GeneratePdfReport(string rptPath, Dictionary<string, object>? parameters = null);
}
