using Microsoft.AspNetCore.Mvc;
using Twilio.Rest.Verify.V2.Service;
using TwilioVerifyAspNet.Models;

public class HomeController : Controller
{
    private readonly string verifyServiceSid;

    public HomeController(IConfiguration configuration)
    {
        verifyServiceSid = configuration["Twilio:VerifyServiceSid"];
    }

    [HttpGet]
    public IActionResult Index() => View();
    
    [HttpGet]
    public IActionResult RequestTwoFactorToken() => View();
    
    [HttpPost]
    public async Task<IActionResult> RequestTwoFactorToken(RequestTwoFactorViewModel requestTwoFactorViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(requestTwoFactorViewModel);
        }

        var verificationResource = await VerificationResource.CreateAsync(
            to: requestTwoFactorViewModel.PhoneNumber,
            channel: "sms",
            pathServiceSid: verifyServiceSid
        );

        var verifyTwoFactorViewModel = new VerifyTwoFactorViewModel
        {
            VerificationSid = verificationResource.Sid,
            Code = null
        };
        return View("VerifyTwoFactorToken", verifyTwoFactorViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> VerifyTwoFactorToken(VerifyTwoFactorViewModel verifyTwoFactorViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(verifyTwoFactorViewModel);
        }

        var verificationCheck = await VerificationCheckResource.CreateAsync(
            verificationSid: verifyTwoFactorViewModel.VerificationSid,
            code: verifyTwoFactorViewModel.Code,
            pathServiceSid: verifyServiceSid
        );

        switch (verificationCheck.Status)
        {
            case "pending":
                verifyTwoFactorViewModel.Status = "pending";
                return View("VerifyTwoFactorToken", verifyTwoFactorViewModel);
            case "canceled":
                verifyTwoFactorViewModel.Status = "canceled";
                return View("VerifyTwoFactorToken", verifyTwoFactorViewModel);
            case "approved":
                return View("Success");
        }
            
        return View("VerifyTwoFactorToken", verifyTwoFactorViewModel);
    }
}