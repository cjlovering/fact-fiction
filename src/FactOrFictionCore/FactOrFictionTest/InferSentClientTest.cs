using FactOrFictionCommon.Models;
using FactOrFictionTextHandling.InferSentClient;
using System;
using Xunit;

namespace FactOrFictionTest
{
    public class InferSentClientTest
    {
        private readonly double TOLERANCE = Math.Pow(10, -7);

        [Fact]
        public void ConvertVectorStringToDouleArray()
        {
            Sentence sentence = new Sentence();
            sentence.InferSentVectorsString = "1;2;3";
            double[] vectorDouble = new double[3];
            vectorDouble[0] = 1.0;
            vectorDouble[1] = 2.0;
            vectorDouble[2] = 3.0;
            Assert.Equal(vectorDouble, sentence.InferSentVectorsDouble);
        }

        [Fact]
        public void ConvertDoubleArrayToString()
        {
            Sentence sentence = new Sentence();
            double[] vectorDouble = new double[3];
            vectorDouble[0] = 1.0;
            vectorDouble[1] = 2.0;
            vectorDouble[2] = 3.0;
            sentence.InferSentVectorsDouble = vectorDouble;
            Assert.Equal("1;2;3", sentence.InferSentVectorsString);
        }

        [Fact]
        public void CalculateCosineDistanceBetweenSameVectors()
        {
            double[] vector1 = new double[3];
            vector1[0] = 1;
            vector1[1] = 2;
            vector1[2] = 3;
            double[] vector2 = new double[3];
            vector2[0] = 1;
            vector2[1] = 2;
            vector2[2] = 3;
            double distance = DistanceCalculator.CalculateCosineSimilarity(vector1, vector2);
            Assert.True(Math.Abs(distance - 1) <= TOLERANCE);
        }

        [Fact]
        public void CalculateCosineDistanceBetweenDifferentVectors()
        {
            double[] vector1 = new double[3];
            vector1[0] = 1;
            vector1[1] = 2;
            vector1[2] = 3;
            double[] vector2 = new double[3];
            vector2[0] = 7;
            vector2[1] = 8;
            vector2[2] = 9;
            double distance = DistanceCalculator.CalculateCosineSimilarity(vector1, vector2);
            Assert.True(Math.Abs(distance - 0.9594119) <= TOLERANCE);
        }
    }
}
