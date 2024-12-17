using System.ComponentModel.DataAnnotations;

namespace Escs_Client.ViewModels
{
    public class EmailConfigurationViewModel
    {
        [Required(ErrorMessage = "SmtpEmail is required")]
        public string SmtpEmail { get; set; } = default!;
        [Required(ErrorMessage = "SmtpPassword is required")]
        public string SmtpPassword { get; set; } = default!;
        [Required(ErrorMessage = "SmtpPort is required")]
        public int SmtpPort { get; set; }
        public long UserId { get; set; }
        public long ServiceId { get; set; }

    }

    public class GetEmailConfigurationViewModel
    {
        public long Id { get; set; }
        public string SmtpEmail { get; set; } = default!;
        public string SmtpPassword { get; set; } = default!;
        public int SmtpPort { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
        public long ServiceId { get; set; }

    }
}
