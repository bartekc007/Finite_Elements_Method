using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM.Models
{
    public class LocalElement3P : ILocalElement
    {
        /// <summary>
        /// Array[9] of KSI values at each integration point
        /// </summary>
        double[] Ksi = { -1 * Math.Sqrt(3.0 / 5.0) , 0 , Math.Sqrt(3.0 / 5.0) , -1 * Math.Sqrt(3.0 / 5.0) , 0 , Math.Sqrt(3.0 / 5.0) , -1 * Math.Sqrt(3.0 / 5.0) , 0 , Math.Sqrt(3.0 / 5.0) };
        /// <summary>
        /// Array[9] of ETA values at each integration point
        /// </summary>
        double[] Eta = { -1 * Math.Sqrt(3.0 / 5.0) , -1 * Math.Sqrt(3.0 / 5.0) , -1 * Math.Sqrt(3.0 / 5.0) , 0 , 0 , 0 , Math.Sqrt(3.0 / 5.0) , Math.Sqrt(3.0 / 5.0) ,  Math.Sqrt(3.0 / 5.0) };
        double[] IntegrationPointWeightKsi = { 5.0 / 9.0, 8.0 / 9.0, 5.0 / 9.0, 5.0 / 9.0, 8.0 / 9.0, 5.0 / 9.0 , 5.0 / 9.0, 8.0 / 9.0, 5.0 / 9.0 };
        double[] IntegrationPointWeightEta = { 5.0 / 9.0, 5.0 / 9.0, 5.0 / 9.0, 8.0 / 9.0, 8.0 / 9.0, 8.0 / 9.0 , 5.0 / 9.0, 5.0 / 9.0, 5.0 / 9.0 };
        /// <summary>
        /// Array[9] of X coordinates of element nodes
        /// </summary>
        double[] X;
        /// <summary>
        /// Array[9] of Y coordinates of element nodes
        /// </summary>
        double[] Y;
        /// <summary>
        /// [9x9] matrix containig [dX/dKSI,dY/dKSI][dX/dETA,dY/dETA]
        /// </summary>
        public double[,] Jacoby { get; private set; }
        /// <summary>
        /// Jacoby^-1 * 1/detJ
        /// </summary>
        public double[,] ReverseJacoby { get; private set; }
        /// <summary>
        /// Jacoby Determinant
        /// </summary>
        public double DetJacoby { get; private set; }

        /// <summary>
        /// Array[4] Containing ReverseJacoby[0,0] * dNi/dKSI + ReverseJacoby[0,1] * dNi/dETA
        /// of each integration point.
        /// </summary>
        public double[] Nx { get; private set; }
        /// <summary>
        /// Array[4] Containing ReverseJacoby[1,0] * dNi/dKSI + ReverseJacoby[1,1] * dNi/dETA
        /// of each integration point.
        /// </summary>
        public double[] Ny { get; private set; }

        /// <summary>
        /// Array[ i , j , k ] ,  
        /// [ i ]   0= KSI, 1=ETA,
        /// [ j ]   number of integration point,
        /// [ k ]   number of shape function,
        /// This array containing Derivatives of every shape function for each integration point in element
        /// </summary>
        double[,,] DerivativesOfShapeFunctions;
        /// <summary>
        /// Array[9,9] of NX[4]*NX[4,1] => {dN/dx} * {dN/dx}^T
        /// </summary>
        public double[,] FinalNx { get; private set; }
        /// <summary>
        /// Array[4,4] of NY[4]*NY[4,1] => {dN/dy} * {dN/dy}^T
        /// </summary>
        public double[,] FinalNy { get; private set; }

        /// <summary>
        /// [4x4] matrix containig values of FinalNx[i,j]+FinalNy[i,j] * alfa * detJ. 
        /// This localElement.H contains data for only one integration point.
        /// </summary>
        public double[,] H { get; private set; }

        public LocalElement3P(Grid grid, Element element)
        {
            this.DerivativesOfShapeFunctions = new double[2, 9, 4];
            this.X = new double[4];
            this.Y = new double[4];

            this.X[0] = grid.Nodes[grid.Elements[element.ElementID - 1].ID1 - 1].X;
            this.X[1] = grid.Nodes[grid.Elements[element.ElementID - 1].ID2 - 1].X;
            this.X[2] = grid.Nodes[grid.Elements[element.ElementID - 1].ID3 - 1].X;
            this.X[3] = grid.Nodes[grid.Elements[element.ElementID - 1].ID4 - 1].X;

            this.Y[0] = grid.Nodes[grid.Elements[element.ElementID - 1].ID1 - 1].Y;
            this.Y[1] = grid.Nodes[grid.Elements[element.ElementID - 1].ID2 - 1].Y;
            this.Y[2] = grid.Nodes[grid.Elements[element.ElementID - 1].ID3 - 1].Y;
            this.Y[3] = grid.Nodes[grid.Elements[element.ElementID - 1].ID4 - 1].Y;

            Jacoby = new double[2, 2];
            ReverseJacoby = new double[2, 2];

            Nx = new double[4];
            Ny = new double[4];

            FinalNx = new double[4, 4];
            FinalNy = new double[4, 4];

            H = new double[4, 4];
        }
        /// <summary>
        /// <para>Calculate value of each integration point shape function.</para>
        /// <para> Return void. Data are created inside an object. LocalElement.DerivativersOfShapeFunction[,,]</para>
        /// </summary>
        public void GenerateDerivativesForShapeFunctions()
        {
            for (int i = 0; i < 9; i++)
            {
                this.DerivativesOfShapeFunctions[0, i, 0] = -0.25 * (1 - this.Eta[i]) * this.IntegrationPointWeightKsi[i];
                this.DerivativesOfShapeFunctions[0, i, 1] = 0.25 * (1 - this.Eta[i]) * this.IntegrationPointWeightKsi[i];
                this.DerivativesOfShapeFunctions[0, i, 2] = 0.25 * (1 + this.Eta[i]) * this.IntegrationPointWeightKsi[i];
                this.DerivativesOfShapeFunctions[0, i, 3] = -0.25 * (1 + this.Eta[i]) * this.IntegrationPointWeightKsi[i];
            }
            for (int i = 0; i < 9; i++)
            {
                this.DerivativesOfShapeFunctions[1, i, 0] = -0.25 * (1 - this.Ksi[i]) * this.IntegrationPointWeightEta[i];
                this.DerivativesOfShapeFunctions[1, i, 1] = -0.25 * (1 + this.Ksi[i]) * this.IntegrationPointWeightEta[i];
                this.DerivativesOfShapeFunctions[1, i, 2] = 0.25 * (1 + this.Ksi[i]) * this.IntegrationPointWeightEta[i];
                this.DerivativesOfShapeFunctions[1, i, 3] = 0.25 * (1 - this.Ksi[i]) * this.IntegrationPointWeightEta[i];
            }
        }
        /// <summary>
        /// <para>Calculating values for actual integration point</para>
        /// <para>Return void. Data are created inside an object. LocalElement.Jacoby[,]</para>
        /// </summary>
        /// <param name="j">number of integration point</param>
        public void CalculateJacobian(int j)
        {
            GenerateDerivativesForShapeFunctions();

            this.Jacoby[0, 0] = DerivativesOfShapeFunctions[0, j, 0] * this.X[0]
                + DerivativesOfShapeFunctions[0, j, 1] * this.X[1]
                + DerivativesOfShapeFunctions[0, j, 2] * this.X[2]
                + DerivativesOfShapeFunctions[0, j, 3] * this.X[3];
            this.Jacoby[0, 1] = DerivativesOfShapeFunctions[1, j, 0] * this.X[0]
                + DerivativesOfShapeFunctions[1, j, 1] * this.X[1]
                + DerivativesOfShapeFunctions[1, j, 2] * this.X[2]
                + DerivativesOfShapeFunctions[1, j, 3] * this.X[3];
            this.Jacoby[1, 0] = DerivativesOfShapeFunctions[0, j, 0] * this.Y[0]
                + DerivativesOfShapeFunctions[0, j, 1] * this.Y[1]
                + DerivativesOfShapeFunctions[0, j, 2] * this.Y[2]
                + DerivativesOfShapeFunctions[0, j, 3] * this.Y[3];
            this.Jacoby[1, 1] = DerivativesOfShapeFunctions[1, j, 0] * this.Y[0]
                + DerivativesOfShapeFunctions[1, j, 1] * this.Y[1]
                + DerivativesOfShapeFunctions[1, j, 2] * this.Y[2]
                + DerivativesOfShapeFunctions[1, j, 3] * this.Y[3];
        }
        /// <summary>
        /// <para>Calculating values using inside data from LocalElement.Jacoby </para>
        /// <para>Return void. Data are created inside an object. LocalElement.ReverseJacoby[,]</para>
        /// </summary>
        public void ReverseJacobian()
        {
            CalculateDeterminant();

            this.ReverseJacoby[0, 0] = this.Jacoby[1, 1] / this.DetJacoby;
            this.ReverseJacoby[0, 1] = -1 * this.Jacoby[0, 1] / this.DetJacoby;
            this.ReverseJacoby[1, 0] = -1 * this.Jacoby[1, 0] / this.DetJacoby;
            this.ReverseJacoby[1, 1] = this.Jacoby[0, 0] / this.DetJacoby;
        }
        /// <summary>
        /// <para>Calculating values using inside data from LocalElement.Jacoby </para>
        /// <para>Return void. Data are created inside an object. LocalElement.DetJ</para>
        /// </summary>
        public void CalculateDeterminant()
        {
            this.DetJacoby = (this.Jacoby[0, 0] * this.Jacoby[1, 1]) - (this.Jacoby[0, 1] * this.Jacoby[1, 0]);
        }
        /// <summary>
        /// Write all derivatives of shape functions to standard output
        /// </summary>
        public void PrintDfSF()
        {
            for (int n = 0; n < 2; n++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Console.Write(this.DerivativesOfShapeFunctions[n, j, i] + "  ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
        /// <summary>
        /// <para>Calculating values using inside data from LocalElement.ReverseJacoby and LocalElement.DerivativersOfShapeFunctions </para>
        /// <para>Return void. Data are created inside an object. LocalElement.NX and LocalElement.NY </para>
        /// </summary>
        /// <param name="pc">number of integration point</param>
        public void CalculateNxNy(int pc)
        {
            for (int i = 0; i < 4; i++)
            {
                this.Nx[i] = (this.ReverseJacoby[0, 0] * this.DerivativesOfShapeFunctions[0, pc, i]) + (this.ReverseJacoby[0, 1] * this.DerivativesOfShapeFunctions[1, pc, i]);
                this.Ny[i] = (this.ReverseJacoby[1, 0] * this.DerivativesOfShapeFunctions[0, pc, i]) + (this.ReverseJacoby[1, 1] * this.DerivativesOfShapeFunctions[1, pc, i]);
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    this.FinalNx[i, j] = this.Nx[i] * this.Nx[j];
                    this.FinalNy[i, j] = this.Ny[i] * this.Ny[j];
                }
            }
        }
        /// <summary>
        /// <para>Calculating H for only one integration point</para>
        /// <para>Return void. Data are created inside an object LocalElement.H </para>
        /// </summary>
        public void CalculateH()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    this.H[i, j] = (this.FinalNx[i, j] + this.FinalNy[i, j]) * 25 * this.DetJacoby;
                }
            }
        }
        /// <summary>
        /// Write Local H to standard output
        /// </summary>
        /// <param name="Hl">Local H for one of elements from Grid</param>
        /// <param name="u">index of grid element</param>
        public void DisplayLocalH(double[,] Hl, int u)
        {
            Console.WriteLine("Element:" + (u + 1));
            for (int j = 0; j < 4; j++)
            {
                Console.Write("[");
                for (int k = 0; k < 4; k++)
                {
                    Console.Write(Hl[j, k] + " , ");
                }
                Console.WriteLine("]");
            }
        }
    }
}
