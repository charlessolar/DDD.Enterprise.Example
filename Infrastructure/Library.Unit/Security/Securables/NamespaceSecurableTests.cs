using Demo.Library.Security.Securables;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Security.Securables
{
    [TestFixture]
    public class NamespaceSecurableTests
    {
        [Test]
        public void Can_authorize()
        {
            var securable = new NamespaceSecurable("test");
            var instance = new Moq.Mock<Object>();
            instance.Setup(x => x.GetType().Namespace).Returns("test");

            Assert.True(securable.CanAuthorize(instance.Object));
        }
    }
}