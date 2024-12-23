namespace Escs_Client.ViewModels
{
    public class Response
    {
    }
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class UserApiKeyResponse
    {
        public string Key { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }
    }

    public class ServiceEndpointResponse
    {
        public long Id { get; set; }
        public string Method { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsActive { get; set; }
    }

    public class UserApiKeyDetailResponse
    {
        public long Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }

        public List<KeyAllowedEndpointDetailDto> AllowedEndpoints { get; set; }
    }

    public class KeyAllowedEndpointDetailDto
    {
        public bool IsActive { get; set; } = default;
        public string Method { get; set; } = default;
        public string Url { get; set; } = default;
        public string Description { get; set; } = default;
    }

    public class ItemPagingResponse<T>
    {
        public List<T> Items { get; set; }


        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }

    public class UserResponse
    {
        public long UserId { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string? PhoneNumber { get; set; } = default!;
    }


    public class EmailConfigurationResponse
    {
        public long Id { get; set; }
        public string SmtpEmail { get; set; } = default!;
        public string SmtpServer { get; set; } = default!;
        public int SmtpPort { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
        public long ServiceId { get; set; }

    }

    public class EmailConfigurationDetailResponse
    {
        public long Id { get; set; }
        public string SmtpEmail { get; set; } = default!;
        public string SmtpPassword { get; set; } = default!;
        public string SmtpServer { get; set; } = default!;
        public int SmtpPort { get; set; }
        public bool IsActive { get; set; }
        public bool IsEnableSsl { get; set; }
        public long UserId { get; set; }
        public long ServiceId { get; set; }

    }

    public class ServiceEndpointRegisterByUserResponse
    {
        public long ServiceId { get; set; }
        public long UserId { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
    }

    public class EndpointUsageResponse
    {
        public long ServiceId { get; set; }
        public long UserId { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
    }

}
