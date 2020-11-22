using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM.Models
{
    public class GlobalMatrix
    {
        public double[,] GlobalH { get; }
        public double[,] GlobalC { get; }

        public GlobalMatrix(GlobalData data)
        {
            this.GlobalH = new double[data.NumberOfNodes, data.NumberOfNodes];
            this.GlobalC = new double[data.NumberOfNodes, data.NumberOfNodes];
        }

        public void CalculateGlobalH(int u, Grid grid1)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    this.GlobalH[grid1.Elements[u].ID[i] - 1, grid1.Elements[u].ID[j] - 1] += grid1.Elements[u].LocalH[i, j];
                }
            }
        }
        public void CalculateGlobalC(int u, Grid grid1)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    this.GlobalC[grid1.Elements[u].ID[i] - 1, grid1.Elements[u].ID[j] - 1] += grid1.Elements[u].LocalC[i, j];
                }
            }
        }

        public void DisplayGlobalH()
        {
            Console.WriteLine("Global H:");
            for (int j = 0; j < GlobalH.GetLength(0); j++)
            {
                Console.Write("[");
                for (int k = 0; k < GlobalH.GetLength(1); k++)
                {
                    string element = "{0,-10:F3}";
                    if (GlobalH[j,k]!=0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(string.Format(element, GlobalH[j, k]));
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    else
                    {
                        Console.Write(string.Format(element, GlobalH[j, k]));
                    }
                    
                }
                Console.WriteLine("]");
            }
        }

        public void DisplayGlobalC()
        {
            Console.WriteLine("Global C:");
            for (int j = 0; j < GlobalC.GetLength(0); j++)
            {
                Console.Write("[");
                for (int k = 0; k < GlobalC.GetLength(1); k++)
                {
                    string element = "{0,-10:F3}";
                    if (GlobalC[j, k] != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(string.Format(element, GlobalC[j, k]));
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    else
                    {
                        Console.Write(string.Format(element, GlobalC[j, k]));
                    }

                }
                Console.WriteLine("]");
            }
        }

        public void CalculateNonZeroElementPercentage()
        {
            double NON = 0;
            for(int i=0;i<GlobalH.GetLength(0);i++)
            {
                for(int j= 0; j< GlobalH.GetLength(1);j++)
                {
                    if(this.GlobalH[i,j] != 0)
                    { NON++; }  
                }
            }
            double percentage = NON / (GlobalH.GetLength(0) * GlobalH.GetLength(1));
            Console.WriteLine("Non Zero Elements in Global Matrix: {0:P}",percentage);
        }

        public void DisplayGlobalMatrixSchema()
        {
            double MaxValue = GetMaxVale();

            for (int i = 0; i < this.GlobalH.GetLength(0); i++)
            {
                Console.OutputEncoding = Encoding.Unicode;
                Console.Write("|");
                for (int j = 0; j < this.GlobalH.GetLength(1); j++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    if (Math.Abs(this.GlobalH[i, j]) <= (MaxValue * 0.51) && Math.Abs(this.GlobalH[i, j]) > (MaxValue * 0.26))
                        Console.ForegroundColor = ConsoleColor.DarkBlue;

                    if (Math.Abs(this.GlobalH[i, j]) <= (MaxValue * 0.26) && Math.Abs(this.GlobalH[i, j]) > (MaxValue * 0.14))
                        Console.ForegroundColor = ConsoleColor.DarkRed;

                    if (Math.Abs(this.GlobalH[i, j]) <= (MaxValue * 0.14) && Math.Abs(this.GlobalH[i, j]) > (MaxValue * 0.08))
                        Console.ForegroundColor = ConsoleColor.Blue;

                    if (Math.Abs(this.GlobalH[i, j]) <= (MaxValue * 0.08))
                        Console.ForegroundColor = ConsoleColor.DarkGray;

                    if (this.GlobalH[i, j] ==0)
                        Console.ForegroundColor = ConsoleColor.Black;
                    
                    Console.Write("██");
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("|");
            }

        }

        private double GetMaxVale()
        {
            double result = 0.0;
            for(int i =0; i< this.GlobalH.GetLength(0);i++)
            {
                for(int j = 0; j<this.GlobalH.GetLength(1);j++)
                {
                    if (this.GlobalH[i, j] > result)
                        result = GlobalH[i,j];
                }
            }
            return result;
        }

    }
}
