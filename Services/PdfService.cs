using DinkToPdf;
using DinkToPdf.Contracts;

namespace ToyotaWeb.Services
{
    public class PdfService
    {
        private readonly IConverter _converter;

        public PdfService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GenerateSalaryPdf(string html)
        {
            var document = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },

                Objects =
                {
                    new ObjectSettings()
                    {
                        HtmlContent = html
                    }
                }
            };

            return _converter.Convert(document);
        }
    }
}