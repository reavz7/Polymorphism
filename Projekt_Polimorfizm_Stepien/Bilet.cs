using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.QrCode;

namespace Projekt_Polimorfizm_Stepien
{
    public abstract class Bilet
    {
        public string Nazwa { get; set; }
        public decimal Cena { get; set; }

        public abstract void GenerujPDF(int ilosc);

        protected string GenerujNumerBiletu()
        {
            var random = new Random();
            string numerBiletu = "";
            for (int i = 0; i < 10; i++)
            {
                numerBiletu += random.Next(0, 10).ToString();
            }
            return numerBiletu;
        }
    }

}
