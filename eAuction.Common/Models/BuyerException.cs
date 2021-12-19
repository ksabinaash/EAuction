using System;

namespace eAuction.Common.Models
{
    public class BuyerException : Exception
    {
        public BuyerException() : base()
        { 
        }

        public BuyerException(string message) : base(message)
        {
        }

        public BuyerException(string? message, Exception? innerException) : base(message, innerException)
        { 
        }
    }
}