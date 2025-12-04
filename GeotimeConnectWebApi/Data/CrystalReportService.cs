using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public class CrystalReportService : ICrystalReportService
{
    public byte[] GeneratePdfReport(string rptPath, Dictionary<string, object>? parameters = null)
    {
        using ReportDocument reportDocument = new ReportDocument();
        reportDocument.Load(rptPath);

        // Asignar parámetros si existen
        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                reportDocument.SetParameterValue(param.Key, param.Value);
            }
        }

        // Exportar a PDF y devolver el arreglo de bytes
        using var stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }
}
