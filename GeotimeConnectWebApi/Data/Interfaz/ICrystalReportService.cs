using com.gsitcr.geotime.Data;
using com.gsitcr.geotime.Models.Response;
using GeotimeModelsLib.Models;

public interface ICrystalReportService
{
    /// <summary>
    /// Generates a PDF report based on the specified report name and filter criteria.
    /// </summary>
    /// <param name="rptName">The name of the report to generate. Cannot be null or empty.</param>
    /// <param name="filtro">An optional filter object that specifies criteria to apply to the report data. If null, the report is generated
    /// without additional filtering.</param>
    /// <returns>An EventResponse object containing the result of the report generation, including the generated PDF data or
    /// error information.</returns>
    public Task<EventResponse> GeneratePdfReport(cFiltroReporte filtro);
}
