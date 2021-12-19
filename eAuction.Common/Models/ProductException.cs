using System;

namespace eAuction.Common.Models
{
    public class ProductException : Exception
    {
        public ProductException() : base()
        { 
        }

        public ProductException(string message) : base(message)
        {
        }

        public ProductException(string? message, Exception? innerException) : base(message, innerException)
        { 
        }
    }
}