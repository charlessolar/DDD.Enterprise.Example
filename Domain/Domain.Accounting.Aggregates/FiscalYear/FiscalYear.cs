using Aggregates.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.FiscalYear
{
    public class FiscalYear : Aggregates.Aggregate<Guid>, IFiscalYear
    {
        public ValueObjects.Start Started { get; private set; }

        public ValueObjects.End Ended { get; private set; }

        private FiscalYear()
        {
        }

        public void Create(String Name, String Code)
        {
            this.Started = new ValueObjects.Start();
            this.Ended = new ValueObjects.End();

            Apply<Events.Created>(e =>
            {
                e.FiscalYearId = Id;
                e.Code = Code;
                e.Name = Name;
            });
        }

        public void ChangeName(String Name)
        {
            Apply<Events.NameChanged>(e =>
            {
                e.FiscalYearId = Id;
                e.Name = Name;
            });
        }

        public void Destroy()
        {
            if (this.Started.Date.HasValue)
                throw new BusinessException("Fiscal year already started");

            Apply<Events.Destroyed>(e =>
            {
                e.FiscalYearId = Id;
            });
        }

        public void End(DateTime Effective)
        {
            if (!this.Started.Date.HasValue)
                throw new BusinessException("Fiscal year not started");
            if (this.Ended.Date.HasValue)
                throw new BusinessException("Fiscal year already ended");

            Apply<Events.Ended>(e =>
            {
                e.FiscalYearId = Id;
                e.Effective = Effective;
            });
        }

        public void Start(DateTime Effective)
        {
            if (this.Started.Date.HasValue)
                throw new BusinessException("Fiscal year already started");

            Apply<Events.Started>(e =>
            {
                e.FiscalYearId = Id;
                e.Effective = Effective;
            });
        }
    }
}