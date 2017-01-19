using System.Collections.Generic;
using Forex.Experts;

namespace Forex.Mvc.Models {
    public class ChartViewModel {
        public List<TickData> ActualData { get; set; }
        public List<TickData> RegressionData { get; set; }
    }
}