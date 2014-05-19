using Demo.Library.Security;
using Demo.Library.Security.Managers;
using NUnit.Framework;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Security.Managers
{
    [TestFixture]
    public class ManagerTests
    {
        [Test]
        public void Authorize_no_descriptors()
        {
            var container = new Moq.Mock<IContainer>();
            container.Setup(x => x.GetAllInstances<IDescriptor>()).Returns(new List<IDescriptor>());

            var manager = new Manager(container.Object);

            Assert.True(manager.Authorize(null).IsAuthorized);
        }
        [Test]
        public void Authorize_with_descriptors_authorized()
        {
            var container = new Moq.Mock<IContainer>();
            var descriptor = new Moq.Mock<IDescriptor>();
            var result = new Moq.Mock<AuthorizeDescriptorResult>();

            result.Setup(x => x.IsAuthorized).Returns(true);
            descriptor.Setup(d => d.CanAuthorize(null)).Returns(true);
            descriptor.Setup(d => d.Authorize(null)).Returns(result.Object);

            container.Setup(x => x.GetAllInstances<IDescriptor>()).Returns(new List<IDescriptor> { descriptor.Object });

            var manager = new Manager(container.Object);

            Assert.True(manager.Authorize(null).IsAuthorized);
        }
        [Test]
        public void Authorize_with_descriptors_not_authorized()
        {
            var container = new Moq.Mock<IContainer>();
            var descriptor = new Moq.Mock<IDescriptor>();
            var result = new Moq.Mock<AuthorizeDescriptorResult>();

            result.Setup(x => x.IsAuthorized).Returns(false);
            descriptor.Setup(d => d.CanAuthorize(null)).Returns(true);
            descriptor.Setup(d => d.Authorize(null)).Returns(result.Object);

            container.Setup(x => x.GetAllInstances<IDescriptor>()).Returns(new List<IDescriptor> { descriptor.Object });

            var manager = new Manager(container.Object);

            Assert.False(manager.Authorize(null).IsAuthorized);
        }

        [Test]
        public void Authorize_with_descriptors_cant_authorize_not_authorized()
        {
            var container = new Moq.Mock<IContainer>();
            var descriptor = new Moq.Mock<IDescriptor>();
            var result = new Moq.Mock<AuthorizeDescriptorResult>();

            result.Setup(x => x.IsAuthorized).Returns(false);
            descriptor.Setup(d => d.CanAuthorize(null)).Returns(false);
            descriptor.Setup(d => d.Authorize(null)).Returns(result.Object);

            container.Setup(x => x.GetAllInstances<IDescriptor>()).Returns(new List<IDescriptor> { descriptor.Object });

            var manager = new Manager(container.Object);

            Assert.True(manager.Authorize(null).IsAuthorized);
        }
        [Test]
        public void Authorize_with_descriptors_cant_authorize_authorized()
        {
            var container = new Moq.Mock<IContainer>();
            var descriptor = new Moq.Mock<IDescriptor>();
            var result = new Moq.Mock<AuthorizeDescriptorResult>();

            result.Setup(x => x.IsAuthorized).Returns(true);
            descriptor.Setup(d => d.CanAuthorize(null)).Returns(false);
            descriptor.Setup(d => d.Authorize(null)).Returns(result.Object);

            container.Setup(x => x.GetAllInstances<IDescriptor>()).Returns(new List<IDescriptor> { descriptor.Object });

            var manager = new Manager(container.Object);

            Assert.True(manager.Authorize(null).IsAuthorized);
        }
    }
}