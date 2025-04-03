using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using static lab3_oli.Form1;


namespace lab3_oli
{
    public partial class Form1 : Form
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
            dataTable.Rows.Add("Dorota", "Durzynska", 22, "Muzyk");

            // Połącz dane z BindingSource
            bindingSource1.DataSource = dataTable;
            // Inicjalizacja DataGridView

            dataGridView1.Dock = DockStyle.Fill;
            // Przypisz DataGridView do BindingSource
            dataGridView1.DataSource = bindingSource1;
            // Dodaj DataGridView do formularza
            Controls.Add(dataGridView1);

        }
        [Serializable]
        public class Employee
        {
            public string Imie { get; set; }
            public string Nazwisko { get; set; }
            public int Wiek { get; set; }
            public string Stanowisko { get; set; }
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

        private void saveJSON_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Application.StartupPath, "dataExport.json");
            List<Employee> employees = new List<Employee>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    Employee emp = new Employee
                    {
                        Imie = row.Cells["Imie"].Value?.ToString() ?? "",
                        Nazwisko = row.Cells["Nazwisko"].Value?.ToString() ?? "",
                        Wiek = Convert.ToInt32(row.Cells["Wiek"].Value ?? 0),
                        Stanowisko = row.Cells["Stanowisko"].Value?.ToString() ?? ""
                    };
                    employees.Add(emp);
                }
                var options1 = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping

                };
                string jsonString = JsonSerializer.Serialize(employees);
                File.WriteAllText(filePath, jsonString);
                Console.WriteLine(File.ReadAllText(filePath));

            }



        }

        private void readJSON_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Application.StartupPath, "dataExport.json");
            string jsonString = File.ReadAllText(filePath);

            List<Employee> employees = JsonSerializer.Deserialize<List<Employee>>(jsonString);



            foreach (var emp in employees)
            {
                dataTable.Rows.Add(emp.Imie, emp.Nazwisko, emp.Wiek, emp.Stanowisko);
            }
        }

        private void readXML_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Application.StartupPath, "dataExportXML.xml");
            List<Employee> employees;
            XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>));
            using (TextReader reader = new StreamReader(filePath))
            {
                employees = (List<Employee>)serializer.Deserialize(reader);
                Console.WriteLine("Obiekt został odczytany z pliku XML.");
              
            }
            foreach (var emp in employees)
            {
                dataTable.Rows.Add(emp.Imie, emp.Nazwisko, emp.Wiek, emp.Stanowisko);
            }

        }

        private void saveXML_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Application.StartupPath, "dataExportXML.xml");
            List<Employee> employees = new List<Employee>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    Employee emp = new Employee
                    {
                        Imie = row.Cells["Imie"].Value?.ToString() ?? "",
                        Nazwisko = row.Cells["Nazwisko"].Value?.ToString() ?? "",
                        Wiek = Convert.ToInt32(row.Cells["Wiek"].Value ?? 0),
                        Stanowisko = row.Cells["Stanowisko"].Value?.ToString() ?? ""
                    };

                    employees.Add(emp);
                }


                XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>));
                using (TextWriter writer = new StreamWriter(filePath))
                {

                    serializer.Serialize(writer, employees);

                }



                Console.WriteLine("Obiekt został zserializowany do pliku XML.");

            }
        }
    }
}
