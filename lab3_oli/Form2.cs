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
    public partial class Form2: Form
    {
        Form1 form1;
        DataTable data;
        public Form2(Form1 form, DataTable dataTable)
        {
            InitializeComponent();
            initComboBox();
            this.form1 = form;
            data = dataTable;
        }

        private void initComboBox()
        {
            comboBox1.Items.Add("Muzyk");
            comboBox1.Items.Add("Bioinformatyk");
            comboBox1.Items.Add("Geolog");
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }


        //zatwierdz
        private void button1_Click(object sender, EventArgs e)
        {
            form1.Enabled = true;
            data.Rows.Add(textBox1.Text, textBox2.Text, int.Parse(textBox3.Text), comboBox1.Text);
            this.Close();


        }
        //anuluj
        private void button2_Click(object sender, EventArgs e)
        {
            form1.Enabled = true;
            this.Close();
        }

        private void form2_closed(object sender, FormClosedEventArgs e)
        {
            form1.Enabled = true;
        }




    }
}
