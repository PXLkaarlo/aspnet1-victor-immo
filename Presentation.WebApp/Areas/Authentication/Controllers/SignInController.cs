using Application.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Areas.Authentication.Models;
using System.Security.Claims;

namespace Presentation.WebApp.Areas.Authentication.Controllers;

[Area("Authentication")]
public class SignInController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager) : Controller
{
    private readonly Dictionary<string, string> _redirectPaths = new()
    {
        { "Admin", "/admin" },
        { "Member", "/me" }
    };

    public IActionResult Index() => RedirectToAction(nameof(SignIn));


    #region local sign-in

    [HttpGet("sign-in")]
    public IActionResult SignIn(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        var redirectPath = AuthenticationRedirectManager.GetRedirectPath(User, _redirectPaths);

        return !string.IsNullOrWhiteSpace(redirectPath)
            ? Redirect(redirectPath)
            : View();
    }


    [HttpPost("sign-in")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(SignInForm form, string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Incorrect email adress or password.";
            return View(form);
        }

        var result = await signInManager.PasswordSignInAsync(form.Email, form.Password, form.RememberMe, false);

        if (result.IsLockedOut)
        {
            TempData["ErrorMessage"] = "This user is temporarily locked out.";
            return View(form);
        }

        if (result.IsNotAllowed)
        {
            TempData["ErrorMessage"] = "This user is not allowed to login.";
            return View(form);
        }

        if (result.RequiresTwoFactor)
        {
            TempData["ErrorMessage"] = "This user requires two-factor authentication.";
            return View(form);
        }

        if (!result.Succeeded)
        {
            TempData["ErrorMessage"] = "Incorrect email address or password.";
            return View(form);
        }


        if (!string.IsNullOrWhiteSpace(returnUrl))
            return Redirect(returnUrl);


        var redirectPath = AuthenticationRedirectManager.GetRedirectPath(User, _redirectPaths);

        return !string.IsNullOrWhiteSpace(redirectPath)
            ? Redirect(redirectPath)
            : Redirect("/");
    }

    #endregion


    #region external login

    [HttpPost("external-login")]
    [ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string? returnUrl = null)
    {
        var callbackUrl = Url.Action(nameof(ExternalLoginCallback), "SignIn", new { area = "Authentication", returnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, callbackUrl);

        return Challenge(properties, provider);
    }


    [HttpGet]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        if (!string.IsNullOrWhiteSpace(remoteError))
        {
            TempData["ErrorMessage"] = $"External login error: { remoteError }";
            return RedirectToAction(nameof(SignIn), new { returnUrl });
        }

        var info = await signInManager.GetExternalLoginInfoAsync();

        if (info is null)
        {
            TempData["ErrorMessage"] = "Could not load external login information.";
            return RedirectToAction(nameof(SignIn), new { returnUrl });
        }


        var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

        if (result.Succeeded)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                var redirectPath = AuthenticationRedirectManager.GetRedirectPath(User, _redirectPaths);
                returnUrl = redirectPath ?? "/";
            }

            return Redirect(returnUrl);
        }


        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var firstName = info.Principal?.FindFirstValue(ClaimTypes.GivenName);
        var lastName = info.Principal?.FindFirstValue(ClaimTypes.Surname);
        var imageUrl =
            info.LoginProvider switch
            {
                // Add more providers as needed
                "GitHub" => info.Principal?.FindFirstValue("urn:github:picture"),
                _ => null
            };

        //var array = new[] { email, firstName, lastName, imageUrl };

        //foreach (var item in array)
        //{
        //    if (string.IsNullOrWhiteSpace(item))
        //    {
        //        TempData["ErrorMessage"] = "External login information is incomplete.";
        //        return RedirectToAction(nameof(SignIn), new { returnUrl });
        //    }
        //}
        // I suppose we can allow users to sign in even if some of the information is missing.
        // Names are not required.

        if (string.IsNullOrWhiteSpace(email))
        {
            TempData["ErrorMessage"] = $"No email address was returned from {info.LoginProvider}.";
            return RedirectToAction(nameof(SignIn), new { returnUrl });
        }


        var user = await userManager.FindByEmailAsync(email);
        user ??= AppUser.Create(email, firstName, lastName, imageUrl);

        var created = await userManager.CreateAsync(user);

        if (!created.Succeeded)
        {
            TempData["ErrorMessage"] = string.Join(", ", created.Errors.Select(x => x.Description));
            return RedirectToAction(nameof(SignIn), new { returnUrl });
        }


        var roleadded = await userManager.AddToRoleAsync(user, "Member");

        if (!roleadded.Succeeded)
        {
            TempData["ErrorMessage"] = string.Join(", ", roleadded.Errors.Select(x => x.Description));
            return RedirectToAction(nameof(SignIn), new { returnUrl });
        }

        var addedExternalLogin = await userManager.AddLoginAsync(user, info);

        if (!addedExternalLogin.Succeeded && addedExternalLogin.Errors.All(x => x.Code != "LoginAlreadyAssociated"))
        {
            TempData["ErrorMessage"] = string.Join(", ", addedExternalLogin.Errors.Select(x => x.Description));
            return RedirectToAction(nameof(SignIn), new { returnUrl });
        }


        await signInManager.SignInAsync(user, isPersistent: false);

        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            var redirectPath = AuthenticationRedirectManager.GetRedirectPath(User, _redirectPaths);
            returnUrl = redirectPath ?? "/";
        }


        return Redirect(returnUrl);
    }

    #endregion
}
