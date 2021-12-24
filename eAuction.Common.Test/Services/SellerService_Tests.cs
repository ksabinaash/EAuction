using eAuction.Common.Interfaces;
using eAuction.Common.Models;
using eAuction.Common.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace eAuction.Common.Test.Services
{
    [TestClass]
    public class SellerService_Tests
    {
        private SellerService _sellerService;

        private Mock<IProductRepository> _mockProductRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var mockLogging = Mock.Of<ILogger<SellerService>>();

            _mockProductRepository = mockRepository.Create<IProductRepository>();

            _sellerService = new SellerService(mockLogging, _mockProductRepository.Object);

            SellerService_Data.Initialize();
        }

        [TestMethod]
        [DataRow(SellerService_Data._productId1)]        
        public async Task AddProduct_ExpectedBehavior(int productId)
        {
            // Arrange
            var product = SellerService_Data.GetProductById(productId);

            SellerService_Data.Setup(_mockProductRepository, productId);

            // Act
            var result = await _sellerService.AddProduct(
                product);

            // Assert
            Assert.AreEqual(productId.ToString(), result.ProductId, "The product is not matching");
        }

        [TestMethod]
        [DataRow(SellerService_Data._productId2)]
        public async Task AddProduct_WithBidsBehavior(int productId)
        {
            // Arrange
            var product = SellerService_Data.GetProductById(productId);

            SellerService_Data.Setup(_mockProductRepository, productId);

            try
            {
                // Act
                var result = await _sellerService.AddProduct(
                   product);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValidationException), "Exception should be thrown");
            }
        }

        [TestMethod]
        [DataRow(SellerService_Data._productId4)]
        public async Task AddProduct_ExistingIdBehavior(int productId)
        {
            // Arrange
            var product = SellerService_Data.GetProductById(productId);

            SellerService_Data.Setup(_mockProductRepository, productId);

            try
            {
                // Act
                var result = await _sellerService.AddProduct(
                   product);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ProductException), "Exception should be thrown");
            }
        }

        [TestMethod]
        [DataRow(SellerService_Data._productId4)]
        public async Task DeleteProduct_ExpectedBehavior(int productId)
        {
            // Arrange
            SellerService_Data.Setup(_mockProductRepository, productId);

            var result = await _sellerService.DeleteProduct(
                     productId);

            Assert.IsTrue(result, "Delete Product should be true");
        }


        [TestMethod]
        [DataRow(SellerService_Data._productId4)]        
        public async Task DeleteProduct_WithBidsBehavior(int productId)
        {
            // Arrange
            SellerService_Data.Setup(_mockProductRepository, productId);

            try
            {
                // Act
                var result = await _sellerService.DeleteProduct(
                   productId);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValidationException), "Exception should be thrown");
            }
        }

        [TestMethod]
        [DataRow(SellerService_Data._productId1)]
        public async Task DeleteProduct_WrongIdBehavior(int productId)
        {
            // Arrange
            SellerService_Data.Setup(_mockProductRepository, productId);

            try
            {
                // Act
                var result = await _sellerService.DeleteProduct(
                   productId);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ProductException), "Exception should be thrown");
            }
        }

        [TestMethod]
        [DataRow(SellerService_Data._productId4)]
        public async Task GetProductBids_ExpectedBehavior(int productId)
        {
            // Arrange
            var product = SellerService_Data.GetProductById(productId);

            SellerService_Data.Setup(_mockProductRepository, productId);

            // Act
            var result = await _sellerService.GetProductBids(
                productId);

            // Assert
            Assert.AreEqual(product.ProductId, result.ProductId, "The product is not matching");
        }

        [TestMethod]
        public async Task GetProducts_ExpectedBehavior()
        {
            // Arrange
           SellerService_Data.Setup(_mockProductRepository, 1);

            // Act
            var result = await _sellerService.GetProducts();

            // Assert
            Assert.AreEqual(5, result.Count, "The products count are not matching");
        }
    }
}