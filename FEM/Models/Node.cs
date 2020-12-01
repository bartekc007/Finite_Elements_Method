using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM.Models
{
    public class Node
    {
        public double Temperature { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool EdgeCondition { get; }

        public Node(double x, double y, bool eCondition)
        { 
            this.X = x;
            this.Y = y;
            this.EdgeCondition = eCondition;
            this.Temperature = default;
        }
        public Node(double temperature, double x, double y, bool eCondition)
        {
            this.Temperature = temperature;
            this.X = x;
            this.Y = y;
            this.EdgeCondition = eCondition;
        }
    }
}
