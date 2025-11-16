using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoleBasedBlazorApp.Data;

namespace RoleBasedBlazorApp.Controllers;

[Route("[controller]")]
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Redirect("/login");
    }
}
