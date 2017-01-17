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

            return OpenOrder(_symbol, MqlApi.OP_BUY, volume, slippage, stopLoss, takeProfit, comment, magic,
                expirationDate);
        }

        public int Sell(double volume, int slippage, double stopLoss,
            double takeProfit, string comment, int magic, DateTime expirationDate) {

            return OpenOrder(_symbol, MqlApi.OP_SELL, volume, slippage, stopLoss, takeProfit, comment, magic,
                expirationDate);
        }

        public int BuyLimit(double volume, int slippage, double stopLoss,
            double takeProfit, string comment, int magic, DateTime expirationDate) {

            return OpenOrder(_symbol, MqlApi.OP_BUYLIMIT, volume, slippage, stopLoss, takeProfit, comment, magic,
                expirationDate);
        }

        public int SellLimit(double volume, int slippage, double stopLoss,
            double takeProfit, string comment, int magic, DateTime expirationDate) {

            return OpenOrder(_symbol, MqlApi.OP_SELLLIMIT, volume, slippage, stopLoss, takeProfit, comment, magic,
                expirationDate);
        }

        public int BuyStop(double volume, int slippage, double stopLoss,
            double takeProfit, string comment, int magic, DateTime expirationDate) {

            return OpenOrder(_symbol, MqlApi.OP_BUYSTOP, volume, slippage, stopLoss, takeProfit, comment, magic,
                expirationDate);
        }

        public int SellStop(double volume, int slippage, double stopLoss,
            double takeProfit, string comment, int magic, DateTime expirationDate) {

            return OpenOrder(_symbol, MqlApi.OP_SELLSTOP, volume, slippage, stopLoss, takeProfit, comment, magic,
                expirationDate);
        }

        private int OpenOrder(string symbol, int cmd, double volume, int slippage, double stopLoss,
            double takeProfit, string comment, int magic, DateTime expirationDate) {

            return _api.OrderSend(symbol, cmd, volume, _api.Ask, slippage, stopLoss, takeProfit, comment, magic,
                expirationDate);
        }
    }
}
