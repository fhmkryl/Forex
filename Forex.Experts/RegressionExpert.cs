using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Forex.Core.Entity;
using Forex.Core.Manager.Order;
using Forex.Core.MathLib;
using NQuotes;

namespace Forex.Experts {
    public class RegressionExpert : MqlApi {
        private static int _takeProfit = 50;
        private static int _stopLoss = 25;

        private static int _index = 1;
        private static Bar _lastBar;
        private static int _lastTicketId;
        private static double _maxLossInPips;

        private static readonly List<TickData> ActualData = new List<TickData>();
        private static readonly List<TickData> RegData = new List<TickData>();
        private static readonly List<TickData> ReglineData = new List<TickData>();

        public override int start() {
            // Collect data
            CollectActualData();
            CollectRegData();
            CollectRegLineData();

            OpenPosition();

            DumpProfitToFile();

            return 0;
        }

        private void OpenPosition() {
            if (ActualData == null) {
                return;
            }
            if (ActualData.Count < 10) {
                return;
            }

            var regression = new LinearRegression(RegData.Select(p => p.Index).ToList(),
                RegData.Select(p => p.Price).ToList());
            var predictedValue = regression.GetPredictedYValue(RegData.Count + 1);

            var orderManager = new OrderManager(this);
            //if (predictedValue < RegData[RegData.Count - 1].Price + Point * 3) {
            //    orderManager.CloseOrders(OP_SELL);
            //}
            //if (predictedValue > RegData[RegData.Count - 1].Price - Point * 3) {
            //    orderManager.CloseOrders(OP_BUY);
            //}

            
            if (predictedValue - Point*5 > RegData[RegData.Count - 1].Price) {
                orderManager.CloseOrders(OP_SELL);
                if (orderManager.OrderCount() <= 0) {
                    _lastTicketId = orderManager.Buy(1, 0, 0, Ask + Point*_takeProfit,
                        "Buy order opened",
                        0, DateTime.MaxValue);
                    DumpDataToFile(_lastTicketId.ToString());
                }
            }
            if (predictedValue + Point * 5 < RegData[RegData.Count - 1].Price) {
                orderManager.CloseOrders(OP_BUY);
                if (orderManager.OrderCount() <= 0) {
                    _lastTicketId = orderManager.Sell(1, 0,0, Bid - Point * _takeProfit,
                        "Sell order opened",
                        0, DateTime.MaxValue);
                    DumpDataToFile(_lastTicketId.ToString());
                }
            }
        }

        private void CollectActualData() {
            if (_lastBar == null) {
                _lastBar = new Bar {
                    CurrentTime = Time[0]
                };
            }

            //if (_lastBar.CurrentTime != Time[0]) {
            //    _lastBar.CurrentTime = Time[0];
            //    _index = 1;
            //    _actualData = new List<TickData>();
            //}

            ActualData.Add(new TickData() {
                Index = _index,
                Price = (Ask + Bid)/2,
                CurrentTime = TimeCurrent()
            });
            _index++;
        }

        private void CollectRegData() {
            var reg = new LinearRegression(ActualData.Select(p => p.Index).ToList(),
                ActualData.Select(p => p.Price).ToList());
            var predictedValue = reg.GetPredictedYValue(ActualData.Count + 1);
            if (double.IsNaN(predictedValue)) {
                predictedValue = ActualData.Select(p => p.Price).Min();
            }

            RegData.Add(new TickData() {
                Index = ActualData.Count + 1,
                Price = predictedValue
            });
        }

        private void CollectRegLineData() {
            var reg = new LinearRegression(RegData.Select(p => p.Index).ToList(),
                RegData.Select(p => p.Price).ToList());
            var predictedValue = reg.GetPredictedYValue(RegData.Count + 1);
            if (double.IsNaN(predictedValue)) {
                predictedValue = RegData.Select(p => p.Price).Min();
            }

            ReglineData.Add(new TickData() {
                Index = RegData.Count + 1,
                Price = predictedValue
            });
        }

        private void DumpDataToFile(string orderType) {
            var filePath = string.Format("Positions/{0}.txt", orderType);
            for (int i = 0; i < ActualData.Count; i++) {
                var actData = ActualData[i];
                var regData = RegData[i];
                var regline = ReglineData[i];

                var line = string.Format("{0},{1},{2},{3},{4},{5}", i, Math.Round(actData.Price, 2),
                    Math.Round(regData.Price, 2), Math.Round(regline.Price, 2),
                    Math.Round(actData.Price - regline.Price, 2)*100, Math.Round(regData.Price - regline.Price, 2)*100);

                System.IO.File.AppendAllLines(filePath, new List<string> {line});
            }
        }

        private void DumpProfitToFile() {
            return;
            if (Math.Abs(_maxLossInPips) < Math.Abs(OrderProfit() / 10)) {
                _maxLossInPips = OrderProfit() / 10;
            }

            if (_lastTicketId > 0 && _maxLossInPips < 0) {
                var filePath = string.Format("Positions/{0}.txt", _lastTicketId);
                var line = string.Format("{0}->{1}", _lastTicketId, _maxLossInPips);
                System.IO.File.WriteAllLines(filePath, new List<string>() { line });
            }
        }
    }
}
