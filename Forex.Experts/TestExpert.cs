using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Forex.Core.Entity;
using Forex.Core.Manager.Order;
using Forex.Core.MathLib;
using NQuotes;

namespace Forex.Experts {
    public class TestExpert : MqlApi {
        private static Bar _lastBar = new Bar();
        private static List<TickData> _tickDatas = new List<TickData>();
        public override int start() {
            
            //var account = new Account(this);

            //var orderManager = new OrderManager(this);
            //var result = orderManager.Buy(1, 3, 0, 0, "New Order", 0, DateTime.MaxValue);

            //var currentBar = new Bar {
            //    Open = Open[0],
            //    Close = Close[0],
            //    High = High[0],
            //    Low = Low[0],
            //    CurrentTime = TimeCurrent(),
            //    OpenTime = Time[0]
            //};

            //if (_lastBar.OpenTime != currentBar.OpenTime) {
            //    _lastBar = currentBar;
            //    _tickDatas = new List<TickData>();
            //    Trace.WriteLine("New Bar Opened!");
            //}
            //else {
            //    _tickDatas.Add(new TickData {
            //        CurrentTime = currentBar.CurrentTime,
            //        Price = currentBar.Close
            //    });
            //}


            //Trace.WriteLine(currentBar);

            //Analyze();
            _tickDatas.Add(new TickData {
                CurrentTime = TimeCurrent(),
                Price = (Ask + Bid)/2
            });

            System.IO.File.AppendAllLines("Output.txt", new List<string>() {
                TimeCurrent().ToString() + "," + Open[0]
            });
            
            return 0;
        }

        private void Analyze() {
            var regression = new LinearRegression(_tickDatas.Select((p, index) => (double) index).ToList(),
                _tickDatas.Select(p => p.Price).ToList());

            var nextIndex = (double) (_tickDatas.Count + 1);

            var predictedValue =  regression.GetPredictedYValue(nextIndex);

            Trace.WriteLine("PredictedValue:" + predictedValue.ToString(CultureInfo.InvariantCulture) + " Formula:" +
                            regression.M + "x+" + regression.N);
        }
    }

    public class TickData {
        public DateTime CurrentTime { get; set; }
        public double Time { get; set; }
        public double Price { get; set; }
    }
}
