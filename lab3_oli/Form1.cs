using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3_oli
{
    public partial class Form1: Form
    {
        Form2 form2;
        private BindingSource bindingSource1 = new BindingSource();
        private DataTable dataTable = new System.Data.DataTable();

        public bool opened;
        public Form1()
        {
            InitializeComponent();
            initializeGrid();

        }

        private void initializeGrid()
        {
            dataTable.Columns.Add("Imie", typeof(string));
            dataTable.Columns.Add("Nazwisko", typeof(string));
            dataTable.Columns.Add("Wiek", typeof(int));
            dataTable.Columns.Add("Stanowisko", typeof(string));

            dataTable.Rows.Add("Olimpia", "Gawrońska", 21, "Bioinformatyk");
            dataTable.Rows.Add("Dorota", "Durzynska" , 22 , "Muzyk");

            // Połącz dane z BindingSource
            bindingSource1.DataSource = dataTable;
            // Inicjalizacja DataGridView
          
            dataGridView1.Dock = DockStyle.Fill;
            // Przypisz DataGridView do BindingSource
            dataGridView1.DataSource = bindingSource1;
            // Dodaj DataGridView do formularza
            Controls.Add(dataGridView1);

        }

        public void addToGrid()
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //dodaj
        private void button1_Click(object sender, EventArgs e)
        {
            form2 = new Form2(this, dataTable);
            form2.Show();
            opened = true;
            this.Enabled = false;

        }

        //zapis

        private void button3_Click(object sender, EventArgs e)
        {
            ExportToCSV(dataGridView1);
        }


        private void ExportToCSV(DataGridView dataGridView)
        {
            string filePath = Path.Combine(Application.StartupPath, "FILENAME.txt");
            string csvContent = "";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
               
                if (!row.IsNewRow)
                {
                    csvContent += string.Join(",", row.Cells.Cast<DataGridViewCell>().Select(c => c.Value?.ToString() ?? "")) + Environment.NewLine;
                }
            }

     
            File.WriteAllText(filePath, csvContent);
            MessageBox.Show("Plik zapisany w: " + filePath, "Zapisano", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void ImportFromCSV(DataGridView dataGridView)
        {
            string filePath = Path.Combine(Application.StartupPath, "FILENAME.txt");

    
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Plik CSV nie istnieje.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] lines = File.ReadAllLines(filePath);

            dataTable.Clear();

            foreach (string line in lines)
            {
                string[] values = line.Split(',');

                if (values.Length == dataTable.Columns.Count)
                {
                    dataTable.Rows.Add(values);
                }
                else
                {
                    MessageBox.Show("Nieprawidłowy format pliku CSV.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            ImportFromCSV(dataGridView1);
        }


        //usun
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) 
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
            }
            else
            {
                MessageBox.Show("Wybierz wiersz do usunięcia.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
