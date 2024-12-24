using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using Escs_Client.Interfaces;
using Escs_Client.ViewModels;

namespace Escs_Client.Services
{
    public class EmailService : IEmailService
    {

        private readonly IHttpCaller _httpCaller;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;

        public EmailService(IHttpCaller httpCaller, IHttpContextAccessor httpContextAccessor, ILogger<EmailService> logger, IConfiguration configuration)
        {
            _httpCaller = httpCaller;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<BaseResult> CreateEmailConfiguration(EmailConfigurationViewModel emailConfigurationViewModel)
        {
            try
            {
                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions<EmailConfigurationViewModel>.UnAuthenticated(
                    isSerialized: false,
                    isRetry: false,
                    body: emailConfigurationViewModel,
                    baseUrl: "http://localhost:5212/api/Users/email-config",
                    queryStringElements: new List<string>()
                );

                // Use LoginResponse as the data type
                var loginResult = await _httpCaller.PostAsync<EmailConfigurationViewModel, bool>(httpCallOptions);
                if (loginResult.Succeeded)
                {
                    return BaseResult.Success();
                }
                return BaseResult.Failure(loginResult.ExceptionError.StatusCode.ToString(), loginResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "Create email configuration occured exception");
                throw;
            }
        }

        public async Task<BaseResult<EmailConfigurationDetailResponse>> GetEmailConfigurationById(long id)
        {
            try
            {
                var key = _configuration["ApiKey:Identity"];

                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions.Authenticated(
                    isSerialized: true,
                    isRetry: false,
                    baseUrl: $"http://localhost:5212/api/Users/email-config/detail/{id}",
                    queryStringElements: new List<string>(),
                     "X-Api-Key", key
                );

                // Use LoginResponse as the data type
                var loginResult = await _httpCaller.GetAsync<EmailConfigurationDetailResponse>(httpCallOptions);
                if (loginResult.Succeeded)
                {
                    return BaseResult<EmailConfigurationDetailResponse>.Success(loginResult.Data);
                }
                return BaseResult<EmailConfigurationDetailResponse>.Failure(loginResult.ExceptionError.StatusCode.ToString(), loginResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "Get email configuration occured exception");
                throw;
            }
        }

        public async Task<BaseResult<IEnumerable<EmailConfigurationResponse>>> GetEmailConfigurationByUserId(long id)
        {
            try
            {
                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    baseUrl: $"http://localhost:5212/api/Users/email-config/{id}",
                    queryStringElements: new List<string>()
                );

                // Use LoginResponse as the data type
                var loginResult = await _httpCaller.GetAsync<IEnumerable<EmailConfigurationResponse>>(httpCallOptions);
                if (loginResult.Succeeded)
                {
                    return BaseResult<IEnumerable<EmailConfigurationResponse>>.Success(loginResult.Data);
                }
                return BaseResult<IEnumerable<EmailConfigurationResponse>>.Failure(loginResult.ExceptionError.StatusCode.ToString(), loginResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "GetEmailConfigurationByUserId occured exception");
                throw;
            }
        }

        public async Task<BaseResult<IEnumerable<EmailConfigurationDetailResponse>>> GetEmailConfigurationDetailByUserId(long id)
        {
            try
            {
                var key = _configuration["ApiKey:Identity"];

                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions.Authenticated(
                    isSerialized: true,
                    isRetry: false,
                    baseUrl: $"http://localhost:5212/api/Users/{id}/email-config/detail",
                    queryStringElements: new List<string>(),
                    "X-Api-Key", key
                );

                // Use LoginResponse as the data type
                var loginResult = await _httpCaller.GetAsync<IEnumerable<EmailConfigurationDetailResponse>>(httpCallOptions);
                if (loginResult.Succeeded)
                {
                    return BaseResult<IEnumerable<EmailConfigurationDetailResponse>>.Success(loginResult.Data);
                }
                return BaseResult<IEnumerable<EmailConfigurationDetailResponse>>.Failure(loginResult.ExceptionError.StatusCode.ToString(), loginResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "GetEmailConfigurationDetailByUserId occured exception");
                throw;
            }
        }

        public async Task<BaseResult<IEnumerable<ServiceEndpointResponse>>> GetEndpointOfEmailService()
        {
            try
            {
                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    baseUrl: "http://localhost:5212/api/Services/endpoint?serviceId=1",
                    queryStringElements: new List<string>()
                );

                // Use LoginResponse as the data type
                var endpointResult = await _httpCaller.GetAsync<IEnumerable<ServiceEndpointResponse>>(httpCallOptions);
                if (endpointResult.Succeeded)
                {
                    return BaseResult<IEnumerable<ServiceEndpointResponse>>.Success(endpointResult.Data);
                }
                return BaseResult<IEnumerable<ServiceEndpointResponse>>.Failure(endpointResult.ExceptionError.StatusCode.ToString(), endpointResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "GetEndpointOfEmailService occured exception");
                throw;
            }
        }

        public async Task<BaseResult<ItemPagingResponse<UserResponse>>> GetUserInEmailServiceWithPaging(UserSearchViewModel userSearchViewModel)
        {
            try
            {
                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    baseUrl: $"http://localhost:5212/api/Services/email/user?userName={userSearchViewModel.UserName}&pageIndex={userSearchViewModel.PageIndex}&pageSize={userSearchViewModel.PageSize}",
                    queryStringElements: new List<string>()
                );

                // Use LoginResponse as the data type
                var endpointResult = await _httpCaller.GetAsync<ItemPagingResponse<UserResponse>>(httpCallOptions);
                if (endpointResult.Succeeded)
                {
                    return BaseResult<ItemPagingResponse<UserResponse>>.Success(endpointResult.Data);
                }
                return BaseResult<ItemPagingResponse<UserResponse>>.Failure(endpointResult.ExceptionError.StatusCode.ToString(), endpointResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "GetUserInEmailServiceWithPaging occured exception");
                throw;
            }
        }

        public async Task<BaseResult> UpdateEmailConfiguration(UpdateUserEmailConfigRequest updateUserEmailConfigRequest)
        {
            try
            {
                // Call the Login API to get the accessToken and refreshToken
                var httpCallOptions = HttpCallOptions<UpdateUserEmailConfigRequest>.UnAuthenticated(
                    isSerialized: true,
                    isRetry: false,
                    body: updateUserEmailConfigRequest,
                    baseUrl: "http://localhost:5212/api/Users/email-config",
                    queryStringElements: new List<string>()
                );

                // Use LoginResponse as the data type
                var createUserApiKeyResult = await _httpCaller.PutAsync<UpdateUserEmailConfigRequest, long>(httpCallOptions);
                if (createUserApiKeyResult.Succeeded)
                {
                    return BaseResult<long>.Success(createUserApiKeyResult.Data);
                }
                return BaseResult<long>.Failure(createUserApiKeyResult.ExceptionError.StatusCode.ToString(), createUserApiKeyResult.ExceptionError.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex.InnerException, "UpdateUserEmailConfigRequest occured exception");
                throw;
            }
        }
    }
}
