using Microsoft.AspNetCore.Identity;

namespace RoleBasedBlazorApp.Data;

public class ApplicationUser : IdentityUser
{
    // You can add custom properties here
    public string? FullName { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
