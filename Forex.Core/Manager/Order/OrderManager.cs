using System;
using NQuotes;

namespace Forex.Core.Manager.Order {
    public class OrderManager {
        private readonly MqlApi _api;
        private readonly string _symbol;

        public OrderManager(MqlApi api) {
            _api = api;
            _symbol = _api.Symbol();
        }

        public int Buy(double volume, int slippage, double stopLoss,
            double takeProfit, string comment, int magic, DateTime expirationDate) {

            return OpenOrder(_symbol, MqlApi.OP_BUY, volume, _api.Ask, slippage, stopLoss, takeProfit, comment, magic,
                expirationDate);
        }

        public int Sell(double volume, int slippage, double stopLoss,
            double takeProfit, string comment, int magic, DateTime expirationDate) {

            return OpenOrder(_symbol, MqlApi.OP_SELL, volume, _api.Bid, slippage, stopLoss, takeProfit, comment, magic,
                expirationDate);
        }

        public int BuyLimit(double volume, int slippage, double stopLoss,
            double takeProfit, string comment, int magic, DateTime expirationDate) {

            return OpenOrder(_symbol, MqlApi.OP_BUYLIMIT, volume, _api.Ask, slippage, stopLoss, takeProfit, comment,
                magic,
                expirationDate);
        }

        public int SellLimit(double volume, int slippage, double stopLoss,
            double takeProfit, string comment, int magic, DateTime expirationDate) {

            return OpenOrder(_symbol, MqlApi.OP_SELLLIMIT, volume, _api.Bid, slippage, stopLoss, takeProfit, comment,
                magic,
                expirationDate);
        }

        public int BuyStop(double volume, int slippage, double stopLoss,
            double takeProfit, string comment, int magic, DateTime expirationDate) {

            return OpenOrder(_symbol, MqlApi.OP_BUYSTOP, volume, _api.Ask, slippage, stopLoss, takeProfit, comment,
                magic,
                expirationDate);
        }

        public int SellStop(double volume, int slippage, double stopLoss,
            double takeProfit, string comment, int magic, DateTime expirationDate) {

            return OpenOrder(_symbol, MqlApi.OP_SELLSTOP, volume, _api.Bid, slippage, stopLoss, takeProfit, comment,
                magic,
                expirationDate);
        }

        private int OpenOrder(string symbol, int cmd, double volume, double price, int slippage, double stopLoss,
            double takeProfit, string comment, int magic, DateTime expirationDate) {

            return _api.OrderSend(symbol, cmd, volume, price, slippage, stopLoss, takeProfit, comment, magic,
                expirationDate, System.Drawing.Color.Green);
        }

        public int OrderCount() {
            return _api.OrdersTotal();
        }

        public bool CloseOrders(int orderType) {
            var rv = true;
            var numOfOrders = _api.OrdersTotal();

            for (var index = numOfOrders - 1; index >= 0; index--) {
                _api.OrderSelect(index, MqlApi.SELECT_BY_POS, MqlApi.MODE_TRADES);
                var orderSymbol = _api.OrderSymbol();

                if (orderSymbol == _symbol) {
                    if (_api.OrderType() == orderType) {

                        if (orderType == MqlApi.OP_BUY) {
                            if (
                                !_api.OrderClose(_api.OrderTicket(), _api.OrderLots(),
                                    _api.MarketInfo(orderSymbol, MqlApi.MODE_BID), 0, System.Drawing.Color.Red)) {
                                rv = false;
                            }
                        }

                        if (orderType == MqlApi.OP_SELL) {
                            if (
                                !_api.OrderClose(_api.OrderTicket(), _api.OrderLots(),
                                    _api.MarketInfo(orderSymbol, MqlApi.MODE_ASK), 0,
                                    System.Drawing.Color.Red)) {
                                rv = false;
                            }
                        }

                        if (orderType == MqlApi.OP_BUYLIMIT || orderType == MqlApi.OP_SELLLIMIT ||
                            orderType == MqlApi.OP_BUYSTOP || orderType == MqlApi.OP_SELLSTOP) {
                            if (!_api.OrderDelete(_api.OrderTicket())) {
                                rv = false;
                            }
                        }
                    }
                }
            }

            return rv;
        }
    }
}
