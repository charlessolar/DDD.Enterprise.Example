using Demo.Library.Security;
using Demo.Library.Security.Actors;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Security.Actors
{
    [TestFixture]
    public class ActorTests
    {
        private Actor _actor;

        [SetUp]
        public void Setup()
        {
            _actor = new Actor();
        }

        [Test]
        public void Should_have_description()
        {
            var actor = new Actor("test");
            Assert.AreEqual("test", actor.Description);
        }

        [Test]
        public void Should_not_have_description()
        {
            var actor = new Actor();
            Assert.True(String.IsNullOrEmpty(actor.Description));
        }

        [Test]
        public void Add_rule_is_added()
        {
            var rule = new Moq.Mock<IRule>();

            Assert.True(_actor.Rules.Count() == 0);
            _actor.AddRule(rule.Object);
            Assert.True(_actor.Rules.Count() == 1);
        }

        [Test]
        public void Authorize_authorized()
        {
            var rule = new Moq.Mock<IRule>();
            rule.Setup<Boolean>(x => x.IsAuthorized(null)).Returns(true);

            _actor.AddRule(rule.Object);

            Assert.True(_actor.IsAuthorized(null).IsAuthorized);
            Assert.AreEqual(0, _actor.IsAuthorized(null).BrokenRules.Count());
        }

        [Test]
        public void Authorize_not_authorized()
        {
            var rule = new Moq.Mock<IRule>();
            rule.Setup<Boolean>(x => x.IsAuthorized(null)).Returns(false);

            _actor.AddRule(rule.Object);

            Assert.False(_actor.IsAuthorized(null).IsAuthorized);
            Assert.AreEqual(1, _actor.IsAuthorized(null).BrokenRules.Count());
        }

        [Test]
        public void Authorize_both_authorized()
        {
            var rule1 = new Moq.Mock<IRule>();
            rule1.Setup<Boolean>(x => x.IsAuthorized(null)).Returns(true);
            var rule2 = new Moq.Mock<IRule>();
            rule2.Setup<Boolean>(x => x.IsAuthorized(null)).Returns(true);

            _actor.AddRule(rule1.Object);
            _actor.AddRule(rule2.Object);

            Assert.True(_actor.IsAuthorized(null).IsAuthorized);
            Assert.AreEqual(0, _actor.IsAuthorized(null).BrokenRules.Count());
        }

        [Test]
        public void Authorize_one_authorized()
        {
            var rule1 = new Moq.Mock<IRule>();
            rule1.Setup<Boolean>(x => x.IsAuthorized(null)).Returns(false);
            var rule2 = new Moq.Mock<IRule>();
            rule2.Setup<Boolean>(x => x.IsAuthorized(null)).Returns(true);

            _actor.AddRule(rule1.Object);
            _actor.AddRule(rule2.Object);

            Assert.False(_actor.IsAuthorized(null).IsAuthorized);
            Assert.AreEqual(1, _actor.IsAuthorized(null).BrokenRules.Count());
        }

        [Test]
        public void Authorize_rule_throws()
        {
            var rule = new Moq.Mock<IRule>();
            rule.Setup<Boolean>(x => x.IsAuthorized(null)).Throws(new Exception());

            _actor.AddRule(rule.Object);

            Assert.False(_actor.IsAuthorized(null).IsAuthorized);
            Assert.True(_actor.IsAuthorized(null).BrokenRules.Count() == 0);
            Assert.AreEqual(1, _actor.IsAuthorized(null).RulesThatEncounteredAnErrorWhenEvaluating.Count());
        }
    }
}