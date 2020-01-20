using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estrutura.Reports
{
    public static class ReportHelpers
    {
        public static byte[] GeReportStreamPDF(string reportName, string dataSetName, object dataSet, string reportPath, List<Microsoft.Reporting.WebForms.ReportParameter> parameters = null, Dictionary<string, object> dataSets = null)
        {
            Microsoft.Reporting.WebForms.LocalReport report = new Microsoft.Reporting.WebForms.LocalReport();
            report.EnableExternalImages = true;
            report.ReportPath = reportPath + "/" + reportName + ".rdlc";
            if (parameters.Count() > 0)
                report.SetParameters(parameters);


            ReportDataSource rds = new ReportDataSource();
            rds.Name = dataSetName;
            rds.Value = dataSet;
            report.DataSources.Add(rds);

            if (dataSets != null)
            {
                foreach (KeyValuePair<string, object> kvp in dataSets)
                {
                    ReportDataSource reportDataSource = new ReportDataSource();
                    reportDataSource.Name = kvp.Key;
                    reportDataSource.Value = kvp.Value;
                    report.DataSources.Add(reportDataSource);
                }
            }

            string deviceInfo = null; // http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string mimeType;
            string encoding;
            string fileNameExtension;
            string[] streams;
            Microsoft.Reporting.WebForms.Warning[] warnings;

            byte[] renderedBytes = report.Render(
               "PDF",
               deviceInfo,
               out mimeType,
               out encoding,
               out fileNameExtension,
               out streams,
               out warnings);

            return renderedBytes;
        }

    }
}
