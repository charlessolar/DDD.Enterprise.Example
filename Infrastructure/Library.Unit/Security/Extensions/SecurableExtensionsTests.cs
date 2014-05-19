using Demo.Library.Queries;
using Demo.Library.Security;
using Demo.Library.Security.Securables;
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
    public class SecurableExtensionsTests
    {
        [Test]
        public void OfType_QueryTarget_adds_securable()
        {
            var target = new Moq.Mock<QueryTarget>();
            target.Setup(x => x.AddSecurable(Moq.It.IsAny<TypeSecurable>())).Verifiable();
            target.Object.OfType<BasicQuery>();
            target.Verify(x => x.AddSecurable(Moq.It.IsAny<TypeSecurable>()), Moq.Times.Once);
        }

        [Test]
        public void OfType_QueryTarget_continue_with_called()
        {
            var target = new Moq.Mock<QueryTarget>();
            target.Setup(x => x.AddSecurable(Moq.It.IsAny<TypeSecurable>()));

            var actionCalled = false;
            Action<ISecurable> action = s => actionCalled = true;

            target.Object.OfType<BasicQuery>(action);

            Assert.True(actionCalled);
        }
        [Test]
        public void OnType_FunctionTarget_adds_securable()
        {
            var target = new Moq.Mock<FunctionTarget>();
            target.Setup(x => x.AddSecurable(Moq.It.IsAny<TypeSecurable>())).Verifiable();
            target.Object.OnType<Object>();
            target.Verify(x => x.AddSecurable(Moq.It.IsAny<TypeSecurable>()), Moq.Times.Once);
        }
        [Test]
        public void OnType_FunctionTarget_continue_with_called()
        {
            var target = new Moq.Mock<FunctionTarget>();
            target.Setup(x => x.AddSecurable(Moq.It.IsAny<TypeSecurable>()));

            var actionCalled = false;
            Action<ISecurable> action = s => actionCalled = true;

            target.Object.OnType<Object>(action);

            Assert.True(actionCalled);
        }
        [Test]
        public void InNamespace_FunctionTarget_add_securable()
        {
            var target = new Moq.Mock<FunctionTarget>();
            target.Setup(x => x.AddSecurable(Moq.It.IsAny<NamespaceSecurable>())).Verifiable();
            target.Object.InNamespace("test");
            target.Verify(x => x.AddSecurable(Moq.It.IsAny<NamespaceSecurable>()), Moq.Times.Once);
        }
        [Test]
        public void InNamespace_FunctionTarget_continue_with_called()
        {
            var target = new Moq.Mock<FunctionTarget>();
            target.Setup(x => x.AddSecurable(Moq.It.IsAny<NamespaceSecurable>()));

            var actionCalled = false;
            Action<ISecurable> action = s => actionCalled = true;

            target.Object.InNamespace("test", action);

            Assert.True(actionCalled);
        }
        [Test]
        public void InType_PropertyTarget_add_securable()
        {
            var target = new Moq.Mock<PropertyTarget>();
            target.Setup(x => x.AddSecurable(Moq.It.IsAny<PropertySecurable>())).Verifiable();
            target.Object.InType<Object>("test");
            target.Verify(x => x.AddSecurable(Moq.It.IsAny<PropertySecurable>()), Moq.Times.Once);
        }
        [Test]
        public void Intype_PropertyTarget_continue_with_called()
        {
            var target = new Moq.Mock<PropertyTarget>();
            target.Setup(x => x.AddSecurable(Moq.It.IsAny<PropertySecurable>()));

            var actionCalled = false;
            Action<ISecurable> action = s => actionCalled = true;

            target.Object.InType<Object>("test", action);

            Assert.True(actionCalled);
        }
    }
}