﻿using System;
using System.Collections.Generic;

namespace Assets.Scripts.Handlers
{
    public static class SrtHandler
    {
        private const double Z = 0.255;  // p(X < 0.255) = 60%

        /// <summary>
        /// This methond calculates the simple reaction time (SRT)
        /// </summary>
        /// <param name="results"></param>
        public static double GetAcceptableReationTime(double[] results)
        {
            double mean = 0;
            var valid = 0;
            foreach (var result in results)
            {
                if (result < 0) continue;
                mean += result;
                valid++;
            }
            if (valid == 0)
                return double.MaxValue;
            
            mean /= valid;
            
            double sumOfSquaresOfDifferences = 0;
            foreach (var result in results)
            {
                if (result < 0) continue;
                sumOfSquaresOfDifferences += Math.Pow(result - mean, 2);
            }
            double stDev = Math.Sqrt(sumOfSquaresOfDifferences / valid);

            return Z * stDev + mean;
        }

	    public static double GetAcceptableReationTime(List<double> results)
	    {
		    double mean = 0;
		    var valid = 0;
		    foreach (var result in results)
		    {
			    if (result < 0) continue;
			    mean += result;
			    valid++;
		    }
		    if (valid == 0)
			    return double.MaxValue;

		    mean /= valid;

		    double sumOfSquaresOfDifferences = 0;
		    foreach (var result in results)
		    {
			    if (result < 0) continue;
			    sumOfSquaresOfDifferences += Math.Pow(result - mean, 2);
		    }
		    double stDev = Math.Sqrt(sumOfSquaresOfDifferences / valid);

		    return Z * stDev + mean;
	    }

		public static double GetMean(double[] results)
        {
            double mean = 0;
            var valid = 0;
            foreach (var result in results)
            {
                if (result < 0) continue;
                mean += result;
                valid++;
            }
            if (valid == 0)
                return double.MaxValue;
            
            mean /= valid;
            return mean;
        }

	    public static int HowManyValid(double[] results)
	    {
		    int valid = 0;
			foreach (var result in results)
			{
				if (result < 0) continue;
				valid++;
			}
		    return valid;
	    }
    }
}


