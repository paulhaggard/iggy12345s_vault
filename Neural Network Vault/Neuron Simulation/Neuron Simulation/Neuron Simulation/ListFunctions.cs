using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron_Simulation
{
    static class ListFunctions
    {
        public static List<List<T>> Transpose<T>(List<List<T>> data)
        {
            // Transposes a 2 dimensional array of lists

            List<List<T>> temp = new List<List<T>>(data[0].Count);

            for (int i = 0; i < data.Count; i++)
                for (int j = 0; j < data[0].Count; j++)
                    temp[j][i] = data[i][j];

            return temp;
        }

        public static List<List<double>> DotProduct(List<List<double>> m1, List<List<double>> m2)
        {
            // Performs a dot product on 2, 2 dimensional arrays of lists
            if (m1.Count != m2[0].Count)
                throw new InvalidOperationException("DotProduct",new Exception("Cannot multiply two matrices with different sizes!"));

            List<List<double>> temp = new List<List<double>>(m1.Count);

            for(int i = 0; i < m1.Count; i++)
            {
                temp.Add(new List<double>(m2[0].Count));

                for(int j = 0; j < m2[0].Count; j++)
                {
                    double total = 0;
                    for (int k = 0; k < m1[i].Count; k++)
                        total += m1[i][k] * m2[j][k];
                    temp[i].Add(total);
                }
            }

            return temp;
        }
    }
}
