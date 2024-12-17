using BuildingBlocks.Common;
using Escs_Client.ViewModels;

namespace Escs_Client.Interfaces
{
    public interface IEmailService
    {
        Task<BaseResult> CreateEmailConfiguration(EmailConfigurationViewModel emailConfigurationViewModel);

        Task<BaseResult> UpdateEmailConfiguration(UpdateUserEmailConfigRequest updateUserEmailConfigRequest);

        Task<BaseResult<EmailConfigurationDetailResponse>> GetEmailConfigurationById(long id);
        Task<BaseResult<IEnumerable<EmailConfigurationResponse>>> GetEmailConfigurationByUserId(long id);

        Task<BaseResult<IEnumerable<EmailConfigurationDetailResponse>>> GetEmailConfigurationDetailByUserId(long id);

        Task<BaseResult<IEnumerable<ServiceEndpointResponse>>> GetEndpointOfEmailService();

        Task<BaseResult<ItemPagingResponse<UserResponse>>> GetUserInEmailServiceWithPaging(UserSearchViewModel userSearchViewModel);
    }
}
