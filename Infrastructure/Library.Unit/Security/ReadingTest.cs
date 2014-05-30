using Demo.Library.Dynamics;
using Demo.Library.Security;
using Demo.Library.Security.Hows;
using NUnit.Framework;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Security
{
    [TestFixture]
    public class ReadingTest
    {
        private Reading _reading;

        [SetUp]
        public void Setup()
        {
            _reading = new Reading(ObjectFactory.Container);
        }

        [Test]
        public void Intercept_call()
        {
            var policy = new Demo.Library.Security.Policies.Default();
            var rule = new Demo.Library.Security.Rules.Default();
            rule.Hows.Add(new MatchDefinition { Operation = "equal", Name = "action-id", Value = "read" });
            rule.Whats.Add(new MatchDefinition { Operation = "equal", Name = "context", Value = "test" });
            rule.Whos.Add(new MatchDefinition { Operation = "equal", Name = "id", Value = "john" });
            policy.Rules.Add(new Tuple<IRule, Effect>(rule, Effect.ALLOW));

            var request = new Demo.Library.Security.Requests.Default();
            request.SetHow("action-id", "read");
            request.SetWhat("context", "test");
            request.SetWho("id", "john");

            Assert.True(policy.Matches(request));
            Assert.AreEqual(Effect.ALLOW, policy.Resolve(request));
        }
    }
}