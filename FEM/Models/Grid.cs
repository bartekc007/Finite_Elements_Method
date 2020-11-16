using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FEM.Models
{
    public class Grid
    {
        public GlobalData Data { get; set; }
        public Element[] Elements { get; set; }
        public Node[] Nodes { get; set; }

        public Grid(GlobalData globaldata)
        {
            this.Data = globaldata;
            this.Elements = new Element[globaldata.NumberOfElements];
            this.Nodes = new Node[globaldata.NumberOfNodes];
        }
        public Grid() { }

        public void GenerateElements()
        {
            int temp = 0;

            for (int i = 0; i < this.Data.WidthPointNumber-1 ; i++)
            {
                for (int j = 1; j < this.Data.HeightPointNumber ; j++)
                {
                    Elements[temp] = new Element(
                        temp + 1, 
                        j + i* this.Data.HeightPointNumber,
                        j + i*this.Data.HeightPointNumber + this.Data.HeightPointNumber,
                        j + i * this.Data.HeightPointNumber + this.Data.HeightPointNumber + 1,
                        j + i * this.Data.HeightPointNumber +1);
                    temp++;
                }
                
            }
        }
        public void GenerateNodes()
        {
            double dH = this.Data.Height / (this.Data.HeightPointNumber - 1);
            double dW = this.Data.Width / (this.Data.WidthPointNumber - 1);
            int temp = 0;

            for(int i = 0; i < this.Data.WidthPointNumber; i++)
            {
                for(int j = 0; j < this.Data.HeightPointNumber; j++)
                {
                    this.Nodes[temp] = new Node(i * dW, j * dH);
                    temp++;
                }
            }
        }

        public void DisplayElements()
        {
            string oneElemnt = "Element ID: {0,-4} ID1: {1,-4} ID2: {2,-4} ID3: {3,-4} ID4: {4,-4}";
            foreach (var item in this.Elements)
                Console.WriteLine(string.Format(oneElemnt,item.ElementID,item.ID1,item.ID2,item.ID3,item.ID4));
            Console.WriteLine();
        }

        public void DisplayNodes()
        {
            string oneElemnt = "Node ID:{0,-4}  Node temperature: {1,-4} X: {2,-4:N4} Y: {3,-4:N4}";
            int i = 1;
            foreach (var item in this.Nodes)
                Console.WriteLine(string.Format(oneElemnt,i++, item.Temperature, item.X, item.Y));
            Console.WriteLine();
        }

        public void DisplayElementWithNodes(int elementId)
        {
            elementId -= 1;
            string oneElement = $@"Element ID: {Elements[elementId].ElementID,-4}
First Node: {Elements[elementId].ID1, -5}  Node temperature: {Nodes[Elements[elementId].ID1].Temperature,-4} X: {Nodes[Elements[elementId].ID1].X, -4:N4} Y: {Nodes[Elements[elementId].ID1].Y, -4:N4}
Second Node: {Elements[elementId].ID2,-4}  Node temperature: {Nodes[Elements[elementId].ID2].Temperature,-4} X: {Nodes[Elements[elementId].ID2].X,-4:N4} Y: {Nodes[Elements[elementId].ID2].Y,-4:N4}
Third Node: {Elements[elementId].ID3,-5}  Node temperature: {Nodes[Elements[elementId].ID3].Temperature,-4} X: {Nodes[Elements[elementId].ID3].X,-4:N4} Y: {Nodes[Elements[elementId].ID3].Y,-4:N4}
Fourth Node: {Elements[elementId].ID4,-4}  Node temperature: {Nodes[Elements[elementId].ID4].Temperature,-4} X: {Nodes[Elements[elementId].ID4].X,-4:N4} Y: {Nodes[Elements[elementId].ID4].Y,-4:N4}";
            Console.WriteLine(oneElement);
        }
    }
}
