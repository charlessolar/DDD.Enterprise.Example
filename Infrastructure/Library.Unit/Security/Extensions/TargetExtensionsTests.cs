using Demo.Library.Security;
using Demo.Library.Security.Actions;
using Demo.Library.Security.Targets;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Security.Extensions
{
    [TestFixture]
    public class TargetExtensionsTests
    {
        [Test]
        public void Receiving_adds_query_target()
        {
            var action = new Moq.Mock<ReceivingAction>();
            action.Setup(x => x.AddTarget(Moq.It.IsAny<QueryTarget>())).Verifiable();
            action.Object.Queries();
            action.Verify(x => x.AddTarget(Moq.It.IsAny<QueryTarget>()), Moq.Times.Once);
        }
        [Test]
        public void Executing_adds_function_target()
        {
            var action = new Moq.Mock<ExecutingAction>();
            action.Setup(x => x.AddTarget(Moq.It.IsAny<FunctionTarget>())).Verifiable();
            action.Object.Functions();
            action.Verify(x => x.AddTarget(Moq.It.IsAny<FunctionTarget>()), Moq.Times.Once);
        }
        [Test]
        public void Reading_adds_properties_target()
        {
            var action = new Moq.Mock<ReadingAction>();
            action.Setup(x => x.AddTarget(Moq.It.IsAny<PropertyTarget>())).Verifiable();
            action.Object.Properties();
            action.Verify(x => x.AddTarget(Moq.It.IsAny<PropertyTarget>()), Moq.Times.Once);
        }
        [Test]
        public void Writing_adds_properties_target()
        {
            var action = new Moq.Mock<WritingAction>();
            action.Setup(x => x.AddTarget(Moq.It.IsAny<PropertyTarget>())).Verifiable();
            action.Object.Properties();
            action.Verify(x => x.AddTarget(Moq.It.IsAny<PropertyTarget>()), Moq.Times.Once);
        }
    }
}