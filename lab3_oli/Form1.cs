using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3_oli
{
    public partial class Form1: Form
    {
        private BindingSource bindingSource1 = new BindingSource();

        public Form1()
        {
            InitializeComponent();
            initializeGrid();

        }

        private void initializeGrid()
        {
            var dataTable = new System.Data.DataTable();
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

      
    }
}
