using System.Threading.Tasks;

namespace eAuction.Common.Interfaces
{
    public interface IAzureMsgSenderService
    {
        Task SendMessage(string message, string queueName);
    }
}