using Demo.Library.Dynamics;
using Demo.Library.Security;
using Demo.Library.Security.Hows;
using NUnit.Framework;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Security
{
    public class ConcreteWhat : IWhat
    {
        public String Description { get { return "test"; } set { } }

        public void AddWhere<T>(IWhere<T> where)
        {
        }

        public Boolean Authorized()
        {
            return true;
        }
    }

    [TestFixture]
    public class ReadingTest
    {
        private Reading _reading;

        [SetUp]
        public void Setup()
        {
            _reading = new Reading(ObjectFactory.Container);
        }

        [Test]
        public void Intercept_call()
        {
            var fields = new Dictionary<String, Type> { { "test", typeof(Int32) } };
            var type = DynamicTypeBuilder.GetDynamicSecuredType<ConcreteWhat>(fields);

            dynamic obj = (ConcreteWhat)Activator.CreateInstance(type);
            var o = obj.test;
            Assert.AreEqual(0, o);
        }
    }
}