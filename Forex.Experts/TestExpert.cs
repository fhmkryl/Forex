using System;
using Forex.Core.Entity;
using Forex.Core.Manager.Order;
using NQuotes;

namespace Forex.Experts {
    public class TestExpert : MqlApi {
        public override int start() {
            var account = new Account(this);
            
            var orderManager = new OrderManager(this);
            var result = orderManager.Buy(1, 3, 0, 0, "New Order", 0, DateTime.MaxValue);

            return 0;
        }
    }
}
