namespace Escs_Client.ViewModels
{
    public class Request
    {
    }

    public class LogoutRequest
    {
        public string RefreshToken { get; set; }
    }

    public class CreateUserApiKeyRequest
    {
        public long UserId { get; set; }
        public long ServiceId { get; set; }
    }

    public class CreateUserApiKeyAllowedEndpointTransactionRequest
    {
        public long UserId { get; set; }
        public long ServiceId { get; set; }
        public List<long> EndpointId { get; set; }
    }

    public class UpdateUserApiKeyRequest
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateEndpointOfKeyRequest
    {
        public long KeyId { get; set; }
        public List<long> EndpointsId { get; set; }

    }

    public class CreateKeyAllowedEndpointRequest
    {
        public long UserApiKeyId { get; set; }
        public List<long> EndpointId { get; set; }
    }

    public class CreateEndpointRequest
    {
        public string Method { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string Description { get; set; } = default!;
        public long ServiceId { get; set; }
    }

    public class UpdateEndpointRequest
    {
        public long Id { get; set; }
        public string Method { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsActive { get; set; }

    }

    public class UpdateUserEmailConfigRequest
    {
        public long Id { get; set; }
        public string SmtpEmail { get; set; } = default!;
        public string SmtpServer { get; set; } = default!;
        public string SmtpPassword { get; set; } = default!;
        public int SmtpPort { get; set; }
        public bool IsActive { get; set; }
        public bool IsEnableSsl { get; set; }

    }
}