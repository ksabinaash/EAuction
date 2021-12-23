using eAuction.Common.Interfaces;
using eAuction.Common.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eAuction.Common.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly ILogger<BuyerService> _logger;

        private readonly IProductRepository _repository;

        public BuyerService(ILogger<BuyerService> logger, IProductRepository repository)
        {
            this._logger = logger;

            this._repository = repository;
        }

        public async Task<Buyer> AddBid(Buyer bid)
        {
            _logger.LogInformation("Began" + nameof(AddBid), bid.Email);

            var product = await this.GetProduct(bid.ProductId).ConfigureAwait(false);

            var modifiedProduct = this.UpdateProduct(product, bid);

            var response = await _repository.UpdateProduct(modifiedProduct).ConfigureAwait(false);

            _logger.LogInformation("Ended" + nameof(AddBid));

            var buyer = response?.Buyers.FirstOrDefault(m => m.Email == bid.Email && m.ProductId == bid.ProductId);

            return buyer ?? new Buyer();
        }

        public async Task<Buyer> UpdateBid(int productId, string email, double amount)
        {
            _logger.LogInformation("Began" + nameof(UpdateBid), productId);

            this.ValidateUpdateBid(email, amount);

            var product = await this.GetProduct(productId).ConfigureAwait(false);

            var modifiedProduct = this.UpdateProduct(product, email, amount);

            var response = await _repository.UpdateProduct(modifiedProduct).ConfigureAwait(false);

            _logger.LogInformation("Ended" + nameof(UpdateBid));

            var buyer = response?.Buyers.FirstOrDefault(m => m.Email == email && m.ProductId == productId);

            return buyer ?? new Buyer();
        }

        private async Task<Product> GetProduct(int Id)
        {
            var product = new Product();

            product = await _repository.GetProduct(Id).ConfigureAwait(false);

            if (product.ProductId == null)
            {
                throw new ProductException("Product not available");
            }

            return product;
        }

        private Product UpdateProduct(Product product, Buyer bid)
        {
            var buyers = product.Buyers.Where(b => b.Email == bid.Email);

            if (buyers.Count() > 0)
            {
                throw new BuyerException("Buyer already placed bid for this Product!");
            }

            if (product.BidEndDate < DateTime.Now)
            {
                throw new ValidationException("Bid can't be placed past the Product's Bid end date!");
            }

            if (product.StartingPrice >= bid.BidAmount)
            {
                throw new ValidationException("Bid can't be lower than/equal to the Product's Starting Price!");
            }

            else
            {
                if (product.Buyers == null)
                {
                    product.Buyers = new System.Collections.Generic.List<Buyer>();
                }

                product.Buyers.Add(bid);
            }

            return product;
        }

        private Product UpdateProduct(Product product, string email, double amount)
        {
            var buyers = product.Buyers.Where(b => b.Email == email);

            if (buyers.Count() < 1)
            {
                throw new BuyerException("Bid not available for this Buyer!");
            }

            if (product.BidEndDate < DateTime.Now)
            {
                throw new ValidationException("Bid can't be modified past the Product's Bid end date!");
            }

            if (product.StartingPrice >= amount)
            {
                throw new ValidationException("Bid can't be lower than/equal to the Product's Starting Price!");
            }

            product.Buyers.ForEach(b =>
            {
                if (b.Email == email)
                {
                    b.BidAmount = amount;
                }
            });

            return product;
        }

        private void ValidateUpdateBid(string email, double amount)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            Match match = regex.Match(email);

            if (!match.Success)
            { 
                throw new ValidationException("Buyer's email is not a valid email format!");
            }
            if (amount < 1)
            {
                throw new ValidationException("Bid Amount should be atleast greater that Rs. 1!");
            }
        }
    }
}