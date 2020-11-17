#define LAB_3

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
            GlobalGrid GH = new GlobalGrid(data);
            //grid1.DisplayElements();

            for (int u = 0; u<grid1.Elements.Length;u++)
            {
                // Creating global element
                LocalElement localElement = new LocalElement(grid1, grid1.Elements[u],data);

                for (int i = 0; i < Math.Pow(data.IntegrationSchemaWariant,2); i++)
                {
                    // Calculating Local H for every integration point
                    localElement.CalculateJacobian(i);
                    localElement.ReverseJacobian();
                    localElement.CalculateNxNy(i);
                    localElement.CalculateH(i,data.KFactor);

                    // Saving Local H
                    grid1.Elements[u].CalculateLocalH(u,grid1,localElement);
                }

                // Calculating Global H
                GH.CalculateGlobalH(u, grid1);
                //localElement.DisplayGausseQuadrature(u);
            }
            data.DisplayData();
            GH.DisplayGlobalH();
            Console.WriteLine("Execution Time: " + t1.Elapsed.TotalMilliseconds);
            GH.CalculateNonZeroElementPercentage();
            Console.ReadKey();
        }

        
    }
}
