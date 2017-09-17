using PdfSharp;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace BakeryPR.Utilities
{
    public class PDFReader
    {
        public String GetproductionReporttemplate()
        {
            return File.ReadAllText("productionTemplate.html");
        }

        public async Task GetProductionReport(string template, String path)
        {
            await Task.Run(() =>
            {
                PdfDocument pdf = PdfGenerator.GeneratePdf(template, PageSize.A4);
                pdf.Save(path);
            });           
        }
    }
}
