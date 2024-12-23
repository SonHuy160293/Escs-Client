using BuildingBlocks.Common;
using Escs_Client.ViewModels;

namespace Escs_Client.Interfaces
{
    public interface IKeyService
    {
        Task<BaseResult<long>> CreateUserApiKey(CreateUserApiKeyRequest createUserApiKeyRequest);

        Task<BaseResult> CreateUserApiKeyAllowedEndpointTransaction(CreateUserApiKeyAllowedEndpointTransactionRequest createUserApiKeyRequest);
        Task<BaseResult<long>> UpdateUserApiKeyStatus(UpdateUserApiKeyRequest updateUserApiKeyRequest);
        Task<BaseResult<IEnumerable<UserApiKeyDetailResponse>>> GetKeyDetail(long userId, long serviceId);
        Task<BaseResult> CreateKeyAllowedEndpoint(CreateKeyAllowedEndpointRequest createKeyAllowedEndpointRequest);
        Task<BaseResult<IEnumerable<ServiceEndpointRegisterByUserResponse>>> GetServiceEndpointRegisterByUser(long userId);


    }
}
