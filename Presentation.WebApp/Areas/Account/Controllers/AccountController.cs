using Application.Abstractions.Identity;
using Application.Dtos.Identity;
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


    #region about-me

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
                Form = new AboutMeForm
                {
                    FirstName = accountDetails?.FirstName ?? "",
                    LastName = accountDetails?.LastName ?? "",
                    Email = accountDetails?.Email ?? "",
                    PhoneNumber = accountDetails?.PhoneNumber ?? ""
                },
                ProfileImageUrl = accountDetails?.ImageUrl
                // This default image URL will be tricky to handle later.
                // Needs to be resolved in the view file.
            };

            return View(viewModel);
        }

        await authService.SignOutUserAsync();

        return Redirect("/");
    }


    [HttpPost("about-me")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AboutMe(AboutMeViewModel viewModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            Console.Error.WriteLine("Failed to retrieve user ID: " + userId);

            return RedirectToAction(nameof(SignOut));
        }


        var account = await accountService.GetUserAccountAsync(userId);

        if (account.Details is null)
        {
            Console.Error.WriteLine("Failed to retrieve account details for user ID: " + userId);

            return RedirectToAction(nameof(SignOut));
        }


        var imageUrl = account.Details.ImageUrl;

        viewModel.ProfileImageUrl = imageUrl;


        if (!ModelState.IsValid)
            return View(viewModel);
        
        
        if (viewModel.Form is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid form submission.");

            return View(viewModel);
        }
        

        var viewModelForm = viewModel.Form;

        if (viewModelForm.ProfileImage is not null && viewModelForm.ProfileImage.Length > 0)
        {
            imageUrl = await SaveProfileImageAsync(viewModelForm.ProfileImage);
        }


        var details = new UpdateAccountDetails(
            userId,
            viewModelForm.Email,
            viewModelForm.FirstName,
            viewModelForm.LastName,
            viewModelForm.PhoneNumber,
            imageUrl
        );

        var result = await accountService.UpdateUserAccountDetailsAsync(details);

        if (!result.Succeeded)
        {
            viewModel.ProfileImageUrl = imageUrl;
            viewModel.Message = "Unable to save changes.";
            return View(viewModel);
        }


        return RedirectToAction(nameof(AboutMe));
    }

    #endregion


    #region sign-out and remove account

    [HttpGet("sign-out")]
    public new async Task<IActionResult> SignOut()
    {
        await authService.SignOutUserAsync();

        return Redirect("/");
    }


    [HttpGet("remove-account")]
    public async Task<IActionResult> RemoveAccount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrWhiteSpace(userId))
        {
            var deleted = await accountService.DeleteUserAccountAsync(userId);

            if (!deleted.Succeeded)
            {
                ViewBag.Message = deleted.ErrorMessage;

                return View();
            }
        }


        await authService.SignOutUserAsync();

        return Redirect("/");
    }

    #endregion


    // Save profile image
    private static async Task<string> SaveProfileImageAsync(IFormFile file)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");

        Directory.CreateDirectory(uploadsFolder);


        var extension = Path.GetExtension(file.FileName);

        var fileName = $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(uploadsFolder, fileName);


        await using var stream = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(stream);


        return $"/uploads/profiles/{fileName}";
    }
}
