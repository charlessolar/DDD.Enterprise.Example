using Demo.Library.Security;
using Demo.Library.Security.Descriptors;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Security.Descriptors
{
    [TestFixture]
    public class DescriptorTests
    {
        private Descriptor _descriptor;

        [SetUp]
        public void Setup()
        {
            _descriptor = new Descriptor();
        }

        [Test]
        public void Add_action_is_added()
        {
            var action = new Moq.Mock<IAction>();

            Assert.True(_descriptor.Actions.Count() == 0);
            _descriptor.AddAction(action.Object);
            Assert.True(_descriptor.Actions.Count() == 1);
        }

        [Test]
        public void Can_authorize()
        {
            var action = new Moq.Mock<IAction>();
            action.Setup<Boolean>(x => x.CanAuthorize(null)).Returns(true);

            _descriptor.AddAction(action.Object);

            Assert.True(_descriptor.CanAuthorize(null));
        }
        [Test]
        public void Can_not_authorize()
        {
            var action = new Moq.Mock<IAction>();
            action.Setup<Boolean>(x => x.CanAuthorize(null)).Returns(false);

            _descriptor.AddAction(action.Object);

            Assert.False(_descriptor.CanAuthorize(null));
        }

        [Test]
        public void One_can_one_cant_authorize()
        {
            var action1 = new Moq.Mock<IAction>();
            action1.Setup<Boolean>(x => x.CanAuthorize(null)).Returns(false);
            var action2 = new Moq.Mock<IAction>();
            action2.Setup<Boolean>(x => x.CanAuthorize(null)).Returns(true);

            _descriptor.AddAction(action1.Object);
            _descriptor.AddAction(action2.Object);

            Assert.True(_descriptor.CanAuthorize(null));
        }

        [Test]
        public void Authorize_authorized()
        {
            var action = new Moq.Mock<IAction>();
            action.Setup<Boolean>(t => t.CanAuthorize(null)).Returns(true);

            var result = new Moq.Mock<AuthorizeActionResult>(action.Object);
            result.SetupGet<Boolean>(t => t.IsAuthorized).Returns(true);
            action.Setup<AuthorizeActionResult>(t => t.Authorize(null)).Returns(result.Object);

            _descriptor.AddAction(action.Object);
            Assert.True(_descriptor.Authorize(null).IsAuthorized);
            Assert.AreEqual(0, _descriptor.Authorize(null).AuthorizationFailures.Count());
        }

        [Test]
        public void Authorize_not_authorized()
        {
            var action = new Moq.Mock<IAction>();
            action.Setup<Boolean>(t => t.CanAuthorize(null)).Returns(true);

            var result = new Moq.Mock<AuthorizeActionResult>(action.Object);
            result.SetupGet<Boolean>(t => t.IsAuthorized).Returns(false);
            action.Setup<AuthorizeActionResult>(t => t.Authorize(null)).Returns(result.Object);

            _descriptor.AddAction(action.Object);
            Assert.False(_descriptor.Authorize(null).IsAuthorized);
            Assert.AreEqual(1, _descriptor.Authorize(null).AuthorizationFailures.Count());
        }

        [Test]
        public void Authorize_both_authorized()
        {
            var action1 = new Moq.Mock<IAction>();
            action1.Setup<Boolean>(t => t.CanAuthorize(null)).Returns(true);
            var action2 = new Moq.Mock<IAction>();
            action2.Setup<Boolean>(t => t.CanAuthorize(null)).Returns(true);


            var result1 = new Moq.Mock<AuthorizeActionResult>(action1.Object);
            result1.SetupGet<Boolean>(t => t.IsAuthorized).Returns(true);
            action1.Setup<AuthorizeActionResult>(t => t.Authorize(null)).Returns(result1.Object);
            var result2 = new Moq.Mock<AuthorizeActionResult>(action2.Object);
            result2.SetupGet<Boolean>(t => t.IsAuthorized).Returns(true);
            action2.Setup<AuthorizeActionResult>(t => t.Authorize(null)).Returns(result1.Object);

            _descriptor.AddAction(action1.Object);
            _descriptor.AddAction(action2.Object);

            Assert.True(_descriptor.Authorize(null).IsAuthorized);
            Assert.AreEqual(0, _descriptor.Authorize(null).AuthorizationFailures.Count());
        }
        [Test]
        public void Authorize_one_authorized()
        {
            var action1 = new Moq.Mock<IAction>();
            action1.Setup<Boolean>(t => t.CanAuthorize(null)).Returns(true);
            var action2 = new Moq.Mock<IAction>();
            action2.Setup<Boolean>(t => t.CanAuthorize(null)).Returns(true);


            var result1 = new Moq.Mock<AuthorizeActionResult>(action1.Object);
            result1.SetupGet<Boolean>(t => t.IsAuthorized).Returns(false);
            action1.Setup<AuthorizeActionResult>(t => t.Authorize(null)).Returns(result1.Object);
            var result2 = new Moq.Mock<AuthorizeActionResult>(action2.Object);
            result2.SetupGet<Boolean>(t => t.IsAuthorized).Returns(true);
            action2.Setup<AuthorizeActionResult>(t => t.Authorize(null)).Returns(result2.Object);

            _descriptor.AddAction(action1.Object);
            _descriptor.AddAction(action2.Object);

            Assert.False(_descriptor.Authorize(null).IsAuthorized);
            Assert.AreEqual(1, _descriptor.Authorize(null).AuthorizationFailures.Count());
        }

        [Test]
        public void Authorize_one_able_authorized()
        {
            var action1 = new Moq.Mock<IAction>();
            action1.Setup<Boolean>(t => t.CanAuthorize(null)).Returns(false);
            var action2 = new Moq.Mock<IAction>();
            action2.Setup<Boolean>(t => t.CanAuthorize(null)).Returns(true);


            var result1 = new Moq.Mock<AuthorizeActionResult>(action1.Object);
            result1.SetupGet<Boolean>(t => t.IsAuthorized).Returns(false);
            action1.Setup<AuthorizeActionResult>(t => t.Authorize(null)).Returns(result1.Object);
            var result2 = new Moq.Mock<AuthorizeActionResult>(action2.Object);
            result2.SetupGet<Boolean>(t => t.IsAuthorized).Returns(true);
            action2.Setup<AuthorizeActionResult>(t => t.Authorize(null)).Returns(result2.Object);

            _descriptor.AddAction(action1.Object);
            _descriptor.AddAction(action2.Object);

            Assert.True(_descriptor.Authorize(null).IsAuthorized);
            Assert.AreEqual(0, _descriptor.Authorize(null).AuthorizationFailures.Count());
        }

        [Test]
        public void Authorize_one_able_not_authorized()
        {
            var action1 = new Moq.Mock<IAction>();
            action1.Setup<Boolean>(t => t.CanAuthorize(null)).Returns(false);
            var action2 = new Moq.Mock<IAction>();
            action2.Setup<Boolean>(t => t.CanAuthorize(null)).Returns(true);


            var result1 = new Moq.Mock<AuthorizeActionResult>(action1.Object);
            result1.SetupGet<Boolean>(t => t.IsAuthorized).Returns(true);
            action1.Setup<AuthorizeActionResult>(t => t.Authorize(null)).Returns(result1.Object);
            var result2 = new Moq.Mock<AuthorizeActionResult>(action2.Object);
            result2.SetupGet<Boolean>(t => t.IsAuthorized).Returns(false);
            action2.Setup<AuthorizeActionResult>(t => t.Authorize(null)).Returns(result2.Object);

            _descriptor.AddAction(action1.Object);
            _descriptor.AddAction(action2.Object);

            Assert.False(_descriptor.Authorize(null).IsAuthorized);
            Assert.AreEqual(1, _descriptor.Authorize(null).AuthorizationFailures.Count());
        }
    }
}