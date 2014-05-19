using Demo.Library.Security;
using Demo.Library.Security.Actions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Security.Extensions
{
    [TestFixture]
    public class ActionExtensionsTests
    {
        private Moq.Mock<IDescriptor> _descriptor;

        [SetUp]
        public void Setup()
        {
            _descriptor = new Moq.Mock<IDescriptor>();
        }

        [Test]
        public void Executing_should_add_action()
        {
            _descriptor.Setup(x => x.AddAction(Moq.It.IsAny<ExecutingAction>())).Verifiable();
            _descriptor.Object.Executing();
            _descriptor.Verify(x => x.AddAction(Moq.It.IsAny<ExecutingAction>()), Moq.Times.Once);
        }

        [Test]
        public void Reading_should_add_action()
        {
            _descriptor.Setup(x => x.AddAction(Moq.It.IsAny<ReadingAction>())).Verifiable();
            _descriptor.Object.Reading();
            _descriptor.Verify(x => x.AddAction(Moq.It.IsAny<ReadingAction>()), Moq.Times.Once);
        }
        [Test]
        public void Receiving_should_add_action()
        {
            _descriptor.Setup(x => x.AddAction(Moq.It.IsAny<ReceivingAction>())).Verifiable();
            _descriptor.Object.Receiving();
            _descriptor.Verify(x => x.AddAction(Moq.It.IsAny<ReceivingAction>()), Moq.Times.Once);
        }
        [Test]
        public void Requesting_should_add_action()
        {
            _descriptor.Setup(x => x.AddAction(Moq.It.IsAny<RequestingAction>())).Verifiable();
            _descriptor.Object.Requesting();
            _descriptor.Verify(x => x.AddAction(Moq.It.IsAny<RequestingAction>()), Moq.Times.Once);
        }
        [Test]
        public void Writing_should_add_action()
        {
            _descriptor.Setup(x => x.AddAction(Moq.It.IsAny<WritingAction>())).Verifiable();
            _descriptor.Object.Writing();
            _descriptor.Verify(x => x.AddAction(Moq.It.IsAny<WritingAction>()), Moq.Times.Once);
        }
    }
}