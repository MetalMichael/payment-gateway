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
        /// <summary>
        /// Check that when the Model State is invalid, a BadRequest response is returned.
        /// No bank or store actions should occur.
        /// </summary>
        [Test]
        public async Task CheckCard_InvalidModel_BadRequest()
        {
            var bank = new Mock<IBankProvider>();
            var store = new Mock<IPaymentRequestStore>();

            var controller = new PaymentController(bank.Object, store.Object);

            controller.ModelState.AddModelError("an error", "error");

            var response = await controller.CheckCard(new CardDetails
            {
                CardholderName = "Robin Wood",
                CardNumber = "1234567890123456",
                Expires = DateTime.Now.AddMonths(5),
                ValidFrom = DateTime.Now.AddMonths(-1),
                CSC = 400
            });

            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            bank.VerifyNoOtherCalls();
            store.VerifyNoOtherCalls();
        }

        /// <summary>
        /// Check that when the bank states that the card is okay, this is returned correctly
        /// </summary>
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

        /// <summary>
        /// Check that when the bank states that the card is invalid, this is returned correctly.
        /// </summary>
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
            var content = ((OkObjectResult)response).Value as CheckCardResult;

            Assert.False(content.Valid);
        }

        /// <summary>
        /// Ensure that the same details are passed to the bank as given to the controller.
        /// Also ensures that no other requests are made to the bank, such as processing a payment.
        /// Also checks that no logs are made to the transaction store.
        /// </summary>
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

            // TODO: This is lazy, should check actual values of the object to ensure they've not changed
            bank.Verify(b => b.ValidateCardDetails(details), Times.Once);
            bank.VerifyNoOtherCalls();
            store.VerifyNoOtherCalls();
        }

        /// <summary>
        /// Check that when the ModelState is invalid, a BadRequest is returned
        /// </summary>
        [Test]
        public async Task ProcessPayment_InvalidModel_BadRequest()
        {
            var bank = new Mock<IBankProvider>();
            var store = new Mock<IPaymentRequestStore>();

            var controller = new PaymentController(bank.Object, store.Object);

            controller.ModelState.AddModelError("an error", "error");

            var response = await controller.ProcessPayment(new PaymentDetails());

            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            bank.VerifyNoOtherCalls();
            store.VerifyNoOtherCalls();
        }

        /// <summary>
        /// Check that when the bank successfully processes the payment, the correct response is returned
        /// </summary>
        [Test]
        public async Task ProcessPayment_ValidAccepted_OkSuccess()
        {
            var bank = new Mock<IBankProvider>();
            var store = new Mock<IPaymentRequestStore>();

            var id = Guid.NewGuid();
            bank.Setup(x => x.ProcessPayment(It.IsAny<CardDetails>(), It.IsAny<TransactionDetails>()))
                .Returns(Task.FromResult(new PaymentResponse
                {
                    Successful = true,
                    TransactionId = id
                }));

            var controller = new PaymentController(bank.Object, store.Object);
            var response = await controller.ProcessPayment(new PaymentDetails
            {
                CardDetails = new CardDetails
                {
                    CardholderName = "平安",
                    CardNumber = "1234567890123456",
                    Expires = DateTime.Now.AddMonths(5),
                    CSC = 400
                },
                TransactionDetails = new TransactionDetails
                {
                    Amount = 123m,
                    Currency = "HKD"
                }
            });

            Assert.IsInstanceOf<OkObjectResult>(response);
            var content = ((OkObjectResult)response).Value as ProcessPaymentResult;

            Assert.IsNotNull(content.PaymentId);
            Assert.IsTrue(content.Success);
            Assert.AreEqual(id, content.TransactionId);
        }

        /// <summary>
        /// Check that when the bank rejects the payment, the correct response is given
        /// </summary>
        [Test]
        public async Task ProcessPayment_ValidRejected_OkFailure()
        {
            var bank = new Mock<IBankProvider>();
            var store = new Mock<IPaymentRequestStore>();

            bank.Setup(x => x.ProcessPayment(It.IsAny<CardDetails>(), It.IsAny<TransactionDetails>()))
                .Returns(Task.FromResult(new PaymentResponse
                {
                    Successful = false
                }));

            var controller = new PaymentController(bank.Object, store.Object);
            var response = await controller.ProcessPayment(new PaymentDetails
            {
                CardDetails = new CardDetails
                {
                    CardholderName = "平安",
                    CardNumber = "1234567890123456",
                    Expires = DateTime.Now.AddMonths(5),
                    CSC = 400
                },
                TransactionDetails = new TransactionDetails
                {
                    Amount = 100000000m,
                    Currency = "GBP"
                }
            });

            Assert.IsInstanceOf<OkObjectResult>(response);
            var content = ((OkObjectResult)response).Value as ProcessPaymentResult;

            Assert.IsNotNull(content.PaymentId);
            Assert.IsFalse(content.Success);
            Assert.IsNotNull(content.TransactionId);
        }

        /// <summary>
        /// Check that when the correct card and transaction details are passed to the bank
        /// </summary>
        [Test]
        public async Task ProcessPayment_PassesCorrectDetailsToBank()
        {
            var bank = new Mock<IBankProvider>();
            var store = new Mock<IPaymentRequestStore>();

            bank.Setup(x => x.ProcessPayment(It.IsAny<CardDetails>(), It.IsAny<TransactionDetails>()))
                .Returns(Task.FromResult(new PaymentResponse
                {
                    Successful = false
                }));

            var controller = new PaymentController(bank.Object, store.Object);

            var card = new CardDetails
            {
                CardholderName = "ひとみ",
                CardNumber = "1234567890123456",
                Expires = DateTime.Now.AddMonths(5),
                CSC = 400
            };

            var transaction = new TransactionDetails
            {
                Amount = 0.12m,
                Currency = "EUR"
            };

            var response = await controller.ProcessPayment(new PaymentDetails
            {
                CardDetails = card,
                TransactionDetails = transaction
            });

            // TODO: This is lazy, should check actual values of the object to ensure they've not changed
            bank.Verify(b => b.ProcessPayment(card, transaction), Times.Once);
            bank.VerifyNoOtherCalls();
        }

        /// <summary>
        /// Check that when the bank successfully processes the payment, the correct response is returned
        /// </summary>
        [Test]
        public async Task ProcessPayment_Success_RecordSuccess()
        {
            var bank = new Mock<IBankProvider>();
            var store = new Mock<IPaymentRequestStore>();

            var id = Guid.NewGuid();
            bank.Setup(x => x.ProcessPayment(It.IsAny<CardDetails>(), It.IsAny<TransactionDetails>()))
                .Returns(Task.FromResult(new PaymentResponse
                {
                    Successful = true,
                    TransactionId = id
                }));
            store.Setup(s => s.LogPaymentRequest(It.IsAny<PaymentRequestLog>()));

            var controller = new PaymentController(bank.Object, store.Object);
            var transaction = new TransactionDetails
            {
                Amount = 123m,
                Currency = "HKD"
            };
            var response = await controller.ProcessPayment(new PaymentDetails
            {
                CardDetails = new CardDetails
                {
                    CardholderName = "伟祺",
                    CardNumber = "1234567890123456",
                    Expires = DateTime.Now.AddMonths(5),
                    CSC = 400
                },
                TransactionDetails = transaction
            });

            store.Verify(s => s.LogPaymentRequest(It.Is<PaymentRequestLog>(log =>
                log.Id != null &&
                log.PaymentResponse.Successful &&
                log.PaymentResponse.TransactionId == id &&
                log.TransactionDetails == transaction && // TODO: Should check values
                log.MaskedCardDetails.MaskedCardNumber == "************3456")), Times.Once);
            store.VerifyNoOtherCalls();
        }

        /// <summary>
        /// Check that when the bank rejects the payment, the correct details are logged
        /// </summary>
        [Test]
        public async Task ProcessPayment_Rejected_RecordFailure()
        {
            var bank = new Mock<IBankProvider>();
            var store = new Mock<IPaymentRequestStore>();

            bank.Setup(x => x.ProcessPayment(It.IsAny<CardDetails>(), It.IsAny<TransactionDetails>()))
                .Returns(Task.FromResult(new PaymentResponse
                {
                    Successful = false
                }));
            store.Setup(s => s.LogPaymentRequest(It.IsAny<PaymentRequestLog>()));

            var controller = new PaymentController(bank.Object, store.Object);
            var transaction = new TransactionDetails
            {
                Amount = 3m,
                Currency = "AUS"
            };
            var response = await controller.ProcessPayment(new PaymentDetails
            {
                CardDetails = new CardDetails
                {
                    CardholderName = "bob",
                    CardNumber = "1234567890123454",
                    Expires = DateTime.Now.AddMonths(5),
                    CSC = 400
                },
                TransactionDetails = transaction
            });

            store.Verify(s => s.LogPaymentRequest(It.Is<PaymentRequestLog>(log =>
                log.Id != null &&
                !log.PaymentResponse.Successful &&
                log.TransactionDetails == transaction && // TODO: Should check values
                log.MaskedCardDetails.MaskedCardNumber == "************3454"
            )), Times.Once);
            store.VerifyNoOtherCalls();
        }

        // TODO: Create find tests
    }
}