using System;
using System.Collections.Generic;

namespace Forex.Core.MathLib {
    public static class MathNetSimpleRegression {
        public static Tuple<double, double> Fit(List<double> xData, List<double> yData) {
            return MathNet.Numerics.LinearRegression.SimpleRegression.Fit(xData.ToArray(), yData.ToArray());
        }
    }
}
