using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.QrCode;

namespace Projekt_Polimorfizm_Stepien
{
    public class BiletVIP : Bilet
    {
        public BiletVIP()
        {
            Nazwa = "Bilet VIP";
            Cena = 100.00m; 
        }

        public override void GenerujPDF(int ilosc)
        {
            
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string uniqueFileName = Guid.NewGuid().ToString();
            string pdfPath = Path.Combine(desktopPath, $"BiletVIP_{uniqueFileName}.pdf");

            using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                string numerBiletu = GenerujNumerBiletu();

                document.Add(new Paragraph($"Numer biletu: {numerBiletu}"));
                document.Add(new Paragraph($"Bilet: {Nazwa}"));
                document.Add(new Paragraph($"Cena za sztuke: {Cena} PLN"));
                document.Add(new Paragraph($"Ilosc: {ilosc}"));
                document.Add(new Paragraph($"Calkowity koszt: {Cena * ilosc} PLN"));

                var qrWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        Height = 200,
                        Width = 200
                    }
                };

                // Możesz umieścić w kodzie QR dowolną zawartość, na przykład numer biletu
                var qrCodeImage = qrWriter.Write(numerBiletu);

                // Konwersja System.Drawing.Image na iTextSharp.text.Image
                using (MemoryStream ms = new MemoryStream())
                {
                    qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    var iTextImage = iTextSharp.text.Image.GetInstance(ms.ToArray());
                    document.Add(iTextImage);
                }

                document.Close();
                writer.Close();
            }

            MessageBox.Show($"Plik PDF został zapisany na Pulpicie: {pdfPath}");
        }
    }
}
