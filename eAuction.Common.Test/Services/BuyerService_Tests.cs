using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using eAuction.Common.Interfaces;
using eAuction.Common.Services;
using eAuction.Common.Models;

namespace eAuction.Common.Test.Services
{
    [TestClass]
    public class BuyerService_Tests
    {
        private BuyerService _buyerService;

        private Mock<IProductRepository> _mockProductRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var mockLogging = Mock.Of<ILogger<BuyerService>>();

            _mockProductRepository = mockRepository.Create<IProductRepository>();

            _buyerService = new BuyerService(mockLogging, _mockProductRepository.Object);

            BuyerService_Data.Initialize();
        }

        [TestMethod]
        [DataRow(BuyerService_Data._productId1, BuyerService_Data._emailId1)]
        public async Task AddBid_ExpectedBehavior(int productId, string email)
        {
            // Arrange
            var bid = BuyerService_Data.GetBidById(productId, email);

            BuyerService_Data.Setup(_mockProductRepository, bid);

            // Act
            var result = await _buyerService.AddBid(
                bid);

            // Assert
            Assert.AreEqual(bid.Email,  result.Email, "The bid email is not matching");
            
        }

        [TestMethod]
        [DataRow(BuyerService_Data._productId2, BuyerService_Data._emailId2)]
        public async Task AddBid_AmountException(int productId, string email)
        {
            // Arrange
            var bid = BuyerService_Data.GetBidById(productId, email);

            BuyerService_Data.Setup(_mockProductRepository, bid);

            // Act
            try
            {
                var result = await _buyerService.AddBid(
                    bid);
            }
            // Assert
            catch (Exception ex)
            {

                Assert.IsInstanceOfType(ex, typeof(ValidationException), "Exception should be thrown");
            }
        }

        [TestMethod]
        [DataRow(BuyerService_Data._productId2, BuyerService_Data._emailId2)]
        public async Task AddBid_ProductException(int productId, string email)
        {
            // Arrange
            var bid = BuyerService_Data.GetBidById(productId, email);

            BuyerService_Data.Setup(_mockProductRepository, bid);

            // Act
            try
            {
                var result = await _buyerService.AddBid(
                    bid);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValidationException), "Exception should be thrown");
            }
        }

        [TestMethod]
        [DataRow(BuyerService_Data._productId3, BuyerService_Data._emailId3)]
        public async Task AddBid_EmailException(int productId, string email)
        {
            // Arrange
            var bid = BuyerService_Data.GetBidById(productId, email);

            BuyerService_Data.Setup(_mockProductRepository, bid);

            // Act
            try
            {
                var result = await _buyerService.AddBid(
                    bid);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(BuyerException), "Exception should be thrown");
            }
        }

        [TestMethod]
        [DataRow(BuyerService_Data._productId3, BuyerService_Data._emailId3, BuyerService_Data._bidAmount1)]
        public async Task UpdateBid_ExpectedBehavior(int productId, string emailId, double amount)
        {
            // Arrange
            var bid = BuyerService_Data.GetBidById(productId, emailId);

            BuyerService_Data.Setup(_mockProductRepository, bid);

            // Act
            var result = await _buyerService.UpdateBid(
                productId,
                emailId,
                amount);

            // Assert
            Assert.AreEqual(productId, result.ProductId, "The product id should be same");
        }


        [TestMethod]
        [DataRow(BuyerService_Data._productId3, BuyerService_Data._emailId3, BuyerService_Data._bidAmount2_Low)]
        public async Task UpdateBid_AmountException(int productId, string emailId, double amount)
        {
            // Arrange
            var bid = BuyerService_Data.GetBidById(productId, emailId);

            BuyerService_Data.Setup(_mockProductRepository, bid);

            try
            {
                // Act
                var result = await _buyerService.UpdateBid(
                    productId,
                    emailId,
                    amount);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValidationException), "Exception should be thrown");
            }
        }


        [TestMethod]
        [DataRow(BuyerService_Data._productId3, BuyerService_Data._emailId3, BuyerService_Data._bidAmount3_Wrong)]
        public async Task UpdateBid_AmountLowException(int productId, string emailId, double amount)
        {
            // Arrange
            var bid = BuyerService_Data.GetBidById(productId, emailId);

            BuyerService_Data.Setup(_mockProductRepository, bid);

            try
            {
                // Act
                var result = await _buyerService.UpdateBid(
                    productId,
                    emailId,
                    amount);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValidationException), "Exception should be thrown");
            }
        }

        [TestMethod]
        [DataRow(BuyerService_Data._productId3, BuyerService_Data._emailId3, BuyerService_Data._bidAmount1)]
        public async Task UpdateBid_EmailException(int productId, string emailId, double amount)
        {
            // Arrange
            var bid = BuyerService_Data.GetBidById(productId, emailId);

            BuyerService_Data.Setup(_mockProductRepository, bid);

            try
            {
                // Act
                var result = await _buyerService.UpdateBid(
                    productId,
                    BuyerService_Data._emailId_WrongPattern,
                    amount);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValidationException), "Exception should be thrown");
            }
        }
    }
}