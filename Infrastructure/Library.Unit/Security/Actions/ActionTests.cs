using Demo.Library.Security;
using Demo.Library.Security.Actions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Security.Actions
{
    [TestFixture]
    public class ActionTests
    {
        private Demo.Library.Security.Actions.Action _action;

        [SetUp]
        public void Setup()
        {
            _action = new Library.Security.Actions.Action();
        }

        [Test]
        public void Should_have_description()
        {
            var action = new Demo.Library.Security.Actions.Action("test");
            Assert.AreEqual("test", action.Description);
        }

        [Test]
        public void Should_not_have_description()
        {
            var action = new Demo.Library.Security.Actions.Action();
            Assert.True(String.IsNullOrEmpty(action.Description));
        }

        [Test]
        public void Add_target_is_added()
        {
            var target = new Moq.Mock<ITarget>();

            Assert.True(_action.Targets.Count() == 0);
            _action.AddTarget(target.Object);
            Assert.True(_action.Targets.Count() == 1);
        }

        [Test]
        public void Can_authorize()
        {
            var target = new Moq.Mock<ITarget>();
            target.Setup(t => t.CanAuthorize(null)).Returns(true);

            _action.AddTarget(target.Object);
            Assert.True(_action.CanAuthorize(null));
        }

        [Test]
        public void Cant_authorize()
        {
            var target = new Moq.Mock<ITarget>();
            target.Setup(t => t.CanAuthorize(null)).Returns(false);

            _action.AddTarget(target.Object);
            Assert.False(_action.CanAuthorize(null));
        }

        [Test]
        public void One_can_one_cant_authorize()
        {
            var target1 = new Moq.Mock<ITarget>();
            target1.Setup(t => t.CanAuthorize(null)).Returns(false);
            var target2 = new Moq.Mock<ITarget>();
            target2.Setup(t => t.CanAuthorize(null)).Returns(true);

            _action.AddTarget(target1.Object);
            _action.AddTarget(target2.Object);

            Assert.True(_action.CanAuthorize(null));
        }

        [Test]
        public void Authorize_authorized()
        {
            var target = new Moq.Mock<ITarget>();
            target.Setup(t => t.CanAuthorize(null)).Returns(true);

            var result = new Moq.Mock<AuthorizeTargetResult>(target.Object);
            result.SetupGet<Boolean>(t => t.IsAuthorized).Returns(true);
            target.Setup<AuthorizeTargetResult>(t => t.Authorize(null)).Returns(result.Object);

            _action.AddTarget(target.Object);
            Assert.True(_action.Authorize(null).IsAuthorized);
            Assert.AreEqual(0, _action.Authorize(null).AuthorizationFailures.Count());
        }

        [Test]
        public void Authorize_not_authorized()
        {
            var target = new Moq.Mock<ITarget>();
            target.Setup(t => t.CanAuthorize(null)).Returns(true);

            var result = new Moq.Mock<AuthorizeTargetResult>(target.Object);
            result.SetupGet<Boolean>(t => t.IsAuthorized).Returns(false);
            target.Setup<AuthorizeTargetResult>(t => t.Authorize(null)).Returns(result.Object);

            _action.AddTarget(target.Object);
            Assert.False(_action.Authorize(null).IsAuthorized);
            Assert.AreEqual(1, _action.Authorize(null).AuthorizationFailures.Count());
        }

        [Test]
        public void Authorize_both_authorized()
        {
            var target1 = new Moq.Mock<ITarget>();
            target1.Setup(t => t.CanAuthorize(null)).Returns(true);
            var target2 = new Moq.Mock<ITarget>();
            target2.Setup(t => t.CanAuthorize(null)).Returns(true);

            var result1 = new Moq.Mock<AuthorizeTargetResult>(target1.Object);
            result1.SetupGet<Boolean>(t => t.IsAuthorized).Returns(true);
            var result2 = new Moq.Mock<AuthorizeTargetResult>(target2.Object);
            result2.SetupGet<Boolean>(t => t.IsAuthorized).Returns(true);

            target1.Setup<AuthorizeTargetResult>(t => t.Authorize(null)).Returns(result1.Object);
            target2.Setup<AuthorizeTargetResult>(t => t.Authorize(null)).Returns(result2.Object);

            _action.AddTarget(target1.Object);
            _action.AddTarget(target2.Object);

            Assert.True(_action.Authorize(null).IsAuthorized);
            Assert.AreEqual(0, _action.Authorize(null).AuthorizationFailures.Count());
        }
        [Test]
        public void Authorize_one_authorized()
        {
            var target1 = new Moq.Mock<ITarget>();
            target1.Setup(t => t.CanAuthorize(null)).Returns(true);
            var target2 = new Moq.Mock<ITarget>();
            target2.Setup(t => t.CanAuthorize(null)).Returns(true);

            var result1 = new Moq.Mock<AuthorizeTargetResult>(target1.Object);
            result1.SetupGet<Boolean>(t => t.IsAuthorized).Returns(false);
            var result2 = new Moq.Mock<AuthorizeTargetResult>(target2.Object);
            result2.SetupGet<Boolean>(t => t.IsAuthorized).Returns(true);

            target1.Setup<AuthorizeTargetResult>(t => t.Authorize(null)).Returns(result1.Object);
            target2.Setup<AuthorizeTargetResult>(t => t.Authorize(null)).Returns(result2.Object);

            _action.AddTarget(target1.Object);
            _action.AddTarget(target2.Object);

            Assert.False(_action.Authorize(null).IsAuthorized);
            Assert.AreEqual(1, _action.Authorize(null).AuthorizationFailures.Count());
        }

        [Test]
        public void Authorize_one_able_authorized()
        {
            var target1 = new Moq.Mock<ITarget>();
            target1.Setup(t => t.CanAuthorize(null)).Returns(false);
            var target2 = new Moq.Mock<ITarget>();
            target2.Setup(t => t.CanAuthorize(null)).Returns(true);

            var result1 = new Moq.Mock<AuthorizeTargetResult>(target1.Object);
            result1.SetupGet<Boolean>(t => t.IsAuthorized).Returns(true);
            var result2 = new Moq.Mock<AuthorizeTargetResult>(target2.Object);
            result2.SetupGet<Boolean>(t => t.IsAuthorized).Returns(true);

            target1.Setup<AuthorizeTargetResult>(t => t.Authorize(null)).Returns(result1.Object);
            target2.Setup<AuthorizeTargetResult>(t => t.Authorize(null)).Returns(result2.Object);

            _action.AddTarget(target1.Object);
            _action.AddTarget(target2.Object);

            Assert.True(_action.Authorize(null).IsAuthorized);
            Assert.AreEqual(0, _action.Authorize(null).AuthorizationFailures.Count());
        }

        [Test]
        public void Authorize_one_able_not_authorized()
        {
            var target1 = new Moq.Mock<ITarget>();
            target1.Setup(t => t.CanAuthorize(null)).Returns(false);
            var target2 = new Moq.Mock<ITarget>();
            target2.Setup(t => t.CanAuthorize(null)).Returns(true);

            var result1 = new Moq.Mock<AuthorizeTargetResult>(target1.Object);
            result1.SetupGet<Boolean>(t => t.IsAuthorized).Returns(true);
            var result2 = new Moq.Mock<AuthorizeTargetResult>(target2.Object);
            result2.SetupGet<Boolean>(t => t.IsAuthorized).Returns(false);

            target1.Setup<AuthorizeTargetResult>(t => t.Authorize(null)).Returns(result1.Object);
            target2.Setup<AuthorizeTargetResult>(t => t.Authorize(null)).Returns(result2.Object);

            _action.AddTarget(target1.Object);
            _action.AddTarget(target2.Object);

            Assert.False(_action.Authorize(null).IsAuthorized);
            Assert.AreEqual(1, _action.Authorize(null).AuthorizationFailures.Count());
        }
    }
}