using Pos.Web.Infrastructure.Documents;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Dtos;
using QuestPDF.Fluent;

namespace Pos.Web.Infrastructure.Services
{
    public class QuestPdfInvoiceGenerator : IInvoiceGenerator
    {
        private readonly IWebHostEnvironment _env;

        public QuestPdfInvoiceGenerator(IWebHostEnvironment env)
        {
            _env = env;
        }

        public byte[] Generate(InvoiceModel model)
        {
            // 1. Load Logo (Ensure you have a logo.png in wwwroot/assets)
            var logoPath = Path.Combine(_env.WebRootPath, "assets", "logo.png");
            byte[] logoBytes = File.Exists(logoPath) ? File.ReadAllBytes(logoPath) : new byte[0];

            // 2. Create Document
            var document = new InvoiceDocument(model, logoBytes);

            // 3. Render to Bytes
            return document.GeneratePdf();
        }
    }
}
