using System;

namespace Forex.Core.Entity {
    public class TickData {
        public DateTime CurrentTime { get; set; }
        public double Index { get; set; }
        public double Price { get; set; }
    }
}
