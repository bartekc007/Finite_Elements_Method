﻿using System;
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
        public readonly double Height;
        public readonly double Width;
        public readonly int HeightPointNumber;
        public readonly int WidthPointNumber;
        public readonly int NumberOfElements;
        public readonly int NumberOfNodes;
        public readonly int IntegrationSchemaWariant;
        public readonly int KFactor;
        public readonly double SpecificHeat;
        public readonly double Density;
        public readonly double InitialTemperature;
        public readonly double AmbientTemperature;
        public readonly double alfa;
        public readonly double Time;
        public readonly double dTime;
       

        public GlobalData()
        {
            string[] lines = System.IO.File.ReadAllLines("C:\\Users\\barte\\source\\repos\\FEM\\FEM\\Data\\GlobalData.txt");

            string[] oneLine = lines[0].Split(' ');
            this.Height = Double.Parse(oneLine[0]);

            oneLine = lines[1].Split(' ');
            this.Width = Double.Parse(oneLine[0]);

            oneLine = lines[2].Split(' ');
            this.HeightPointNumber = Int32.Parse(oneLine[0]);

            oneLine = lines[3].Split(' ');
            this.WidthPointNumber = Int32.Parse(oneLine[0]);

            this.NumberOfElements = (HeightPointNumber - 1) * (WidthPointNumber - 1);

            this.NumberOfNodes = HeightPointNumber * WidthPointNumber;

            oneLine = lines[4].Split(' ');
            if (Int32.Parse(oneLine[0]) == 2 || Int32.Parse(oneLine[0]) == 3 || Int32.Parse(oneLine[0]) == 4)
                this.IntegrationSchemaWariant = Int32.Parse(oneLine[0]);
            else
                throw new ArgumentException("Integration schema wariat has to be set as value 2,3 or 4");
            

            oneLine = lines[5].Split(' ');
            this.KFactor = Int32.Parse(oneLine[0]);

            oneLine = lines[6].Split(' ');
            this.SpecificHeat = double.Parse(oneLine[0]);

            oneLine = lines[7].Split(' ');
            this.Density = double.Parse(oneLine[0]);

            oneLine = lines[8].Split(' ');
            this.InitialTemperature = double.Parse(oneLine[0]);

            oneLine = lines[9].Split(' ');
            this.AmbientTemperature = double.Parse(oneLine[0]);

            oneLine = lines[10].Split(' ');
            this.alfa = double.Parse(oneLine[0]);

            oneLine = lines[11].Split(' ');
            this.Time = double.Parse(oneLine[0]);

            oneLine = lines[12].Split(' ');
            this.dTime = double.Parse(oneLine[0]);
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
            Console.WriteLine(string.Format(oneLineOfData, "Integration schema wariant:", this.IntegrationSchemaWariant));
            Console.WriteLine(string.Format(oneLineOfData, "k Factor value:", this.KFactor));
            Console.WriteLine(string.Format(oneLineOfData, "Specific Heat:", this.SpecificHeat));
            Console.WriteLine(string.Format(oneLineOfData, "Density:", this.Density));
            Console.WriteLine(string.Format(oneLineOfData, "Initial Temperature:", this.InitialTemperature));
            Console.WriteLine(string.Format(oneLineOfData, "alfa: ", this.alfa));
            Console.WriteLine(string.Format(oneLineOfData, "Time: ", this.Time));
            Console.WriteLine(string.Format(oneLineOfData, "dTime: ", this.dTime));
            Console.WriteLine();
        }
    }
}
