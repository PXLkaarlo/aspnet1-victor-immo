using Application.Abstractions.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Areas.Authentication.Models;

namespace Presentation.WebApp.Areas.Authentication.Controllers;

[Area("Authentication")]
[Route("registration")]
public class SignUpController(IAuthService authService) : Controller
{
    #region sign-up

    const string SessionEmailAddressKey = "SessionEmailAddressKey";

    [HttpGet("registration")]
    public IActionResult Index() => RedirectToAction(nameof(SetEmail));


    [HttpGet("registration/sign-up")]
    public IActionResult SetEmail()
    {
        var viewModel = new SetEmailViewModel();
        return View(viewModel);
    }

    [HttpPost("registration/sign-up")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetEmail(SetEmailViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var exists = await authService.UserExistsAsync(viewModel.Form.Email);

        // if (exists.Succeeded == true) => "User already exists."
        // if (exists.Succeeded == false) => Not Found
        if (exists.Succeeded)
        {
            ModelState.AddModelError(nameof(viewModel.ErrorMessage), exists?.ErrorMessage ?? "User already exists.");

            return View(viewModel);
        }

        HttpContext.Session.SetString(SessionEmailAddressKey, viewModel.Form.Email);

        return RedirectToAction(nameof(SetPassword));
    }
    
    #endregion


    [HttpGet("set-password")]
    public IActionResult SetPassword()
    {
        var email = HttpContext.Session.GetString(SessionEmailAddressKey);

        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction(nameof(Index));


        return View();
    }

    [HttpPost("set-password")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(SetPasswordForm form)
    {
        if (!ModelState.IsValid)
            return View(form);

        var email = HttpContext.Session.GetString(SessionEmailAddressKey);

        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction(nameof(Index));


        var result = await authService.CreateUserAsync(email, form.Password, "Member");

        if (!result.Succeeded)
        {
            ModelState.AddModelError(nameof(form.ErrorMessage), result?.ErrorMessage ?? "Unable to create user.");
            return View(form);
        }


        var loggedIn = await authService.SignInUserAsync(email, form.Password, false);


        return RedirectToAction("Index", "SignIn");
    }
}
