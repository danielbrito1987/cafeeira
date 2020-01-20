using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reporting.WebForms;

namespace Estrutura.Reports
{
    public class ReportHelper
    {
        public static ReportConfig RenderReport<T>(List<string> selectedFields, string reportType, IEnumerable vals)
        {
            LocalReport localReport = new LocalReport();
            localReport.LoadReportDefinition(ReportHelper.GenerateReport<T>(selectedFields));
            localReport.DataSources.Add(new ReportDataSource("MyData", vals));

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo = @"<DeviceInfo>
                                     <OutputFormat>PDF</OutputFormat>
                                     <PageWidth>9in</PageWidth>
                                     <PageHeight>11in</PageHeight>
                                     <MarginTop>0.7in</MarginTop>
                                     <MarginLeft>2cm</MarginLeft>
                                     <MarginRight>2cm</MarginRight>
                                     <MarginBottom>0.7in</MarginBottom>
                                  </DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] bytes;

            //Renderiza o relatório em bytes
            bytes = localReport.Render(reportType,
                                       deviceInfo,
                                       out mimeType,
                                       out encoding,
                                       out fileNameExtension,
                                       out streams,
                                       out warnings);

            return new ReportConfig { Bytes = bytes, MimeType = mimeType };
        }

        public static MemoryStream GenerateReport<T>(List<string> selectedFields)
        {
            List<string> allFields = typeof(T).GetProperties().Select(pi => pi.Name).ToList(); //get fields name
            return GenerateRdl(allFields, selectedFields);
        }

        private static MemoryStream GenerateRdl(List<string> allFields, List<string> selectedFields)
        {
            MemoryStream ms = new MemoryStream();
            RdlGenerator gen = new RdlGenerator();
            gen.AllFields = allFields;
            gen.SelectedFields = selectedFields;
            gen.WriteXml(ms);
            ms.Position = 0;
            return ms;
        }
    }

    public class ReportConfig
    {
        public byte[] Bytes;

        public string MimeType;
    }

}
