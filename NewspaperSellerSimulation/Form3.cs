using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewspaperSellerSimulation
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label2.Text = Convert.ToString(Program.SimS.PerformanceMeasures.TotalSalesProfit);
            label3.Text = Convert.ToString(Program.SimS.PerformanceMeasures.TotalCost);
            label7.Text = Convert.ToString(Program.SimS.PerformanceMeasures.TotalLostProfit);
            label5.Text = Convert.ToString(Program.SimS.PerformanceMeasures.TotalScrapProfit);
            label13.Text = Convert.ToString(Program.SimS.PerformanceMeasures.TotalNetProfit);
            label11.Text = Convert.ToString(Program.SimS.PerformanceMeasures.DaysWithMoreDemand);
            label9.Text = Convert.ToString(Program.SimS.PerformanceMeasures.DaysWithUnsoldPapers);
        }
    }
}
