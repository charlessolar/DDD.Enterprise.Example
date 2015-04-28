using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Currency.Services
{
    [Api("Accounting")]
    [Route("/accounting/currency/{CurrencyId}/rates/{RateId}/effective", "PUT POST")]
    public class ChangeRateEffective
    {
        public Guid CurrencyId { get; set; }
        public Guid RateId { get; set; }
        public DateTime? EffectiveTill { get; set; }
    }
}
