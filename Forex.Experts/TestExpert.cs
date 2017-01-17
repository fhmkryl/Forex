using System;
using System.Collections.Generic;
using System.Diagnostics;
using Forex.Core.Entity;
using Forex.Core.Manager.Order;
using NQuotes;

namespace Forex.Experts {
    public class TestExpert : MqlApi {
        private static Bar _lastBar = new Bar();
        private static List<TickData> _tickDatas = new List<TickData>();
        public override int start() {
            //var account = new Account(this);

            //var orderManager = new OrderManager(this);
            //var result = orderManager.Buy(1, 3, 0, 0, "New Order", 0, DateTime.MaxValue);

            var currentBar = new Bar {
                Open = Open[0],
                Close = Close[0],
                High = High[0],
                Low = Low[0],
                CurrentTime = TimeCurrent(),
                OpenTime = Time[0]
            };

            if (_lastBar.OpenTime != currentBar.OpenTime) {
                _lastBar = currentBar;
                _tickDatas = new List<TickData>();
                Trace.WriteLine("New Bar Opened!");
            }
            else {
                _tickDatas.Add(new TickData {
                    CurrentTime = currentBar.CurrentTime,
                    Price = currentBar.Close
                });

                AnalyzeTickData();
            }


            Trace.WriteLine(currentBar);

            return 0;
        }

        private int AnalyzeTickData() {
            // Todo: Linear regression must be implemented
            throw new NotImplementedException();
        }
    }

    public class TickData {
        public DateTime CurrentTime { get; set; }
        public double Price { get; set; }
    }
}
