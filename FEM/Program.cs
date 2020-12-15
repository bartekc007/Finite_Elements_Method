using FEM.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM
{
    class Program
    {
        static void Main(string[] args)
        {
            // Generating Grid from the data
            GlobalData data = new GlobalData();

            Grid grid1 = new Grid(data);
            grid1.GenerateElements();
            grid1.GenerateNodes(data);

            // Creating empty Global Structure
            Stopwatch t1 = Stopwatch.StartNew();
            GlobalMatrix GloalStructure = new GlobalMatrix(data);
            //grid1.DisplayElements();
            for (int j = 0; j < data.Time / data.dTime; j++)
            {
                GloalStructure.ClearTables();
                for (int u = 0; u<grid1.Elements.Length;u++)
                {
                    // Creating global element
                    LocalElement localElement = new LocalElement(grid1, grid1.Elements[u],data);
                    
                    // Calculating Local H for every integration point
                    for (int i = 0; i < Math.Pow(data.IntegrationSchemaWariant,2); i++)
                    {
                        localElement.CalculateJacobian(i);
                        localElement.ReverseJacobian();
                        localElement.CalculateNxNy(i);
                        localElement.CalculateH(i, data.KFactor);
                        localElement.CalculateC(i, data.SpecificHeat, data.Density);

                        // Saving Local H
                        grid1.Elements[u].CalculateLocalH(u,grid1,localElement);
                        grid1.Elements[u].CalculateLocalC(u, grid1, localElement);
                    }
                    // Calculation H Boudary Condition
                    localElement.CalculateHcb(data.alfa, grid1, u,data.AmbientTemperature,data.IntegrationSchemaWariant);
                    grid1.Elements[u].MergeHWithHbcAndP(localElement.Hbc,localElement.P);

                
                    // Calculating Global H
                    GloalStructure.CalculateGlobalH(u, grid1);
                    GloalStructure.CalculateGlobalC(u, grid1);
                    GloalStructure.CalculateGlobalP(u, grid1);
                }

                GloalStructure.MergeGlobalHGlobalC(data,grid1);

                //GH.DisplayGlobalH();
                //GH.DisplayGlobalP();

                double[] T0 = SimulationSolver.GaussElimination(GloalStructure.GlobalH, GloalStructure.GlobalP, data.NumberOfNodes);

                for (int i = 0; i < grid1.Nodes.Length; i++)
                {
                    grid1.Nodes[i].Temperature = T0[i];
                }
                string temperatureStringFormat = "iteration: {0,-4} T0 min: {1,-15:F6} T0 max: {2,-15:F6}";
                Console.WriteLine(string.Format(temperatureStringFormat,j + 1, T0.Min(), T0.Max()));                
            }
            
            Console.WriteLine("Execution Time: " + (t1.Elapsed.TotalMilliseconds/1000) + " seconds");
                GloalStructure.CalculateNonZeroElementPercentage();
            Console.ReadKey();
        }
        
        
    }
}
