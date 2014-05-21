using Demo.Library.Dynamics;
using Demo.Library.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Dynamics
{
    public class TestPoco
    {
        public String Test { get; set; }
    }

    [TestFixture]
    public class DynamicTypeBuilderTests
    {
        [Test]
        public void Get_dynamic_type_empty_fields()
        {
            var type = DynamicTypeBuilder.GetDynamicType(new Dictionary<string, Type>());
            Assert.AreEqual(0, type.GetProperties().Count());
        }

        [Test]
        public void Get_dynamic_type_one_field()
        {
            var type = DynamicTypeBuilder.GetDynamicType(new Dictionary<string, Type> { { "test", typeof(Int32) } });
            Assert.AreEqual(1, type.GetProperties().Count());
        }

        [Test]
        public void Select_partial_has_field()
        {
            var data = new List<TestPoco> { new TestPoco { Test = "test" } };
            var projection = data.AsQueryable().SelectPartial("Test").First();
            Assert.That(() => projection.Test, Throws.Nothing);
        }
        [Test]
        public void Select_partial_not_has_field()
        {
            var data = new List<TestPoco> { new TestPoco { Test = "test" } };
            var projection = data.AsQueryable().SelectPartial("Not").First();
            Assert.That(() => projection.Not, Throws.Exception);
        }
        [Test]
        public void Select_partial_mismatch_case()
        {
            var data = new List<TestPoco> { new TestPoco { Test = "test" } };
            var projection = data.AsQueryable().SelectPartial("test").First();
            Assert.That(() => projection.test, Throws.Exception);
        }
        [Test]
        public void Select_partial_access_wrong_property()
        {
            var data = new List<TestPoco> { new TestPoco { Test = "test" } };
            var projection = data.AsQueryable().SelectPartial("Test").First();
            Assert.That(() => projection.Not, Throws.Exception);
        }

        [Test]
        public void To_partial_has_field()
        {
            var data = new TestPoco { Test = "test" };
            var projection = data.ToPartial("Test");
            Assert.That(() => projection.Test, Throws.Nothing);
        }
        [Test]
        public void To_partial_not_has_field()
        {
            var data = new TestPoco { Test = "test" };
            var projection = data.ToPartial("Not");
            Assert.That(() => projection.Not, Throws.Exception);
        }
        [Test]
        public void To_partial_mismatch_case()
        {
            var data = new TestPoco { Test = "test" };
            var projection = data.ToPartial("test");
            Assert.That(() => projection.test, Throws.Exception);
        }
        [Test]
        public void To_partial_access_wrong_property()
        {
            var data = new TestPoco { Test = "test" };
            var projection = data.ToPartial("Test");
            Assert.That(() => projection.Not, Throws.Exception);
        }
    }
}