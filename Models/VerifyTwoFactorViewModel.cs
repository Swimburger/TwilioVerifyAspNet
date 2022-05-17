using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TwilioVerifyAspNet.Models;

public class VerifyTwoFactorViewModel
{
    [Required]
    public string? VerificationSid { get; set; }
    
    [Required]
    public string? Code { get; set; }

    // prevent user from setting the status via form
    [BindNever]
    public string? Status { get; set; }

}