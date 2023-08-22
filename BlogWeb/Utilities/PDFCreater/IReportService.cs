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

        OperationResult DeletePdf(string fileName, string path);
    }

    public class ReportService : IReportService
    {
        private readonly IConverter _converter;
        private readonly IFileManager _fileManager;
        public ReportService(IConverter converter, IFileManager fileManager)
        {
            _converter = converter;
            _fileManager = fileManager;
        }

        public OperationResult DeletePdf(string fileName, string path)
        {
            _fileManager.DeleteFile(fileName, path);
            return OperationResult.Success();
        }

        public OperationResult GeneratePdfReport(PDFObject pdf)
        {
            var html = $@"<!DOCTYPE html>
   <html lang=""en""  dir=""rtl"">
   <head>
       نویسنده : {pdf.Writer}
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
            //globalSettings.Out = $@"C:\Users\Amir\source\repos\SampleWeblog\BlogWeb\wwwroot\PostPdf\{pdf.Slug}.pdf";
            //globalSettings.Out = $@"C:\C#\test\test\SampleWeblog\BlogWeb\wwwroot\PostPdf\{pdf.Slug}.pdf";
            globalSettings.Out = Path.Combine(Directory.GetCurrentDirectory(),$@"wwwroot\PostPdf\{pdf.Slug}.pdf");

            globalSettings.DocumentTitle = pdf.Writer;
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
        public string Writer { get; set; }
    }
}
