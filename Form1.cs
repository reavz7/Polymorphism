using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Projekt_Polimorfizm_Stepien
{
    public partial class Form1 : Form
    {
        //Jeśli będzie chciał Pan otworzyć ten projekt u siebie, będzie trzeba zmienić  DESKTOP-D705V9A  na nazwę urządzenia na którym otwierany jest projekt
        private string connectionString = @"Server=DESKTOP-D705V9A;Database=SystemBiletowy;Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();

        }

        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Bilety", connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                    //tutaj z bazy danych wyjalem wszystkie wartosci z bazy danych z tabelki Bilety zeby wrzucic kazdy rodzaj do datagrida
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Proszę wybrać bilet z listy.");
                return;
            }

            // ten string nazwabiletu przypisuje sie do zaznaczoneg Row z datagrida a ilosc oczywisice z numericupdowna
            string nazwaBiletu = dataGridView1.CurrentRow.Cells["Nazwa"].Value.ToString();
            int ilosc = (int)numericUpDown1.Value; // pobiera ilosc biletow

            Bilet bilet = null;

            // switch, zeby nie bylo 10000 ifów, w przypadku konkrentego biletu  wartosc bilet z klasy Bilet ustawia sie jako odpowiedni rodzaj biletu zaleznie od tego co wybral uzytkownik
            switch (nazwaBiletu)
            {
                case "Bilet Normalny":
                    bilet = new BiletNormalny();
                    break;
                case "Bilet Ulgowy":
                    bilet = new BiletUlgowy();
                    break;
                case "Bilet Rodzinny":
                    bilet = new BiletRodzinny();
                    break;
                case "Bilet VIP":
                    bilet = new BiletVIP();
                    break;
                default:
                    MessageBox.Show("Nieznany typ biletu.");
                    return;
            }

            // A tutaj na koniec wywolujemy funcje ktora jest w zasadzie najwazniejsza bo to ona przedstawia polimorfizm
            //czyli GenerujPDF, dzieki polimorfizmowi mozemy napisac po prostu .GenerujPDF zamiast tu pisac milion razy taka sama funkcje
            if (bilet != null)
            {
                bilet.GenerujPDF(ilosc);
                MessageBox.Show($"Plik PDF dla {ilosc}x '{nazwaBiletu}' został wygenerowany.");
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }

}