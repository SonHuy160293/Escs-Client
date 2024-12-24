using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using Escs_Client.Interfaces;
using Escs_Client.ViewModels;

namespace Escs_Client.Services
{
    public class KeyService : IKeyService
    {

        private readonly IHttpCaller _httpCaller;
        private readonly ILogger<KeyService> _logger;

        public KeyService(IHttpCaller httpCaller, ILogger<KeyService> logger)
        {
            _httpCaller = httpCaller;
            _logger = logger;
        }

        public async Task<BaseResult> CreateKeyAllowedEndpoint(CreateKeyAllowedEndpointRequest createKeyAllowedEndpointRequest)
        {
            try
            {

                var httpCallOptions = HttpCallOptions<CreateKeyAllowedEndpointRequest>.UnAuthenticated(
                    isSerialized: false,
                    isRetry: false,
                    body: createKeyAllowedEndpointRequest,
                    baseUrl: "http://localhost:5212/api/Services/endpoint/list-allowed",
                    queryStringElements: new List<string>()
                );


                var createUserApiKeyResult = await _httpCaller.PostAsync<CreateKeyAllowedEndpointRequest, bool>(httpCallOptions);
                if (createUserApiKeyResult.Succeeded)
                {
                    return BaseResult.Success();
                }
                return BaseResult.Failure(createUserApiKeyResult.ExceptionError.StatusCode.ToString(), createUserApiKeyResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "CreateKeyAllowedEndpoint occured exception");
                throw;
            }
        }

        public async Task<BaseResult<long>> CreateUserApiKey(CreateUserApiKeyRequest createUserApiKeyRequest)
        {
            try
            {

                var httpCallOptions = HttpCallOptions<CreateUserApiKeyRequest>.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    body: createUserApiKeyRequest,
                    baseUrl: "http://localhost:5212/api/Users/api-key",
                    queryStringElements: new List<string>()
                );


                var createUserApiKeyResult = await _httpCaller.PostAsync<CreateUserApiKeyRequest, long>(httpCallOptions);
                if (createUserApiKeyResult.Succeeded)
                {
                    return BaseResult<long>.Success(createUserApiKeyResult.Data);
                }
                return BaseResult<long>.Failure(createUserApiKeyResult.ExceptionError.StatusCode.ToString(), createUserApiKeyResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "CreateUserApiKey occured exception");
                throw;
            }
        }

