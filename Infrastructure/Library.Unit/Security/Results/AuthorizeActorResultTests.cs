using Demo.Library.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Security.Results
{
    [TestFixture]
    public class AuthorizeActorResultTests
    {
        private AuthorizeActorResult _result;

        [SetUp]
        public void Setup()
        {
            var actor = new Moq.Mock<IActor>();
            actor.Setup(a => a.Description).Returns("Mock");
            _result = new AuthorizeActorResult(actor.Object);
        }

        [Test]
        public void is_authorized_has_no_rules()
        {
            Assert.True(_result.IsAuthorized);
            Assert.AreEqual(0, _result.BrokenRules.Count());
            Assert.AreEqual(0, _result.RulesThatEncounteredAnErrorWhenEvaluating.Count());
        }
        [Test]
        public void is_authorized_has_broken_rule()
        {
            var rule = new Moq.Mock<IRule>();

            _result.AddBrokenRule(rule.Object);

            Assert.False(_result.IsAuthorized);
        }

        [Test]
        public void is_authorized_has_exception_rule()
        {
            var rule = new Moq.Mock<IRule>();

            _result.AddErrorRule(rule.Object, Moq.It.IsAny<Exception>());

            Assert.False(_result.IsAuthorized);
        }

        [Test]
        public void add_broken_rule_has_broken_rule()
        {
            var rule = new Moq.Mock<IRule>();

            _result.AddBrokenRule(rule.Object);

            Assert.AreEqual(1, _result.BrokenRules.Count());
        }

        [Test]
        public void add_exception_rule_has_exception_rule()
        {
            var rule = new Moq.Mock<IRule>();

            _result.AddErrorRule(rule.Object, Moq.It.IsAny<Exception>());

            Assert.AreEqual(1, _result.RulesThatEncounteredAnErrorWhenEvaluating.Count());
        }

        [Test]
        public void add_broken_rule_has_failed_message()
        {
            var rule = new Moq.Mock<IRule>();
            rule.Setup(x => x.Description).Returns("test");

            _result.AddBrokenRule(rule.Object);

            Assert.AreEqual(1, _result.BuildFailedAuthorizationMessages().Count());
        }
        [Test]
        public void add_error_rule_has_failed_message()
        {
            var rule = new Moq.Mock<IRule>();
            rule.Setup(x => x.Description).Returns("test");

            _result.AddErrorRule(rule.Object, Moq.It.IsAny<Exception>());

            Assert.AreEqual(1, _result.BuildFailedAuthorizationMessages().Count());
        }
        [Test]
        public void add_both_has_two_failed_messages()
        {
            var rule = new Moq.Mock<IRule>();
            rule.Setup(x => x.Description).Returns("test");

            _result.AddBrokenRule(rule.Object);
            _result.AddErrorRule(rule.Object, Moq.It.IsAny<Exception>());

            Assert.AreEqual(2, _result.BuildFailedAuthorizationMessages().Count());
        }
    }
}