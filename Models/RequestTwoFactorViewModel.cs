using System.ComponentModel.DataAnnotations;

namespace TwilioVerifyAspNet.Models;

public class RequestTwoFactorViewModel
{
    [Required]
    public string PhoneNumber { get; set; }
}
