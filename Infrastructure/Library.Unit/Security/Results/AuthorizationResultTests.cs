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
    public class AuthorizationResultTests
    {
        private AuthorizationResult _result;

        [SetUp]
        public void Setup()
        {
            _result = new AuthorizationResult();
        }

        [Test]
        public void Process_authorized_has_no_failures()
        {
            var descriptor = new Moq.Mock<AuthorizeDescriptorResult>();
            descriptor.Setup(x => x.IsAuthorized).Returns(true);

            _result.ProcessAuthorizeDescriptorResult(descriptor.Object);

            Assert.AreEqual(0, _result.AuthorizationFailures.Count());
        }

        [Test]
        public void Process_not_authorized_has_one_failure()
        {
            var descriptor = new Moq.Mock<AuthorizeDescriptorResult>();
            descriptor.Setup(x => x.IsAuthorized).Returns(false);

            _result.ProcessAuthorizeDescriptorResult(descriptor.Object);

            Assert.AreEqual(1, _result.AuthorizationFailures.Count());
        }

        [Test]
        public void Process_authorized_is_authorized()
        {
            var descriptor = new Moq.Mock<AuthorizeDescriptorResult>();
            descriptor.Setup(x => x.IsAuthorized).Returns(true);

            _result.ProcessAuthorizeDescriptorResult(descriptor.Object);

            Assert.True(_result.IsAuthorized);
        }

        [Test]
        public void Process_not_authorized_is_not_authorized()
        {
            var descriptor = new Moq.Mock<AuthorizeDescriptorResult>();
            descriptor.Setup(x => x.IsAuthorized).Returns(false);

            _result.ProcessAuthorizeDescriptorResult(descriptor.Object);

            Assert.False(_result.IsAuthorized);
        }

        [Test]
        public void Authorized_has_no_failed_messages()
        {
            var descriptor = new Moq.Mock<AuthorizeDescriptorResult>();
            descriptor.Setup(x => x.IsAuthorized).Returns(true);

            _result.ProcessAuthorizeDescriptorResult(descriptor.Object);

            Assert.AreEqual(0, _result.BuildFailedAuthorizationMessages().Count());
        }

        [Test]
        public void Not_authorized_has_one_failed_messages()
        {
            var descriptor = new Moq.Mock<AuthorizeDescriptorResult>();
            descriptor.Setup(x => x.IsAuthorized).Returns(false);
            descriptor.Setup(x => x.BuildFailedAuthorizationMessages()).Returns(new List<String> { "test" });

            _result.ProcessAuthorizeDescriptorResult(descriptor.Object);

            Assert.AreEqual(1, _result.BuildFailedAuthorizationMessages().Count());
        }
        [Test]
        public void Not_authorized_has_many_failed_messages()
        {
            var descriptor = new Moq.Mock<AuthorizeDescriptorResult>();
            descriptor.Setup(x => x.IsAuthorized).Returns(false);
            descriptor.Setup(x => x.BuildFailedAuthorizationMessages()).Returns(new List<String> { "test", "test2" });

            _result.ProcessAuthorizeDescriptorResult(descriptor.Object);

            Assert.AreEqual(2, _result.BuildFailedAuthorizationMessages().Count());
        }
        [Test]
        public void One_authorized_one_not()
        {
            var descriptor1 = new Moq.Mock<AuthorizeDescriptorResult>();
            descriptor1.Setup(x => x.IsAuthorized).Returns(false);
            var descriptor2 = new Moq.Mock<AuthorizeDescriptorResult>();
            descriptor2.Setup(x => x.IsAuthorized).Returns(true);

            _result.ProcessAuthorizeDescriptorResult(descriptor1.Object);
            _result.ProcessAuthorizeDescriptorResult(descriptor2.Object);

            Assert.False(_result.IsAuthorized);
        }

        [Test]
        public void One_authorized_one_not_one_failure()
        {
            var descriptor1 = new Moq.Mock<AuthorizeDescriptorResult>();
            descriptor1.Setup(x => x.IsAuthorized).Returns(false);
            var descriptor2 = new Moq.Mock<AuthorizeDescriptorResult>();
            descriptor2.Setup(x => x.IsAuthorized).Returns(true);

            _result.ProcessAuthorizeDescriptorResult(descriptor1.Object);
            _result.ProcessAuthorizeDescriptorResult(descriptor2.Object);

            Assert.AreEqual(1, _result.AuthorizationFailures.Count());
        }
        [Test]
        public void One_authorized_one_not_one_message()
        {
            var descriptor1 = new Moq.Mock<AuthorizeDescriptorResult>();
            descriptor1.Setup(x => x.IsAuthorized).Returns(false);
            descriptor1.Setup(x => x.BuildFailedAuthorizationMessages()).Returns(new List<String> { "test" });
            var descriptor2 = new Moq.Mock<AuthorizeDescriptorResult>();
            descriptor2.Setup(x => x.IsAuthorized).Returns(true);

            _result.ProcessAuthorizeDescriptorResult(descriptor1.Object);
            _result.ProcessAuthorizeDescriptorResult(descriptor2.Object);

            Assert.AreEqual(1, _result.BuildFailedAuthorizationMessages().Count());
        }
        [Test]
        public void One_authorized_one_not_two_message()
        {
            var descriptor1 = new Moq.Mock<AuthorizeDescriptorResult>();
            descriptor1.Setup(x => x.IsAuthorized).Returns(false);
            descriptor1.Setup(x => x.BuildFailedAuthorizationMessages()).Returns(new List<String> { "test", "test1" });
            var descriptor2 = new Moq.Mock<AuthorizeDescriptorResult>();
            descriptor2.Setup(x => x.IsAuthorized).Returns(true);

            _result.ProcessAuthorizeDescriptorResult(descriptor1.Object);
            _result.ProcessAuthorizeDescriptorResult(descriptor2.Object);

            Assert.AreEqual(2, _result.BuildFailedAuthorizationMessages().Count());
        }
    }
}