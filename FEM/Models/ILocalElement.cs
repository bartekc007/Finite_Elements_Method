using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM.Models
{
    
    public interface ILocalElement
    {
        void GenerateDerivativesForShapeFunctions();
        void CalculateJacobian(int j);
        void ReverseJacobian();
        void CalculateDeterminant();
        void CalculateNxNy(int pc);
        void CalculateH(int pc, int kFactor);
        void PrintDfSF();
        void DisplayLocalH(double[,] Hl, int u);
    }
}
