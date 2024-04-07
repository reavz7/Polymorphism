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


                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.DataBoundItem != null)
                        {
                            DataRowView drv = (DataRowView)row.DataBoundItem;
                            row.Tag = drv["Id"]; // "Id" to kolumna identyfikująca typ biletu
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }



        private void button1_Click(object sender, EventArgs e)
        {

            if (dataGridView1.CurrentRow.Tag == null)
            {
                MessageBox.Show("Proszę wybrać bilet z listy.");
                return;
            }
            if (numericUpDown1.Value <= 0)
            {
                MessageBox.Show("Ilość biletów musi być większa niż 0!");
                return;
            }

            var biletId = dataGridView1.CurrentRow.Tag.ToString();
            int ilosc = (int)numericUpDown1.Value; // Pobiera ilość biletów

            Bilet bilet = null;
            switch (biletId)
            {
                case "1": // "1" to ID dla BiletNormalny
                    bilet = new BiletNormalny();
                    break;
                case "2": // i tak dalej dla pozostałych ID biletów
                    bilet = new BiletUlgowy();
                    break;
                case "3":
                    bilet = new BiletRodzinny();
                    break;
                case "4":
                    bilet = new BiletVIP();
                    break;
                default:
                    MessageBox.Show("Nieznany typ biletu");
                    return;

            }

            if (bilet != null)
            {
                bilet.GenerujPDF(ilosc);
                MessageBox.Show($"Plik PDF dla {ilosc}x '{bilet.Nazwa}' został wygenerowany.");
            }

            if (numericUpDown1.Value == null)
            {
                MessageBox.Show("Ilość biletów nie może wynosić 0!");
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

}