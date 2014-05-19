using Demo.Library.Security;
using Demo.Library.Security.Actors;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Security.Extensions
{
    [TestFixture]
    public class ActorExtensionsTests
    {
        private Moq.Mock<ISecurable> _securable;

        [SetUp]
        public void Setup()
        {
            _securable = new Moq.Mock<ISecurable>();
        }

        [Test]
        public void Users_should_add_actor()
        {
            _securable.Setup(x => x.AddActor(Moq.It.IsAny<UserActor>())).Verifiable();
            _securable.Object.Users();
            _securable.Verify(x => x.AddActor(Moq.It.IsAny<UserActor>()), Moq.Times.Once);
        }
    }
}