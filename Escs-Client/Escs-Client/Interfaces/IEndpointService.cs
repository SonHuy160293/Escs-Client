using BuildingBlocks.Common;
using Escs_Client.ViewModels;

namespace Escs_Client.Interfaces
{
    public interface IEndpointService
    {
        Task<BaseResult<ServiceEndpointResponse>> GetEndpointById(long id);
        Task<BaseResult> CreateEndpoint(CreateEndpointRequest createEndpointRequest);
        Task<BaseResult> UpdateEndpoint(UpdateEndpointRequest updateEndpointRequest);
    }
}
