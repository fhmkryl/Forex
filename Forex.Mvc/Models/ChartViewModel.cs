using System.Collections.Generic;
using Forex.Core.Entity;

namespace Forex.Mvc.Models {
    public class ChartViewModel {
        public List<TickData> ActualData { get; set; }
        public List<TickData> RegressionData { get; set; }
        public List<TickData> RegressionLineData { get; set; }
        public string Explanation { get; set; }
    }
}