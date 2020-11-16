using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace FEM.Models
{
    public class GlobalData
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public int HeightPointNumber { get; set; }
        public int WidthPointNumber { get; set; }
        public readonly int NumberOfElements;
        public readonly int NumberOfNodes;
       

        public GlobalData()
        {
            string[] lines = System.IO.File.ReadAllLines("C:\\Users\\barte\\source\\repos\\FEM\\FEM\\Data\\GlobalData.txt");

            string[] oneLine = lines[0].Split(' ');
            this.Height = Double.Parse(oneLine[1]);

            oneLine = lines[1].Split(' ');
            this.Width = Double.Parse(oneLine[1]);

            oneLine = lines[2].Split(' ');
            this.HeightPointNumber = Int32.Parse(oneLine[1]);

            oneLine = lines[3].Split(' ');
            this.WidthPointNumber = Int32.Parse(oneLine[1]);

            this.NumberOfElements = (HeightPointNumber - 1) * (WidthPointNumber - 1);

            this.NumberOfNodes = HeightPointNumber * WidthPointNumber;
        }

        public void DisplayData()
        {
            string oneLineOfData = "{0,-30} {1,10}";
            Console.WriteLine(string.Format(oneLineOfData, "Height:", this.Height));
            Console.WriteLine(string.Format(oneLineOfData, "Width:", this.Width));
            Console.WriteLine(string.Format(oneLineOfData, "Number of height points:", this.HeightPointNumber));
            Console.WriteLine(string.Format(oneLineOfData, "Number of width points:", this.WidthPointNumber));
            Console.WriteLine(string.Format(oneLineOfData, "Number of Elements:", this.NumberOfElements));
            Console.WriteLine(string.Format(oneLineOfData, "Number of Nodes:", this.NumberOfNodes));
            Console.WriteLine();
        }
    }
}
