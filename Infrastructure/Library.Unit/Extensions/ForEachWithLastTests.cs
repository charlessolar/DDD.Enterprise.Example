using Demo.Library.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Demo.Library.Unit.Extensions
{
    [TestFixture]
    public class ForEachWithLastTests
    {
        private List<Int32> data;

        [SetUp]
        public void Setup()
        {
            data = new List<Int32> { 0, 1, 2 };
        }

        [Test]
        public void Should_be_last()
        {
            data.ForEachWithLast((i, last) =>
                {
                    if (last)
                        Assert.AreEqual(2, i);
                });
        }

        [Test]
        public void Should_not_be_last()
        {
            data.ForEachWithLast((i, last) =>
                {
                    if (!last)
                        Assert.AreNotEqual(2, i);
                });
        }

        [Test]
        public void Empty_list_should_not_execute()
        {
            new List<Int32>().ForEachWithLast((i, last) =>
                {
                    Assert.Fail();
                });
        }
    }
}