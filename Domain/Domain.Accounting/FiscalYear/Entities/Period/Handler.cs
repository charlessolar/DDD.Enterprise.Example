using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.FiscalYear.Entities.Period
{
    public class Handler :
        IHandleMessages<Commands.ChangeName>,
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.Destroy>,
        IHandleMessages<Commands.Start>,
        IHandleMessages<Commands.End>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBus _bus;

        public Handler(IUnitOfWork uow, IBus bus)
        {
            _uow = uow;
            _bus = bus;
        }

        public void Handle(Commands.Create command)
        {
            var fy = _uow.R<FiscalYear>().Get(command.FiscalYearId);
            var period = fy.Entity<Period>().New(command.PeriodId);
            period.Create(command.Name, command.Code);
        }

        public void Handle(Commands.ChangeName command)
        {
            var fy = _uow.R<FiscalYear>().Get(command.FiscalYearId);
            var period = fy.Entity<Period>().Get(command.PeriodId);
            period.ChangeName(command.Name);
        }

        public void Handle(Commands.Destroy command)
        {
            var fy = _uow.R<FiscalYear>().Get(command.FiscalYearId);
            var period = fy.Entity<Period>().Get(command.PeriodId);
            period.Destroy();
        }

        public void Handle(Commands.End command)
        {
            var fy = _uow.R<FiscalYear>().Get(command.FiscalYearId);
            var period = fy.Entity<Period>().Get(command.PeriodId);
            period.End(command.Effective);
        }

        public void Handle(Commands.Start command)
        {
            var fy = _uow.R<FiscalYear>().Get(command.FiscalYearId);
            var period = fy.Entity<Period>().Get(command.PeriodId);
            period.Start(command.Effective);
        }
    }
}