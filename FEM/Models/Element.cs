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
        public double[,] LocalH {get;}

        public Element(int ElemtnID, int id1, int id2, int id3, int id4)
        {
            this.ElementID = ElemtnID;
            this.ID1 = id1;
            this.ID2 = id2;
            this.ID3 = id3;
            this.ID4 = id4;
            this.ID = new int[4] { id1, id2, id3, id4 };
            this.LocalH = new double[4, 4];
        }
        public Element() { }

        public void CalculateLocalH(int u, Grid grid1, LocalElement3P localElement)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    grid1.Elements[u].LocalH[j, k] += localElement.H[j,k];
                }
            }
        }
        public void CalculateLocalH(int u, Grid grid1, LocalElement2P localElement)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    grid1.Elements[u].LocalH[j, k] += localElement.H[j, k];
                }
            }
        }
    }
}

