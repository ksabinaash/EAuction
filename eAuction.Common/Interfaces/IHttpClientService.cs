using System.Collections.Generic;
using System.Threading.Tasks;

namespace eAuction.Common.Interfaces
{
    public interface IHttpClientService
    {
        Task<T> ExecuteGet<T>(string url);

        Task<T> ExecutePost<T>(string url, T item);
    }
}
