using BuildingBlocks.Common;
using Escs_Client.ViewModels;

namespace Escs_Client.Interfaces
{
    public interface IAuthenService
    {
        public Task<BaseResult> Login(LoginViewModel loginViewModel);
        public Task<BaseResult> Logout();
    }
}
