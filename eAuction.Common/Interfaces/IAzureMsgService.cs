using System.Threading.Tasks;

namespace eAuction.Common.Interfaces
{
    public interface IAzureMsgService
    {
        Task SendMessage(string connectionString, string queueName, string message);

        Task<string> ReceiveMessage(string connectionString, string queueName);
    }
}