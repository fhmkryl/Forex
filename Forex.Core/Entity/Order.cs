using System;

namespace Forex.Core.Entity {
    public class Order {
        public double Lots { get; set; }
        public double OpenPrice { get; set; }
        public int Type { get; set; }
        public DateTime OpenTime { get; set; }
    }
}
