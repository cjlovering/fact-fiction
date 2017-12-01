using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactOrFictionTextHandling.InferSentClient
{
    public class DistanceCalculator
    {
        public static double CalculateCosineSimilarity(Double[] vector1, Double[] vector2)
        {
             return CalculateDotProduct(vector1, vector2) / (CalculateNorm(vector1) * CalculateNorm(vector2));
        }

        private static double CalculateDotProduct(Double[] vector1, Double[] vector2)
        {
            double dotProduct = 0;
            int size = vector1.Length;
            for (int i = 0; i < size; i++)
            {
                dotProduct += vector1[i] * vector2[i];
            }
            return dotProduct;
        }

        private static double CalculateNorm(Double[] vector)
        {
            double squaredSum = 0;
            foreach (double num in vector)
            {
                squaredSum += Math.Pow(num, 2);
            }
            return Math.Sqrt(squaredSum);
        }
    }
}
