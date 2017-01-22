using System;
using System.Collections.Generic;
using System.Linq;

namespace Forex.Core.MathLib {
   

    public class LinearRegression {
        public double XSum;
        public double YSum;
        public double XySum;
        public double XSquareSum;
        public double Count = 0;

        /// <summary>
        /// Variables for linear function formula -> y=mx+n
        /// </summary>
        public double M { get; internal set; }

        public double N { get; internal set; }


        /// <summary>
        /// Regression Equation(y) = a + bx 
        /// Slope(b) = (NΣXY - (ΣX)(ΣY)) / (NΣX2 - (ΣX)2)
        /// Intercept(a) = (ΣY - b(ΣX)) / N
        /// </summary>
        public void DoLinearRegression(double x,double y) {
            XSum += x;
            YSum += y;
            XySum += (x*y);
            XSquareSum += (x*x);
            Count++;
            M = (Count * XySum - XSum * YSum) /(Count * XSquareSum - Math.Pow(XSquareSum, 2));
            N = (YSum - M* XSum) / Count;

            //var sumOfX = _xValues.Sum();
            //var sumOfY = _yValues.Sum();
            //var sumOfXy = _xValues.Select((x, index) => x * _yValues[index]).Sum();
            //var sumOfXSquare = _xValues.Select(x => x * x).Sum();

            //var countOfPoints = _xValues.Count;
            //M = (countOfPoints * sumOfXy - sumOfX * sumOfY) / (countOfPoints * sumOfXSquare - Math.Pow(sumOfXSquare, 2));
            //N = (sumOfY - M * sumOfX) / countOfPoints;
        }

        public double GetPredictedYValue(double x) {
            return M*x + N;
        }
    }
}
