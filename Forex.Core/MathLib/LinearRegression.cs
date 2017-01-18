using System;
using System.Collections.Generic;
using System.Linq;

namespace Forex.Core.MathLib {
    public class LinearRegression {
        private readonly List<double> _xValues;
        private readonly List<double> _yValues;

        /// <summary>
        /// Variables for linear function formula -> y=mx+n
        /// </summary>
        public double M { get; internal set; }

        public double N { get; internal set; }

        public LinearRegression(List<double> xValues, List<double> yValues) {
            _xValues = xValues;
            _yValues = yValues;

            DoLinearRegression();
        }

        /// <summary>
        /// Regression Equation(y) = a + bx 
        /// Slope(b) = (NΣXY - (ΣX)(ΣY)) / (NΣX2 - (ΣX)2)
        /// Intercept(a) = (ΣY - b(ΣX)) / N
        /// </summary>
        private void DoLinearRegression() {
            var sumOfX = _xValues.Sum();
            var sumOfY = _yValues.Sum();
            var sumOfXy = _xValues.Select((x, index) => x*_yValues[index]).Sum();
            var sumOfXSquare = _xValues.Select(x => x*x).Sum();

            var countOfPoints = _xValues.Count;
            M = (countOfPoints*sumOfXy - sumOfX*sumOfY)/(countOfPoints*sumOfXSquare - Math.Pow(sumOfXSquare, 2));
            N = (sumOfY - M*sumOfX)/countOfPoints;
        }

        public double GetPredictedYValue(double xValue) {
            return M*xValue + N;
        }
    }
}
