using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winFormsDataGrid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private VisualDataLoadClass _visualDataLoadClass;
        private CalculationClass _calculationClass;
        private void Form1_Load(object sender, EventArgs e)
        {
            var Values = Enumerable.Range(1, 5)
                .Select(c =>
                    Enumerable.Range(0, 2)
                        .Select(Convert.ToDecimal)
                        .ToList())
                .ToList();
            _visualDataLoadClass = new VisualDataLoadClass(dataGridView1, "data.txt");
            _visualDataLoadClass.SetNullCells();
            _calculationClass = new CalculationClass(_visualDataLoadClass);
            _calculationClass.Calculate();
        }
        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
