using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RazorToPdf
{
    public static class MvcRazorToPdfExtensions
    {
        public static byte[] GeneratePdf(this ControllerContext context, object model = null, string viewName = null,
            Action<PdfWriter, Document> configureSettings = null)
        {
            return new RazorToPdf().GeneratePdfOutput(context, model, viewName, configureSettings);
        }
    }
}
