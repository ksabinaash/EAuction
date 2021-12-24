using System.Threading.Tasks;

namespace eAuction.Common.Interfaces
{
    public interface IAzureMsgReceiverService
    {
        Task<string> ReceiveMessage(string queueName);
    }
}