﻿using Demo.Domain.Accounting.FiscalYear.Events;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Handlers
{
    public class Get :
        IHandleMessages<Created>,
        IHandleMessages<Destroyed>,
        IHandleMessages<Ended>,
        IHandleMessages<NameChanged>,
        IHandleMessages<Started>
    {
        private readonly IDocumentStore _store;

        public Get(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(Created e)
        {
            using (var session = _store.OpenSession())
            {
                var get = new Responses.Get
                {
                    Id = e.FiscalYearId,
                    Code = e.Code,
                    Name = e.Name,
                };

                session.Store(get);
                session.SaveChanges();
            }
        }

        public void Handle(Destroyed e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.FiscalYearId);

                session.Delete(get);
                session.SaveChanges();
            }
        }

        public void Handle(Ended e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.FiscalYearId);
                get.End = e.Effective;

                session.Store(get);
                session.SaveChanges();
            }
        }

        public void Handle(NameChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.FiscalYearId);
                get.Name = e.Name;

                session.Store(get);
                session.SaveChanges();
            }
        }

        public void Handle(Started e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.FiscalYearId);
                get.Start = e.Effective;

                session.Store(get);
                session.SaveChanges();
            }
        }
    }
}