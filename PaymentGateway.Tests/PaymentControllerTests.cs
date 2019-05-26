using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Controllers;
using PaymentGateway.Services;
using System.Threading.Tasks;
using PaymentGateway.Models;
using System;

namespace Tests
{
    public class PaymentControllerTests
    {
        [Test]
        public async Task CheckCard_InvalidModel_BadRequest()
        {
            var bank = new Mock<IBankProvider>();
            var store = new Mock<IPaymentRequestStore>();

            var controller = new PaymentController(bank.Object, store.Object);

            controller.ModelState.AddModelError("an error", "error");

            var response = await controller.CheckCard(new CardDetails());

            Assert.IsInstanceOf<BadRequestObjectResult>(response);
        }

        [Test]
        public async Task CheckCard_ValidCard_Ok_Valid()
        {
            var bank = new Mock<IBankProvider>();
            var store = new Mock<IPaymentRequestStore>();

            bank.Setup(x => x.ValidateCardDetails(It.IsAny<CardDetails>()))
                .Returns(Task.FromResult(true));

            var controller = new PaymentController(bank.Object, store.Object);
            var response = await controller.CheckCard(new CardDetails());

            Assert.IsInstanceOf<OkObjectResult>(response);
            var content = ((OkObjectResult)response).Value as dynamic;

            Assert.True(content.Valid);
        }

        [Test]
        public async Task CheckCard_InvalidCard_Ok_NotValid()
        {
            var bank = new Mock<IBankProvider>();
            var store = new Mock<IPaymentRequestStore>();

            bank.Setup(x => x.ValidateCardDetails(It.IsAny<CardDetails>()))
                .Returns(Task.FromResult(false));

            var controller = new PaymentController(bank.Object, store.Object);
            var response = await controller.CheckCard(new CardDetails());

            Assert.IsInstanceOf<OkObjectResult>(response);
            var content = ((OkObjectResult)response).Value as dynamic;

            Assert.False(content.Valid);
        }


        [Test]
        public async Task CheckCard_PassesCorrectDetailsToBank_NoProcess()
        {
            var bank = new Mock<IBankProvider>();
            var store = new Mock<IPaymentRequestStore>();

            bank.Setup(x => x.ValidateCardDetails(It.IsAny<CardDetails>()))
                .Returns(Task.FromResult(false));

            var controller = new PaymentController(bank.Object, store.Object);

            var details = new CardDetails
            {
                CardholderName = "Robin Wood",
                CardNumber = "1234567890123456",
                Expires = DateTime.Now.AddMonths(5),
                ValidFrom = DateTime.Now.AddMonths(-1),
                CSC = 400
            };
            var response = await controller.CheckCard(details);

            bank.Verify(b => b.ValidateCardDetails(details), Times.Once);
            bank.Verify(b => b.ProcessPayment(It.IsAny<CardDetails>(),
                It.IsAny<TransactionDetails>()), Times.Never);
        }
    }
}