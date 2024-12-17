using BuildingBlocks.Common;

namespace BuildingBlocks.Interfaces
{
    public interface IHttpCaller
    {

        Task<BaseHttpResult<T>> GetAsync<T>(HttpCallOptions httpCallOptions) where T : class;
        Task<BaseHttpResult<TResponse>> PostAsync<TRequest, TResponse>(HttpCallOptions<TRequest> httpCallOptions)
           where TRequest : class;

        Task<BaseHttpResult<TResponse>> PutAsync<TRequest, TResponse>(HttpCallOptions<TRequest> httpCallOptions)
           where TRequest : class;
    }
}
