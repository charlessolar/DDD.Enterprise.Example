using Demo.Library.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Exceptions
{
    [TestFixture]
    public class BusinessLogicExceptionTests
    {
        [Test]
        public void Should_have_message()
        {
            var exception = new BusinessLogicException("test");
            Assert.AreEqual("test", exception.Message);
        }

        [Test]
        public void Should_not_have_message()
        {
            var exception = new BusinessLogicException();
            Assert.True(String.IsNullOrEmpty(exception.Message));
        }
    }
}