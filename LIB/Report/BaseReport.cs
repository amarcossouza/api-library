using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Colors;
using iText.Layout.Element;
using iText.IO.Image;
using System.IO;

namespace LIB.Report
{
    public abstract class BaseReport
    {
        protected PdfWriter writer;
        protected PageSize pageSize;
        protected Color red = DeviceRgb.RED;
        protected Color blue = DeviceRgb.BLUE;
        protected Color black = DeviceRgb.BLACK;
        protected PdfCanvas canvas;
        protected PdfDocument pdf;
        protected Document doc;


        private void CreatePDFDoc(Stream stream)
        {
            writer = new PdfWriter(stream);
            writer.SetCloseStream(false);
            pdf = new PdfDocument(writer);
            doc = new Document(pdf);
        }
        

        protected void CreatePDFDocA4(Stream stream)
        {
            CreatePDFDoc(stream);
            pageSize = new PageSize(PageSize.A4);
        }

        protected void CreatePDFDocA3(Stream stream)
        {
            CreatePDFDoc(stream);
            pageSize = new PageSize(PageSize.A3);
        }

        protected void NewPDFPage(int index)
        {
            canvas = new PdfCanvas(pdf.AddNewPage(index));
        }

        protected void AppendPDFPage(string content,float xPosition, float yPosition, float width, Color color, float fontSize = 10)
        {
            doc.Add(new Paragraph(content).SetFixedPosition(xPosition, yPosition, width).SetFontSize(fontSize).SetFontColor(color));
        }

        protected void AppendPDFPage(Paragraph paragraph)
        {
            doc.Add(paragraph);
        }

        protected void BackGroundImage(string path)
        {
            canvas.AddImage(ImageDataFactory.Create(path), pageSize, false);
        }
    }
}