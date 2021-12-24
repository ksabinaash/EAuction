using eAuction.Common.Interfaces;
using eAuction.Common.Models;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace eAuction.Common.Test.Services
{
    public static class SellerService_Data
    {
        private static bool _isInitialized = false;

        private static List<Product> _products = new List<Product>();

        private static List<Product> _sellers = new List<Product>();

        private static List<Buyer> _bids;

        public const int _productId1 = 6;

        public const int _productId2 = 7;

        public const int _productId3 = 3;

        public const int _productId4 = 4;

        public const int _productId_Wrong = 41;

        public const string _emailId1 = "abc@gmail.com";

        public const string _emailId2 = "def@gmail.com";

        public const string _emailId3 = "sundharponting@example.com";

        public const string _emailId_NotAvailable = "abcdef@gmail.com";

        public const string _emailId_WrongPattern = "abcgmail.com";

        public const double _bidAmount1 = 12000;

        public const double _bidAmount2_Low = 100;

        public const double _bidAmount3_Wrong = 0;

        public static void Initialize()
        {
            if (!_isInitialized)
            {
                var productsJson = ReadJsonFromFile("Products.json");

                _products = JsonConvert.DeserializeObject<List<Product>>(productsJson);

                var sellersJson = ReadJsonFromFile("Sellers.json");

                _sellers = JsonConvert.DeserializeObject<List<Product>>(sellersJson);

                GetBids();
            }

            _isInitialized = true;
        }

        public static Buyer GetBidById(int productId, string email)
        {
            return _bids.FirstOrDefault(m => m.ProductId == productId && m.Email == email);
        }

        public static Product GetProductById(int productId)
        {
            return _sellers.FirstOrDefault(m => m.ProductId == productId.ToString());
        }

        private static void GetBids()
        {
            _bids = new List<Buyer>();

            _bids.Add(new Buyer() { ProductId = 1, BidAmount = 1500, Address = "Australia", City = "Sam", Email = "abc@gmail.com", FirstName = "Ramanathan", LastName = "Moorthy", Phone = "9876543210", Pin = "654291", State = "tn" });

            _bids.Add(new Buyer() { ProductId = 2, BidAmount = 2500, Address = "Australia", City = "Sam", Email = "def@gmail.com", FirstName = "Santhosh", LastName = "Sivam", Phone = "9876543210", Pin = "654291", State = "tn" });

            _bids.Add(new Buyer() { ProductId = 3, BidAmount = 550000, Address = "Australia", City = "Sam", Email = "sundharponting@example.com", FirstName = "Murugan", LastName = "Ashwin", Phone = "9876543210", Pin = "654291", State = "tn" });
        }

        private static string ReadJsonFromFile(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Mocks", fileName);

            var jsonData = File.ReadAllText(filePath);

            return jsonData;
        }

        internal static void Setup(Mock<IProductRepository> mockProductRepository, int productId)
        {
            mockProductRepository.Setup(pr => pr.GetProducts()).ReturnsAsync(_products);

            var product = _products.FirstOrDefault(p => p.ProductId == productId.ToString()) ?? new Product();
            
            var sellerProduct = _sellers.FirstOrDefault(p => p.ProductId == productId.ToString());

            mockProductRepository.Setup(pr => pr.GetProduct(It.IsAny<int>())).ReturnsAsync(product);

            mockProductRepository.Setup(pr => pr.AddProduct(It.IsAny<Product>())).ReturnsAsync(sellerProduct);

            mockProductRepository.Setup(pr => pr.UpdateProduct(It.IsAny<Product>())).ReturnsAsync(sellerProduct);

            mockProductRepository.Setup(pr => pr.DeleteProduct(It.IsAny<Product>())).ReturnsAsync(true);
        }
    }
}