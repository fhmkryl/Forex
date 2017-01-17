using Newtonsoft.Json;
using NQuotes;

namespace Forex.Core.Entity {
    public class Account {

        private readonly MqlApi _api;
        public Account(MqlApi api) {
            _api = api;
        }

        public double AccountBalance {
            get { return _api.AccountBalance(); }
        }

        public double AccountMargin {
            get { return _api.AccountMargin(); }
        }

        public double AccountFreeMargin {
            get { return _api.AccountFreeMargin(); }
        }

        public double AccountCredit {
            get { return _api.AccountCredit(); }
        }

        public double AccountEquity {
            get { return _api.AccountEquity(); }
        }

        public string AccountName {
            get { return _api.AccountName(); }
        }

        public string AccountCompany {
            get { return _api.AccountCompany(); }
        }

        public string AccountCurrency {
            get { return _api.AccountCurrency(); }
        }

        public string AccountServer {
            get { return _api.AccountServer(); }
        }

        public double AccountStopoutLevel {
            get { return _api.AccountStopoutLevel(); }
        }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}
