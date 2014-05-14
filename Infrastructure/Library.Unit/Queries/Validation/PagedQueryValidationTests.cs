using Demo.Library.Queries.Validation;
using FluentValidation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Queries.Validation
{
    [TestFixture]
    public class PagedQueryValidationTests
    {
        private PagedQueryValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new PagedQueryValidator();
        }

        [Test]
        public void Should_have_error_when_page_is_invalid()
        {
            _validator.ShouldHaveValidationErrorFor(q => q.Page, 0);
        }

        [Test]
        public void Should_have_error_when_page_size_is_invalid()
        {
            _validator.ShouldHaveValidationErrorFor(q => q.PageSize, 0);
        }

        [Test]
        public void Should_not_have_error_when_page_is_valid()
        {
            _validator.ShouldNotHaveValidationErrorFor(q => q.Page, 1);
        }

        [Test]
        public void Should_not_have_error_when_page_size_is_valid()
        {
            _validator.ShouldNotHaveValidationErrorFor(q => q.PageSize, 1);
        }
    }
}