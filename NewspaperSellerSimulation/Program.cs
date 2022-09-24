using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewspaperSellerTesting;
using NewspaperSellerModels;


namespace NewspaperSellerSimulation
{
    static class Program
    {
        
        static public SimulationSystem SimS = new SimulationSystem();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string[] lines = File.ReadAllLines(@"D:\CS_4th\First\NewspaperSellerSimulation_Students\NewspaperSellerSimulation\TestCases\TestCase3.txt");
            string[] dayTypeCC = new string[3];
            List<DemandDistribution> DD = new List<DemandDistribution>();
            
            Enum enum1;
            Random rnd = new Random();
            DayTypeDistribution dayTypeD = new DayTypeDistribution();
            DemandDistribution dd = new DemandDistribution();

            DayTypeDistribution DTDrow = new DayTypeDistribution();
            List<DayTypeDistribution> DTDlist = new List<DayTypeDistribution>();
            DemandDistribution DDrow = new DemandDistribution();
            int[] saveMax = new int[3];



            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "NumOfNewspapers")
                {

                    SimS.NumOfNewspapers = Convert.ToInt32(lines[i + 1]);
                }
                else if (lines[i] == "NumOfRecords")
                {
                    SimS.NumOfRecords = Convert.ToInt32(lines[i + 1]);
                }
                else if (lines[i] == "PurchasePrice")
                {

                    SimS.PurchasePrice = Convert.ToDecimal(lines[i + 1]);

                }
                else if (lines[i] == "ScrapPrice")
                {
                    SimS.ScrapPrice = Convert.ToDecimal(lines[i + 1]);

                }
                else if (lines[i] == "SellingPrice")
                {
                    
                    SimS.SellingPrice = Convert.ToDecimal(lines[i + 1]);

                }
                else if (lines[i] == "DayTypeDistributions")
                {

                    dayTypeCC = lines[i + 1].Split(',', (char)StringSplitOptions.RemoveEmptyEntries);
                   
                }
                else if (lines[i] == "DemandDistributions")
                {

                    for (int j = 0; j < 7; j++)
                    {
                        string[] DemandCC = lines[i + j + 1].Split(',', (char)StringSplitOptions.RemoveEmptyEntries);

                        DDrow = new DemandDistribution();
                        DTDrow = new DayTypeDistribution();
                        DTDlist = new List<DayTypeDistribution>();
                        DDrow.Demand = Convert.ToInt32(DemandCC[0]);

                        for(int m =1; m <4; m++)
                        {
                            DTDrow = new DayTypeDistribution();
                            DTDrow.Probability = Convert.ToDecimal(DemandCC[m]);
                            if (j == 0)
                            {
                                DTDrow.MinRange = 1;
                                DTDrow.MaxRange = Convert.ToInt32(DTDrow.Probability * 100);
                                saveMax[m - 1] = DTDrow.MaxRange;
                            }
                            else
                            {
                                DTDrow.MinRange = saveMax[m-1]+1;
                                DTDrow.MaxRange = Convert.ToInt32(DTDrow.Probability * 100) + saveMax[m - 1];
                                saveMax[m - 1] = DTDrow.MaxRange;
                            }
                            DTDlist.Add(DTDrow);
                            
                        }
                        //  row1 in list of DayTypeD
                        DDrow.DayTypeDistributions = DTDlist;
                        // row0 in list of DemandD
                        SimS.DemandDistributions.Add(DDrow);

                    }

                }


            }






            for (int j = 0; j < 3 ; j++) //          DayTypeDistribution completing
            {

                DayTypeDistribution DTD = new DayTypeDistribution();
                DTD.Probability = Convert.ToDecimal(dayTypeCC[j]);
                if (j == 0)
                {
                    DTD.CummProbability = DTD.Probability;
                    DTD.MinRange = 1;
                    DTD.MaxRange = Convert.ToInt32(DTD.CummProbability * 100);
                }
                else
                {
                    DTD.CummProbability = DTD.Probability + SimS.DayTypeDistributions[j - 1].CummProbability;
                    DTD.MinRange = SimS.DayTypeDistributions[j - 1].MaxRange + 1;
                    DTD.MaxRange = Convert.ToInt32(DTD.CummProbability * 100);

                }
                SimS.DayTypeDistributions.Add(DTD);// DTD (Row) -> DayTypeDistributions (List)
            }


            int DayTypeIndex = 0;
            for (int i = 0; i < SimS.NumOfRecords; i++) //              Time Per Day
            {
                SimulationCase Sc = new SimulationCase();
                Sc.DayNo = i + 1;
                Sc.RandomNewsDayType = rnd.Next(0, 99) + 1;
                for (int j = 0; j < 3; j++)
                {
                    if (Sc.RandomNewsDayType >= SimS.DayTypeDistributions[j].MinRange && Sc.RandomNewsDayType <= SimS.DayTypeDistributions[j].MaxRange)
                    {
                        if (j == 0)
                        {
                            DayTypeIndex = 1;
                            Sc.NewsDayType = Enums.DayType.Good;
                            break;
                        }
                        else if (j == 1)
                        {
                            DayTypeIndex = 2;
                            Sc.NewsDayType = Enums.DayType.Fair;
                            break;
                        }
                        else
                        {
                            DayTypeIndex = 3;
                            Sc.NewsDayType = Enums.DayType.Poor;
                            break;
                        }
                    }
                }
                Sc.RandomDemand = rnd.Next(0, 99) + 1;

                for (int k = 0; k < 7; k++)
                {
                    if (Sc.RandomDemand >= SimS.DemandDistributions[k].DayTypeDistributions[DayTypeIndex - 1].MinRange)
                    {
                        if (Sc.RandomDemand <= SimS.DemandDistributions[k].DayTypeDistributions[DayTypeIndex - 1].MaxRange)
                        {
                            Sc.Demand = SimS.DemandDistributions[k].Demand;
                            break;
                        }
                    }
                }
                //                     DailyCost
                Sc.DailyCost = SimS.PurchasePrice * SimS.NumOfNewspapers;
                //                     SalesProfit
                if (Sc.Demand < SimS.NumOfNewspapers || Sc.Demand == SimS.NumOfNewspapers)
                    Sc.SalesProfit = SimS.SellingPrice * Sc.Demand;
                else
                    Sc.SalesProfit = SimS.SellingPrice * SimS.NumOfNewspapers;

                //                     LostProfit
                //                     ScrapProfit
                if (Sc.Demand > SimS.NumOfNewspapers)
                {
                    Sc.ScrapProfit = 0;
                    Sc.LostProfit = (Sc.Demand - SimS.NumOfNewspapers) * (SimS.SellingPrice - SimS.PurchasePrice);
                }
                else if (Sc.Demand < SimS.NumOfNewspapers)
                {
                    Sc.LostProfit = 0;
                    Sc.ScrapProfit = (SimS.NumOfNewspapers - Sc.Demand) * SimS.ScrapPrice;
                }
                else
                {
                    Sc.LostProfit = 0;
                    Sc.ScrapProfit = 0;
                }
                //                     DailyNetProfit
                Sc.DailyNetProfit = -(SimS.NumOfNewspapers * SimS.PurchasePrice) + Sc.SalesProfit + Sc.ScrapProfit - Sc.LostProfit;
                SimS.SimulationTable.Add(Sc);
            }
            SimS.PerformanceMeasures.TotalNetProfit = 0;
            SimS.PerformanceMeasures.TotalCost = 0;
            SimS.PerformanceMeasures.TotalLostProfit = 0;
            SimS.PerformanceMeasures.TotalSalesProfit = 0;
            SimS.PerformanceMeasures.TotalScrapProfit = 0;
            SimS.PerformanceMeasures.DaysWithMoreDemand = 0;
            SimS.PerformanceMeasures.DaysWithUnsoldPapers = 0;

            for (int m = 0; m < SimS.NumOfRecords; m++)
            {
                SimS.PerformanceMeasures.TotalNetProfit += SimS.SimulationTable[m].DailyNetProfit;
                SimS.PerformanceMeasures.TotalCost += SimS.SimulationTable[m].DailyCost;
                SimS.PerformanceMeasures.TotalLostProfit += SimS.SimulationTable[m].LostProfit;
                SimS.PerformanceMeasures.TotalSalesProfit += SimS.SimulationTable[m].SalesProfit;
                SimS.PerformanceMeasures.TotalScrapProfit += SimS.SimulationTable[m].ScrapProfit;
                if (SimS.SimulationTable[m].Demand > SimS.NumOfNewspapers)
                    SimS.PerformanceMeasures.DaysWithMoreDemand++;
                if (SimS.SimulationTable[m].ScrapProfit != 0)
                    SimS.PerformanceMeasures.DaysWithUnsoldPapers++;
            }

            String testingResult = TestingManager.Test(SimS, Constants.FileNames.TestCase3);
            MessageBox.Show(testingResult);
            Application.Run(new Form2());
            Application.Run(new Form3());
        }
    }
}
