using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM.Models
{
    public static class SimulationSolver
    {
        public static double[] GaussElimination(double[,] H, double[] P, int size)
        {
            double[] x = new double[size];

            double[,] tmpA = new double[size, size + 1];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    tmpA[i, j] = H[i, j];
                }
                tmpA[i, size] = P[i];
            }

            double tmp = 0;

            for (int k = 0; k < size - 1; k++)
            {
                for (int i = k + 1; i < size; i++)
                {
                    tmp = tmpA[i, k] / tmpA[k, k];
                    for (int j = k; j < size + 1; j++)
                    {
                        tmpA[i, j] -= tmp * tmpA[k, j];
                    }
                }
            }

            for (int k = size - 1; k >= 0; k--)
            {
                tmp = 0;
                for (int j = k + 1; j < size; j++)
                {
                    tmp += tmpA[k, j] * x[j];
                }
                x[k] = (tmpA[k, size] - tmp) / tmpA[k, k];
            }

            return x;
        }

    }
}
