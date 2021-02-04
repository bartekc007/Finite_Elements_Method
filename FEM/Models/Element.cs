using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM.Models
{
    public class Element
    {
        public int ElementID { get; set; }
        public int ID1 { get; }
        public int ID2 { get; }
        public int ID3 { get; }
        public int ID4 { get; }
        public int[] ID { get; }
        public double[,] LocalH { get; }
        public double[,] LocalC { get; }
        public double[,] LocalHbc { get; }
        public double [] LocalP { get; }

        public Element(int ElemtnID, int id1, int id2, int id3, int id4)
        {
            this.ElementID = ElemtnID;
            this.ID1 = id1;
            this.ID2 = id2;
            this.ID3 = id3;
            this.ID4 = id4;
            this.ID = new int[4] { id1, id2, id3, id4 };
            this.LocalH = new double[4, 4];
            this.LocalC = new double[4, 4];
            this.LocalHbc = new double[4, 4];
            this.LocalP = new double[4];
        }
        public Element() { }

        public void CalculateLocalH(int u, Grid grid1, LocalElement localElement)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    grid1.Elements[u].LocalH[j, k] += localElement.H[j, k];
                }
            }
        }

        public void CalculateLocalC(int u, Grid grid1, LocalElement localElement)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    grid1.Elements[u].LocalC[j, k] += localElement.C[j, k];
                }
            }
        }
        public void DisplayLocalHbc(int u)
        {
            Console.WriteLine("Hbc: " + (u + 1) );
            for (int j = 0; j < 4; j++)
            {
                Console.Write("[");
                for (int k = 0; k < 4; k++)
                {
                    string element = "{0,-10:F3}";
                    Console.Write(string.Format(element, LocalHbc[j, k]));
                }
                Console.WriteLine("]");
            }
            Console.WriteLine();
        }
        public void DisplayLocalH(int u)
        {
            Console.WriteLine("Hbc: " + (u + 1));
            for (int j = 0; j < 4; j++)
            {
                Console.Write("[");
                for (int k = 0; k < 4; k++)
                {
                    string element = "{0,-10:F3}";
                    Console.Write(string.Format(element, LocalH[j, k]));
                }
                Console.WriteLine("]");
            }
            Console.WriteLine();
        }

        public void DisplayeLocalC()
        {
            Console.WriteLine("Element C:");
            for (int j = 0; j < 4; j++)
            {
                Console.Write("[");
                for (int k = 0; k < 4; k++)
                {
                    string element = "{0,-10:F3}";
                    Console.Write(string.Format(element, LocalC[j,k]));
                }
                Console.WriteLine("]");
            }
        }
        public void DisplayLocalP(int u)
        {
            Console.WriteLine("Element: " + u);
            Console.Write("[");
            for (int i = 0; i < this.LocalP.Length; i++)
            {
                string element = "{0,-10:F3}";
                Console.Write(string.Format(element, LocalP[i]));
            }
            Console.WriteLine("]"); Console.WriteLine();

        }

        public void MergeHWithHbcAndP(double[,] hbc, double[] p)
        {
            for(int i=0; i< 4; i++)
            {
                for(int j=0; j< 4; j++)
                {
                    this.LocalHbc[i, j] = hbc[i, j];
                    this.LocalH[i,j] += hbc[i, j];
                }
            }
            for(int i=0; i<4; i++)
            {
                this.LocalP[i] += p[i];
            }
        }
    }
}

