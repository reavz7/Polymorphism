using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt_Polimorfizm_Stepien
{
    public class BiletNormalny : Bilet
    {
        public BiletNormalny()
        {
            Nazwa = "Normalny";
            Cena = 20m; // przykładowa cena
        }

        public override void GenerujPDF(int ilosc)
        {
            // okreslenie sciezki do pulpitu bieżącego użytkownika
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Generowanie unikalnej nazwy pilku z użyciem GUID
            string uniqueFileName = Guid.NewGuid().ToString();
            string pdfPath = Path.Combine(desktopPath, $"BiletNormalny_{uniqueFileName}.pdf");

            using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                // Generowanie losowego numeru bileut
                string numerBiletu = GenerujNumerBiletu();
                // Dodawanie treści do dokumentu PDF
                document.Add(new Paragraph($"Numer biletu: {numerBiletu}"));
                document.Add(new Paragraph($"Bilet: {Nazwa}"));
                document.Add(new Paragraph($"Cena za sztuke: {Cena} PLN"));
                document.Add(new Paragraph($"Ilosc: {ilosc}"));
                document.Add(new Paragraph($"Calkowity koszt: {Cena * ilosc} PLN"));

                document.Close();
                writer.Close();
            }

            MessageBox.Show($"Plik PDF został zapisany na Pulpicie: {pdfPath}");
        }
    }
}
