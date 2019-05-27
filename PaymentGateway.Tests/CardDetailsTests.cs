using NUnit.Framework;
using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PaymentGateway.Tests
{
    public class CardDetailsTests
    {
        public static IList<ValidationResult> Validate(object model)
        {
            // https://visualstudiomagazine.com/articles/2015/06/19/tdd-asp-net-mvc-part-4-unit-testing.aspx
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, results, true);
            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);
            return results;
        }

        [Test]
        public void CardDetails_Empty_Invalid()
        {
            var model = new CardDetails();

            var results = Validate(model);
            Assert.AreEqual(4, results.Count);
        }

        [Test]
        public void CardDetails_ShortCardNumber_Invalid()
        {
            var model = new CardDetails
            {
                CardholderName = "bob",
                CardNumber = "123456789012345",
                CSC = "123",
                Expires = DateTime.Now
            };

            var results = Validate(model);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("CardNumber", results.First().MemberNames.First());
        }

        [Test]
        public void CardDetails_LongCardNumber_Invalid()
        {
            var model = new CardDetails
            {
                CardholderName = "bob",
                CardNumber = "12345678901234567",
                CSC = "123",
                Expires = DateTime.Now
            };

            var results = Validate(model);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("CardNumber", results.First().MemberNames.First());
        }

        [Test]
        public void CardDetails_LetterInCardNumber_Invalid()
        {
            var model = new CardDetails
            {
                CardholderName = "bob",
                CardNumber = "123456789012345a",
                CSC = "123",
                Expires = DateTime.Now
            };

            var results = Validate(model);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("CardNumber", results.First().MemberNames.First());
        }

        [Test]
        public void CardDetails_ExpiresPast_Invalid()
        {
            var model = new CardDetails
            {
                CardholderName = "bob",
                CardNumber = "1234567890123456",
                CSC = "123",
                Expires = DateTime.Now.AddMonths(-1)
            };

            var results = Validate(model);
            Assert.AreEqual(1, results.Count);
        }

        [Test]
        public void CardDetails_ValidFromFuture_Invalid()
        {
            var model = new CardDetails
            {
                CardholderName = "bob",
                CardNumber = "1234567890123456",
                CSC = "123",
                Expires = DateTime.Now,
                ValidFrom = DateTime.Now.AddMonths(1)
            };

            var results = Validate(model);
            Assert.AreEqual(1, results.Count);
        }

        [Test]
        public void CardDetails_CSCLong_Invalid()
        {
            var model = new CardDetails
            {
                CardholderName = "bob",
                CardNumber = "1234567890123456",
                CSC = "1234",
                Expires = DateTime.Now
            };

            var results = Validate(model);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("CSC", results.First().MemberNames.First());
        }

        [Test]
        public void CardDetails_CSCShort_Invalid()
        {
            var model = new CardDetails
            {
                CardholderName = "bob",
                CardNumber = "1234567890123456",
                CSC = "12",
                Expires = DateTime.Now
            };

            var results = Validate(model);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("CSC", results.First().MemberNames.First());
        }

        [Test]
        public void CardDetails_CSCLetters_Invalid()
        {
            var model = new CardDetails
            {
                CardholderName = "bob",
                CardNumber = "1234567890123456",
                CSC = "abc",
                Expires = DateTime.Now
            };

            var results = Validate(model);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("CSC", results.First().MemberNames.First());
        }

        [Test]
        public void CardDetails_Valid()
        {
            var model = new CardDetails
            {
                CardholderName = "antony long and his super short name",
                CardNumber = "1234567890123456",
                CSC = "123",
                Expires = DateTime.Now.AddYears(5)
            };

            var results = Validate(model);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CardDetails_ValidWithExpires()
        {
            var model = new CardDetails
            {
                CardholderName = "김정은",
                CardNumber = "1234567890123456",
                CSC = "123",
                Expires = DateTime.Now.AddYears(100),
                ValidFrom = DateTime.Now.AddYears(-10)
            };

            var results = Validate(model);
            Assert.AreEqual(0, results.Count);
        }
    }
}
