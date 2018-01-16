using System;
using System.Linq;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Classes
{
    public class Baseline : IBaseline
    {
        private const double Z = 0.255;  // p(X < 0.255) = 60%
        private double _simpleReactionTime;

        /// <summary>
        /// This methond calculates the simple reaction time (SRT)
        /// </summary>
        /// <param name="results"></param>
        public void SendResults(double[] results)
        {
            double mean = results.Average();
            double sumOfSquaresOfDifferences = results.Select(val => (val - mean) * (val - mean)).Sum();
            double stDev = Math.Sqrt(sumOfSquaresOfDifferences / results.Length);

            _simpleReactionTime = Z * stDev + mean;
        }

        public double GetSrt()
        {
            return _simpleReactionTime;
        }
    }
}


