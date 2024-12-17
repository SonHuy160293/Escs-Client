using BuildingBlocks.Common;

namespace Escs_Client.ViewModels
{
    public class SearchViewModel
    {
    }

    public class UserSearchViewModel : BaseSearchRequest
    {
        public string? UserName { get; set; } = default!;
    }
}
