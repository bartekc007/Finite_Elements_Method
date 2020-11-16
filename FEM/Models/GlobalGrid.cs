using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM.Models
{
    public class GlobalGrid
    {
        public double[,] GlobalElements { get; }
        public GlobalGrid(GlobalData data)
        {
            this.GlobalElements = new double[data.NumberOfNodes, data.NumberOfNodes];
        }

        public void CalculateGlobalH(int u, Grid grid1)
        {
            for(int i= 0; i< 4;i++)
            {
                for(int j=0;j<4;j++)
                {
                    this.GlobalElements[grid1.Elements[u].ID[i] - 1, grid1.Elements[u].ID[j] - 1] += grid1.Elements[u].LocalH[i, j];
                }
            }
        }

        public void DisplayGlobalH()
        {
            Console.WriteLine("Global H:");
            for (int j = 0; j < GlobalElements.GetLength(0); j++)
            {
                Console.Write("[");
                for (int k = 0; k < GlobalElements.GetLength(1); k++)
                {
                    string element = "{0,-10:F3}";
                    if (GlobalElements[j,k]!=0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(string.Format(element, GlobalElements[j, k]));
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    else
                    {
                        Console.Write(string.Format(element, GlobalElements[j, k]));
                    }
                    
                }
                Console.WriteLine("]");
            }
        }

        public void CalculateNonZeroElementPercentage()
        {
            double NON = 0;
            for(int i=0;i<GlobalElements.GetLength(0);i++)
            {
                for(int j= 0; j< GlobalElements.GetLength(1);j++)
                {
                    if(this.GlobalElements[i,j] != 0)
                    { NON++; }  
                }
            }
            double percentage = NON / (GlobalElements.GetLength(0) * GlobalElements.GetLength(1));
            Console.WriteLine("Non Zero Elements in Global Matrix: {0:P}",percentage);
        }

    }
}