        public async Task<BaseResult> CreateUserApiKeyAllowedEndpointTransaction(CreateUserApiKeyAllowedEndpointTransactionRequest createUserApiKeyRequest)
        {
            try
            {

                var httpCallOptions = HttpCallOptions<CreateUserApiKeyAllowedEndpointTransactionRequest>.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    body: createUserApiKeyRequest,
                    baseUrl: "http://localhost:5212/api/Users/api-key-allowed-endpoint",
                    queryStringElements: new List<string>()
                );


                var createUserApiKeyResult = await _httpCaller.PostAsync<CreateUserApiKeyAllowedEndpointTransactionRequest, bool>(httpCallOptions);
                if (createUserApiKeyResult.Succeeded)
                {
                    return BaseResult.Success();
                }
                return BaseResult.Failure(createUserApiKeyResult.ExceptionError.StatusCode.ToString(), createUserApiKeyResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "CreateUserApiKeyAllowedEndpointTransaction occured exception");
                throw;
            }
        }

        public async Task<BaseResult<IEnumerable<UserApiKeyDetailResponse>>> GetKeyDetail(long userId, long serviceId)
        {
            try
            {

                var httpCallOptions = HttpCallOptions.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    baseUrl: $"http://localhost:5212/api/Users/api-key/detail?userId={userId}&serviceId={serviceId}",
                    queryStringElements: new List<string>()
                );


                var endpointResult = await _httpCaller.GetAsync<IEnumerable<UserApiKeyDetailResponse>>(httpCallOptions);
                if (endpointResult.Succeeded)
                {
                    return BaseResult<IEnumerable<UserApiKeyDetailResponse>>.Success(endpointResult.Data);
                }
                return BaseResult<IEnumerable<UserApiKeyDetailResponse>>.Failure(endpointResult.ExceptionError.StatusCode.ToString(), endpointResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "GetKeyDetail occured exception");
                throw;
            }
        }

        public async Task<BaseResult<UserApiKeyDetailResponse>> GetKeyDetailById(long id)
        {
            try
            {

                var httpCallOptions = HttpCallOptions.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    baseUrl: $"http://localhost:5212/api/Users/api-key/{id}/detail",
                    queryStringElements: new List<string>()
                );


                var endpointResult = await _httpCaller.GetAsync<UserApiKeyDetailResponse>(httpCallOptions);
                if (endpointResult.Succeeded)
                {
                    return BaseResult<UserApiKeyDetailResponse>.Success(endpointResult.Data);
                }
                return BaseResult<UserApiKeyDetailResponse>.Failure(endpointResult.ExceptionError.StatusCode.ToString(), endpointResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "GetKeyDetailById occured exception");
                throw;
            }
        }

        public async Task<BaseResult<IEnumerable<ServiceEndpointRegisterByUserResponse>>> GetServiceEndpointRegisterByUser(long userId)
        {
            try
            {

                var httpCallOptions = HttpCallOptions.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    baseUrl: $"http://localhost:5212/api/Users/{userId}/service-registered",
                    queryStringElements: new List<string>()
                );


                var endpointResult = await _httpCaller.GetAsync<IEnumerable<ServiceEndpointRegisterByUserResponse>>(httpCallOptions);
                if (endpointResult.Succeeded)
                {
                    return BaseResult<IEnumerable<ServiceEndpointRegisterByUserResponse>>.Success(endpointResult.Data);
                }
                return BaseResult<IEnumerable<ServiceEndpointRegisterByUserResponse>>.Failure(endpointResult.ExceptionError.StatusCode.ToString(), endpointResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "GetServiceEndpointRegisterByUser occured exception");
                throw;
            }
        }

        public async Task<BaseResult> UpdateUserApiKeyAllowed(UpdateEndpointOfKeyRequest updateEndpointOfKeyRequest)
        {
            try
            {

                var httpCallOptions = HttpCallOptions<UpdateEndpointOfKeyRequest>.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    body: updateEndpointOfKeyRequest,
                    baseUrl: "http://localhost:5212/api/Users/api-key-allowed",
                    queryStringElements: new List<string>()
                );


                var updateUserApiKeyResult = await _httpCaller.PutAsync<UpdateEndpointOfKeyRequest, bool>(httpCallOptions);
                if (updateUserApiKeyResult.Succeeded)
                {
                    return BaseResult.Success();
                }
                return BaseResult.Failure(updateUserApiKeyResult.ExceptionError.StatusCode.ToString(), updateUserApiKeyResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "UpdateUserApiKeyStatus occured exception");
                throw;
            }
        }

        public async Task<BaseResult<long>> UpdateUserApiKeyStatus(UpdateUserApiKeyRequest updateUserApiKeyRequest)
        {
            try
            {

                var httpCallOptions = HttpCallOptions<UpdateUserApiKeyRequest>.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    body: updateUserApiKeyRequest,
                    baseUrl: "http://localhost:5212/api/Users/api-key",
                    queryStringElements: new List<string>()
                );


                var updateUserApiKeyResult = await _httpCaller.PutAsync<UpdateUserApiKeyRequest, long>(httpCallOptions);
                if (updateUserApiKeyResult.Succeeded)
                {
                    return BaseResult<long>.Success(updateUserApiKeyResult.Data);
                }
                return BaseResult<long>.Failure(updateUserApiKeyResult.ExceptionError.StatusCode.ToString(), updateUserApiKeyResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "UpdateUserApiKeyStatus occured exception");
                throw;
            }
        }
    }
}
