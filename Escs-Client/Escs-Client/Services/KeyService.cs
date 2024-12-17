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
                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions<CreateKeyAllowedEndpointRequest>.UnAuthenticated(
                    isSerialized: false,
                    isRetry: false,
                    body: createKeyAllowedEndpointRequest,
                    baseUrl: "http://localhost:5212/api/Services/endpoint/list-allowed",
                    queryStringElements: new List<string>()
                );

                // Use LoginResponse as the data type
                var createUserApiKeyResult = await _httpCaller.PostAsync<CreateKeyAllowedEndpointRequest, bool>(httpCallOptions);
                if (createUserApiKeyResult.Succeeded)
                {
                    return BaseResult.Success();
                }
                return BaseResult.Failure(createUserApiKeyResult.ExceptionError.StatusCode.ToString(), createUserApiKeyResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "Create email configuration occured exception");
                throw;
            }
        }

        public async Task<BaseResult<long>> CreateUserApiKey(CreateUserApiKeyRequest createUserApiKeyRequest)
        {
            try
            {
                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions<CreateUserApiKeyRequest>.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    body: createUserApiKeyRequest,
                    baseUrl: "http://localhost:5212/api/Users/api-key",
                    queryStringElements: new List<string>()
                );

                // Use LoginResponse as the data type
                var createUserApiKeyResult = await _httpCaller.PostAsync<CreateUserApiKeyRequest, long>(httpCallOptions);
                if (createUserApiKeyResult.Succeeded)
                {
                    return BaseResult<long>.Success(createUserApiKeyResult.Data);
                }
                return BaseResult<long>.Failure(createUserApiKeyResult.ExceptionError.StatusCode.ToString(), createUserApiKeyResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "Create email configuration occured exception");
                throw;
            }
        }

        public async Task<BaseResult<IEnumerable<UserApiKeyDetailResponse>>> GetKeyDetail(long userId, long serviceId)
        {
            try
            {
                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    baseUrl: $"http://localhost:5212/api/Users/api-key/detail?userId={userId}&serviceId={serviceId}",
                    queryStringElements: new List<string>()
                );

                // Use LoginResponse as the data type
                var endpointResult = await _httpCaller.GetAsync<IEnumerable<UserApiKeyDetailResponse>>(httpCallOptions);
                if (endpointResult.Succeeded)
                {
                    return BaseResult<IEnumerable<UserApiKeyDetailResponse>>.Success(endpointResult.Data);
                }
                return BaseResult<IEnumerable<UserApiKeyDetailResponse>>.Failure(endpointResult.ExceptionError.StatusCode.ToString(), endpointResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "Get key detail occured exception");
                throw;
            }
        }

        public async Task<BaseResult<IEnumerable<ServiceEndpointRegisterByUserResponse>>> GetServiceEndpointRegisterByUser(long userId)
        {
            try
            {
                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    baseUrl: $"http://localhost:5212/api/Users/{userId}/service-registered",
                    queryStringElements: new List<string>()
                );

                // Use LoginResponse as the data type
                var endpointResult = await _httpCaller.GetAsync<IEnumerable<ServiceEndpointRegisterByUserResponse>>(httpCallOptions);
                if (endpointResult.Succeeded)
                {
                    return BaseResult<IEnumerable<ServiceEndpointRegisterByUserResponse>>.Success(endpointResult.Data);
                }
                return BaseResult<IEnumerable<ServiceEndpointRegisterByUserResponse>>.Failure(endpointResult.ExceptionError.StatusCode.ToString(), endpointResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "Get key detail occured exception");
                throw;
            }
        }

        public async Task<BaseResult<long>> UpdateUserApiKeyStatus(UpdateUserApiKeyRequest updateUserApiKeyRequest)
        {
            try
            {
                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions<UpdateUserApiKeyRequest>.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    body: updateUserApiKeyRequest,
                    baseUrl: "http://localhost:5212/api/Users/api-key",
                    queryStringElements: new List<string>()
                );

                // Use LoginResponse as the data type
                var updateUserApiKeyResult = await _httpCaller.PutAsync<UpdateUserApiKeyRequest, long>(httpCallOptions);
                if (updateUserApiKeyResult.Succeeded)
                {
                    return BaseResult<long>.Success(updateUserApiKeyResult.Data);
                }
                return BaseResult<long>.Failure(updateUserApiKeyResult.ExceptionError.StatusCode.ToString(), updateUserApiKeyResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "Update endpoint occured exception");
                throw;
            }
        }
    }
}
