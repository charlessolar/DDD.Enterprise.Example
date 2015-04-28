using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.FiscalYear
{
    public class Handler :
        IHandleMessages<Commands.ChangeName>,
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.Destroy>,
        IHandleMessages<Commands.End>,
        IHandleMessages<Commands.Start>
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
            var fy = _uow.R<FiscalYear>().New(command.FiscalYearId);
            fy.Create(command.Name, command.Code);
        }

        public void Handle(Commands.ChangeName command)
        {
            var fy = _uow.R<FiscalYear>().Get(command.FiscalYearId);
            fy.ChangeName(command.Name);
        }

        public void Handle(Commands.Destroy command)
        {
            var fy = _uow.R<FiscalYear>().Get(command.FiscalYearId);
            fy.Destroy();
        }

        public void Handle(Commands.End command)
        {
            var fy = _uow.R<FiscalYear>().Get(command.FiscalYearId);
            fy.End(command.Effective);
        }

        public void Handle(Commands.Start command)
        {
            var fy = _uow.R<FiscalYear>().Get(command.FiscalYearId);
            fy.Start(command.Effective);
        }
    }
}