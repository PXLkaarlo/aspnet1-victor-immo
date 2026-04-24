using Application.Abstractions.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Areas.Account.Models;
using System.Security.Claims;

namespace Presentation.WebApp.Areas.Account.Controllers;

[Area("Account")]
[Route("me")]
[Authorize]
public class AccountController(IAccountService accountService, IAuthService authService) : Controller
{
    public IActionResult Index() => RedirectToAction(nameof(AboutMe));


    [HttpGet("about-me")]
    public async Task<IActionResult> AboutMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User ID claim is missing.");

        if (!string.IsNullOrWhiteSpace(userId))
        {
            var account = await accountService.GetUserAccountAsync(userId);
            var accountDetails = account.Details;

            var viewModel = new AboutMeViewModel
            {
                AboutMeForm = new AboutMeForm
                {
                    FirstName = accountDetails?.FirstName ?? "",
                    LastName = accountDetails?.LastName ?? "",
                    Email = accountDetails?.Email ?? "",
                    PhoneNumber = accountDetails?.PhoneNumber ?? ""
                },
                ProfileImageUrl = accountDetails?.ImageUrl ?? "~/images/profile-image-avatar.png"
                // This default image URL will be tricky to handle later.
                // Needs to be resolved in the view file.
            };

            return View(viewModel);
        }

        await authService.SignOutUserAsync();

        return Redirect("/");
    }


    [HttpPost("about-me")]
    public async Task<IActionResult> AboutMe(AboutMeViewModel viewModel)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var account = await accountService.GetUserAccountAsync(userId);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Failed to retrieve user ID: " + ex);

            return RedirectToAction(nameof(SignOut));
        }
        // Continue working on this method later.

        return RedirectToAction(nameof(AboutMe));
    }
}
