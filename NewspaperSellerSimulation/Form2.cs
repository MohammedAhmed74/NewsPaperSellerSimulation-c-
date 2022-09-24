using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewspaperSellerModels;
using NewspaperSellerTesting;

namespace NewspaperSellerSimulation
{
    public partial class Form2 : Form
    {
        DataTable table = new DataTable();
        public Form2()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            table.Columns.Add("DayNo", Type.GetType("System.Int32"));
            table.Columns.Add("RandomNewsDayType", Type.GetType("System.Int32"));
            table.Columns.Add("NewsDayType", Type.GetType("System.String"));
            table.Columns.Add("RandomDemand", Type.GetType("System.Int32"));
            table.Columns.Add("Demand", Type.GetType("System.Int32"));
            table.Columns.Add("DailyCost", Type.GetType("System.Decimal"));
            table.Columns.Add("SalesProfit", Type.GetType("System.Decimal"));
            table.Columns.Add("LostProfit", Type.GetType("System.Decimal"));
            table.Columns.Add("ScrapProfit", Type.GetType("System.Decimal"));
            table.Columns.Add("DailyNetProfit", Type.GetType("System.Decimal"));
            dataGridView1.DataSource = table;

            for (int i = 0; i < Program.SimS.NumOfRecords; i++)
            {
                table.Rows.Add(Convert.ToString(Program.SimS.SimulationTable[i].DayNo), 
                    Convert.ToString(Program.SimS.SimulationTable[i].RandomNewsDayType), 
                    Convert.ToString(Program.SimS.SimulationTable[i].NewsDayType), 
                    Convert.ToString(Program.SimS.SimulationTable[i].RandomDemand), 
                    Convert.ToString(Program.SimS.SimulationTable[i].Demand), 
                    Convert.ToString(Program.SimS.SimulationTable[i].DailyCost), 
                    Convert.ToString(Program.SimS.SimulationTable[i].SalesProfit), 
                    Convert.ToString(Program.SimS.SimulationTable[i].LostProfit), 
                    Convert.ToString(Program.SimS.SimulationTable[i].ScrapProfit), 
                    Convert.ToString(Program.SimS.SimulationTable[i].DailyNetProfit));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
