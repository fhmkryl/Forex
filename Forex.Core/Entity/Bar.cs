using System;
using Newtonsoft.Json;

namespace Forex.Core.Entity {
    public class Bar {
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
        public DateTime CurrentTime { get; set; }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}
