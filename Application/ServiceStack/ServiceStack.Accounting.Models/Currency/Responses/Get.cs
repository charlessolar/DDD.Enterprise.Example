using Demo.Library.Responses;
using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace Demo.Application.ServiceStack.Accounting.Currency.Responses
{
    public class Get : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public String Symbol { get; set; }

        public Boolean SymbolBefore { get; set; }

        public Decimal RoundingFactor { get; set; }

        public Int32 ComputationalAccuracy { get; set; }

        public String Format { get; set; }

        public String Fraction { get; set; }

        public Boolean Activated { get; set; }
    }
}