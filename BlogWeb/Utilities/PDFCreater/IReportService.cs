using CoreLayer.Services.FileManagment;
using CoreLayer.Utilities;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWeb.Utilities.PDFCreater
{
    public interface IReportService
    {
        public OperationResult GeneratePdfReport(PDFObject pdf);


    }

    public class ReportService : IReportService
    {
        private readonly IConverter _converter;

        public ReportService(IConverter converter)
        {
            _converter = converter;

        }
        public OperationResult GeneratePdfReport(PDFObject pdf)
        {
            var html = $@"<!DOCTYPE html>
   <html lang=""en""  dir=""rtl"">
   <head>
       {pdf.Title}
   </head>
  <body>
<h1>{pdf.Title}</h1>
{pdf.Description}
 </body>
  </html>";

            GlobalSettings globalSettings = new GlobalSettings();
            globalSettings.ColorMode = ColorMode.Color;
            globalSettings.Orientation = Orientation.Portrait;
            globalSettings.PaperSize = PaperKind.A4;
            globalSettings.Margins = new MarginSettings { Top = 25, Bottom = 25 };
            globalSettings.Out = $@"C:\Users\Amir\source\repos\SampleWeblog\BlogWeb\wwwroot\PostPdf\{pdf.Slug}.pdf";
            globalSettings.DocumentTitle = pdf.Title;
            ObjectSettings objectSettings = new ObjectSettings();
            objectSettings.PagesCount = true;
            objectSettings.HtmlContent = html;
            WebSettings webSettings = new WebSettings();
            webSettings.DefaultEncoding = "utf-8";
            HeaderSettings headerSettings = new HeaderSettings();
            headerSettings.FontSize = 15;
            headerSettings.FontName = "Ariel";
            headerSettings.Right = "Page [page] of [toPage]";
            headerSettings.Line = true;
            FooterSettings footerSettings = new FooterSettings();
            footerSettings.FontSize = 12;
            footerSettings.FontName = "Ariel";
            footerSettings.Center = "محتویات این پست متعلق به سایت BLogSample میباشد";
            footerSettings.Line = true;
            objectSettings.HeaderSettings = headerSettings;
            objectSettings.FooterSettings = footerSettings;
            objectSettings.WebSettings = webSettings;
            HtmlToPdfDocument htmlToPdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };
            _converter.Convert(htmlToPdfDocument);
            return OperationResult.Success();
        }

    }

    public class PDFObject
    {
        public string? Description { get; set; }
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? ImageAlt { get; set; }

    }
}
