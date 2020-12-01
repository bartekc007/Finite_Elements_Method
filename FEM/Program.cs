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
            grid1.GenerateNodes();

            // Creating empty Global Structure
            Stopwatch t1 = Stopwatch.StartNew();
            GlobalMatrix GH = new GlobalMatrix(data);
            //grid1.DisplayElements();

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
                localElement.CalculateHcb(data.KFactor, grid1, u);
                grid1.Elements[u].MergeHWithHbc(localElement.Hbc);


                // Calculating Global H
                GH.CalculateGlobalH(u, grid1);
                GH.CalculateGlobalC(u, grid1);
                
            }
            data.DisplayData();
            GH.DisplayGlobalH();
            Console.WriteLine();

            GH.DisplayGlobalC();
            Console.WriteLine();

            //GH.DisplayGlobalMatrixSchema();
            Console.WriteLine();

            Console.WriteLine("Execution Time: " + t1.Elapsed.TotalMilliseconds);
            GH.CalculateNonZeroElementPercentage();

            Console.ReadKey();
        }
        
        
    }
}
