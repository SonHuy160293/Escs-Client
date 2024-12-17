using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using Escs_Client.Interfaces;
using Escs_Client.ViewModels;

namespace Escs_Client.Services
{
    public class EndpointService : IEndpointService
    {

        private readonly IHttpCaller _httpCaller;
        private readonly ILogger<EndpointService> _logger;

        public EndpointService(IHttpCaller httpCaller, ILogger<EndpointService> logger)
        {
            _httpCaller = httpCaller;
            _logger = logger;
        }

        public async Task<BaseResult> CreateEndpoint(CreateEndpointRequest createEndpointRequest)
        {
            try
            {
                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions<CreateEndpointRequest>.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    body: createEndpointRequest,
                    baseUrl: "http://localhost:5212/api/Services/endpoint",
                    queryStringElements: new List<string>()
                );

                // Use LoginResponse as the data type
                var createUserApiKeyResult = await _httpCaller.PostAsync<CreateEndpointRequest, long>(httpCallOptions);
                if (createUserApiKeyResult.Succeeded)
                {
                    return BaseResult<long>.Success(createUserApiKeyResult.Data);
                }
                return BaseResult<long>.Failure(createUserApiKeyResult.ExceptionError.StatusCode.ToString(), createUserApiKeyResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "Create endpoint occured exception");
                throw;
            }
        }

        public async Task<BaseResult<ServiceEndpointResponse>> GetEndpointById(long id)
        {
            try
            {
                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    baseUrl: $"http://localhost:5212/api/Services/endpoint/{id}",
                    queryStringElements: new List<string>()
                );

                // Use LoginResponse as the data type
                var endpointResult = await _httpCaller.GetAsync<ServiceEndpointResponse>(httpCallOptions);
                if (endpointResult.Succeeded)
                {
                    return BaseResult<ServiceEndpointResponse>.Success(endpointResult.Data);
                }
                return BaseResult<ServiceEndpointResponse>.Failure(endpointResult.ExceptionError.StatusCode.ToString(), endpointResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "Get key detail occured exception");
                throw;
            }
        }

        public async Task<BaseResult> UpdateEndpoint(UpdateEndpointRequest updateEndpointRequest)
        {
            try
            {
                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions<UpdateEndpointRequest>.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    body: updateEndpointRequest,
                    baseUrl: "http://localhost:5212/api/Services/endpoint",
                    queryStringElements: new List<string>()
                );

                // Use LoginResponse as the data type
                var createUserApiKeyResult = await _httpCaller.PutAsync<UpdateEndpointRequest, long>(httpCallOptions);
                if (createUserApiKeyResult.Succeeded)
                {
                    return BaseResult<long>.Success(createUserApiKeyResult.Data);
                }
                return BaseResult<long>.Failure(createUserApiKeyResult.ExceptionError.StatusCode.ToString(), createUserApiKeyResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "Update endpoint occured exception");
                throw;
            }
        }
    }
}
