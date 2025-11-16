# Role-Based Blazor Web App with ASP.NET Core Identity

A complete Blazor Server application with ASP.NET Core Identity Framework, role-based authentication, separate login/register pages, and custom layouts.

## Features

- ✅ **ASP.NET Core Identity Framework** - Full-featured authentication system
- ✅ **SQLite Database** - Lightweight database for user storage
- ✅ **User Registration** - New users can create accounts
- ✅ **Remember Me** - Persistent login sessions
- ✅ **Password Hashing** - Secure password storage
- ✅ **Account Lockout** - Protection against brute force attacks
- ✅ **Role-based Authorization** (Admin, Manager, User)
- ✅ **Separate Login/Register Pages** with custom layout
- ✅ **Database Seeding** - Auto-creates demo users and roles
- ✅ **Protected Routes** with `[Authorize]` attribute
- ✅ **Role-specific Navigation** menu
- ✅ **Responsive Design**

## Project Structure

```
RoleBasedBlazorApp/
├── Controllers/
│   └── AccountController.cs                  # Logout endpoint
├── Data/
│   ├── ApplicationDbContext.cs               # Identity database context
│   ├── ApplicationUser.cs                    # Custom user model
│   └── DatabaseSeeder.cs                     # Seeds default users/roles
├── Components/
│   ├── Layout/
│   │   ├── LoginLayout.razor                 # Login/Register layout
│   │   ├── MainLayout.razor                  # Main app layout
│   │   ├── NavMenu.razor                     # Navigation menu
│   │   └── LogoutButton.razor                # Logout component
│   ├── Pages/
│   │   ├── Login.razor                       # Login page
│   │   ├── Register.razor                    # Registration page
│   │   ├── Home.razor                        # Home page
│   │   ├── Dashboard.razor                   # Dashboard (Admin, Manager)
│   │   ├── Admin.razor                       # Admin panel (Admin only)
│   │   ├── Reports.razor                     # Reports (Admin, Manager)
│   │   └── Profile.razor                     # User profile
│   ├── App.razor                             # Root component
│   └── Routes.razor                          # Routing configuration
├── Models/
│   └── UserModel.cs                          # User model
├── wwwroot/                                  # Static files
├── appsettings.json                          # Configuration
└── Program.cs                                # App configuration
```

## Demo Credentials

| Username | Password    | Role    | Email                  |
|----------|-------------|---------|------------------------|
| admin    | Admin@123   | Admin   | admin@example.com      |
| manager  | Manager@123 | Manager | manager@example.com    |
| user     | User@123    | User    | user@example.com       |

**Password Requirements:**
- At least 6 characters
- Contains uppercase letter
- Contains lowercase letter
- Contains digit

## Running the Application

### Prerequisites
- .NET 8.0 SDK or later

### Steps

1. **Navigate to the project directory:**
```bash
cd RoleBasedBlazorApp
```

2. **Restore dependencies:**
```bash
dotnet restore
```

3. **Run the application:**
```bash
dotnet run
```

4. **Open your browser and navigate to:**
```
https://localhost:5001
or
http://localhost:5000
```

5. **The database will be automatically created and seeded with demo users**

6. **You'll be redirected to `/login`**

7. **Use demo credentials or register a new account**

## ASP.NET Core Identity Features

### Database

The application uses **SQLite** as the database provider. The database file (`app.db`) will be created automatically in the project root when you first run the application.

**Entity Framework Core** migrations are handled automatically via:
```csharp
await context.Database.EnsureCreatedAsync();
```

### User Management

**ApplicationUser** extends `IdentityUser` and includes:
- Standard Identity properties (Id, Username, Email, etc.)
- Custom property: `FullName`
- Custom property: `CreatedDate`

### Authentication Flow

1. **Registration**:
   - User fills registration form
   - `UserManager.CreateAsync()` creates user with hashed password
   - User is automatically added to "User" role
   - Auto sign-in after registration

2. **Login**:
   - User enters credentials
   - `SignInManager.PasswordSignInAsync()` validates credentials
   - Creates authentication cookie
   - Supports "Remember Me" functionality
   - Includes lockout protection (5 failed attempts = 5 min lockout)

3. **Logout**:
   - Form POST to `/Account/logout`
   - `SignInManager.SignOutAsync()` clears authentication cookie
   - Redirects to login page

### Password Security

Identity automatically:
- **Hashes passwords** using PBKDF2 with HMAC-SHA256
- **Salts** each password uniquely
- **Never stores plain text** passwords

### Account Lockout

After **5 failed login attempts**, accounts are locked for **5 minutes** to prevent brute force attacks.

### Role-Based Authorization

Three roles are automatically created:
- **Admin**: Full access to all features
- **Manager**: Access to Dashboard, Reports, Profile
- **User**: Access to Home and Profile only

Pages use the `[Authorize]` attribute:
```csharp
@attribute [Authorize(Roles = "Admin")]
@attribute [Authorize(Roles = "Admin,Manager")]
@attribute [Authorize]
```

## Database Schema

Identity creates the following tables:
- `AspNetUsers` - User accounts
- `AspNetRoles` - Role definitions
- `AspNetUserRoles` - User-role mappings
- `AspNetUserClaims` - User claims
- `AspNetUserLogins` - External login providers
- `AspNetUserTokens` - Authentication tokens
- `AspNetRoleClaims` - Role claims

## Customization

### Adding Custom User Properties

Edit `Data/ApplicationUser.cs`:
```csharp
public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string? PhoneNumber { get; set; }  // Add your property
}
```

### Creating New Roles

Edit `Data/DatabaseSeeder.cs` and add to the `roleNames` array:
```csharp
string[] roleNames = { "Admin", "Manager", "User", "Supervisor" };
```

### Password Policy

Modify in `Program.cs`:
```csharp
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;  // Change to 8
});
```

### Using a Different Database

Replace SQLite with SQL Server or PostgreSQL:

**For SQL Server:**
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

**Connection String (appsettings.json):**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RoleBasedBlazorApp;Trusted_Connection=True;"
}
```

### Email Confirmation

To enable email confirmation (production):

1. Install an email service package (SendGrid, SMTP, etc.)
2. Remove `EmailConfirmed = true` in registration
3. Generate email confirmation token
4. Send confirmation email
5. Create email confirmation endpoint

### Two-Factor Authentication

Enable 2FA by adding:
```csharp
options.SignIn.RequireConfirmedAccount = true;
options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
```

## Database Management

### View Database

Use a SQLite browser like:
- [DB Browser for SQLite](https://sqlitebrowser.org/)
- [SQLite Studio](https://sqlitestudio.pl/)

### Reset Database

Delete `app.db` file and restart the application. It will recreate and reseed automatically.

### Manual Migrations (Optional)

If you want to use migrations instead of `EnsureCreatedAsync()`:

```bash
# Install EF Core tools
dotnet tool install --global dotnet-ef

# Create initial migration
dotnet ef migrations add InitialCreate

# Apply migration
dotnet ef database update
```

## Production Considerations

⚠️ **Before deploying to production:**

1. **Use a Production Database**
   - SQL Server, PostgreSQL, or MySQL
   - Not SQLite for production

2. **Enable Email Confirmation**
   - Verify user email addresses
   - Send confirmation emails

3. **Enable HTTPS**
   - Already configured in Program.cs
   - Use valid SSL certificates

4. **Secure Connection Strings**
   - Use Azure Key Vault, AWS Secrets Manager, or User Secrets
   - Never commit connection strings to source control

5. **Enable Logging**
   - Log authentication attempts
   - Monitor failed logins

6. **Add Two-Factor Authentication**
   - Increases security significantly

7. **Implement CAPTCHA**
   - On login/registration to prevent bots

8. **Set Strong Password Policy**
   - Increase minimum length
   - Require special characters

9. **Configure CORS** (if needed for APIs)

10. **Regular Security Updates**
    - Keep NuGet packages updated

## Troubleshooting

### Database Locked Error
- Close any SQLite browser tools
- Restart the application

### Login Not Working
- Ensure password meets requirements (6+ chars, uppercase, lowercase, digit)
- Check database was seeded correctly
- Try registering a new account

### Roles Not Working
- Verify `DatabaseSeeder.SeedAsync()` was called
- Check user roles in `AspNetUserRoles` table
- Ensure `UseAuthorization()` is in Program.cs

## Technologies Used

- **.NET 8.0** - Framework
- **Blazor Server** - UI Framework  
- **ASP.NET Core Identity** - Authentication/Authorization
- **Entity Framework Core 8.0** - ORM
- **SQLite** - Database
- **Bootstrap 5** (custom) - Styling

## License

This is a demonstration project for educational purposes.

## Support

For issues with ASP.NET Core Identity, refer to:
- [Official Identity Documentation](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [Identity GitHub Repository](https://github.com/dotnet/aspnetcore/tree/main/src/Identity)


## Features

- ✅ Separate login page with custom layout
- ✅ Role-based authentication (Admin, Manager, User)
- ✅ Custom authentication state provider
- ✅ Protected routes with `[Authorize]` attribute
- ✅ Role-specific navigation menu
- ✅ Responsive design
- ✅ Clean separation of layouts

## Project Structure

```
RoleBasedBlazorApp/
├── Authentication/
│   └── CustomAuthenticationStateProvider.cs  # Custom auth provider
├── Components/
│   ├── Layout/
│   │   ├── LoginLayout.razor                 # Login page layout
│   │   ├── MainLayout.razor                  # Main app layout
│   │   └── NavMenu.razor                     # Navigation menu
│   ├── Pages/
│   │   ├── Login.razor                       # Login page
│   │   ├── Home.razor                        # Home page
│   │   ├── Dashboard.razor                   # Dashboard (Admin, Manager)
│   │   ├── Admin.razor                       # Admin panel (Admin only)
│   │   ├── Reports.razor                     # Reports (Admin, Manager)
│   │   └── Profile.razor                     # User profile
│   ├── App.razor                             # Root component
│   └── Routes.razor                          # Routing configuration
├── Models/
│   └── UserModel.cs                          # User model
├── wwwroot/                                  # Static files
└── Program.cs                                # App configuration
```

## Demo Credentials

| Username | Password    | Role    | Access Level                           |
|----------|-------------|---------|----------------------------------------|
| admin    | admin123    | Admin   | Full access to all pages               |
| manager  | manager123  | Manager | Access to Dashboard, Reports, Profile  |
| user     | user123     | User    | Access to Home and Profile only        |

## Key Components

### 1. Authentication System

**CustomAuthenticationStateProvider.cs**
- Manages authentication state
- Provides methods to authenticate users and logout
- Stores user claims (username and role)

### 2. Layouts

**LoginLayout.razor**
- Minimal layout for the login page
- Centered design with gradient background
- No navigation menu

**MainLayout.razor**
- Full application layout for authenticated users
- Includes sidebar navigation
- Shows user info and logout button
- Displays user's role

### 3. Pages and Authorization

**Login Page** (`/login`)
- Uses `LoginLayout`
- Form-based authentication
- Validates credentials
- Redirects to home after successful login

**Home Page** (`/`)
- Requires authentication (`[Authorize]`)
- Shows user role and accessible pages
- Uses `MainLayout`

**Dashboard** (`/dashboard`)
- Restricted to `Admin` and `Manager` roles
- Displays statistics cards

**Admin Panel** (`/admin`)
- Restricted to `Admin` role only
- System settings and user management

**Reports** (`/reports`)
- Accessible to `Admin` and `Manager`
- Various report types

**Profile** (`/profile`)
- Available to all authenticated users
- Shows user information and account settings

### 4. Navigation Menu

The `NavMenu.razor` component uses `<AuthorizeView>` to show/hide menu items based on user roles:

```razor
<AuthorizeView Roles="Admin">
    <!-- Only visible to Admin -->
</AuthorizeView>

<AuthorizeView Roles="Admin,Manager">
    <!-- Visible to Admin and Manager -->
</AuthorizeView>

<AuthorizeView>
    <!-- Visible to all authenticated users -->
</AuthorizeView>
```

## Running the Application

### Prerequisites
- .NET 8.0 SDK or later

### Steps

1. Navigate to the project directory:
```bash
cd RoleBasedBlazorApp
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Run the application:
```bash
dotnet run
```

4. Open your browser and navigate to:
```
https://localhost:5001
or
http://localhost:5000
```

5. You'll be redirected to `/login`

6. Use any of the demo credentials to login

## How It Works

### Authentication Flow

1. **Login**: User enters credentials on login page
2. **Validation**: Credentials are validated against demo user list
3. **Authentication**: `CustomAuthenticationStateProvider` creates user claims
4. **Redirect**: User is redirected to home page
5. **Authorization**: Each page checks user's role via `[Authorize]` attribute

### Route Protection

Routes are protected using the `[Authorize]` attribute:

```csharp
// Requires authentication
@attribute [Authorize]

// Requires specific role(s)
@attribute [Authorize(Roles = "Admin")]
@attribute [Authorize(Roles = "Admin,Manager")]
```

### Layout Selection

The login page explicitly sets its layout:

```razor
@page "/login"
@layout LoginLayout
```

All other pages use `MainLayout` as the default (configured in `Routes.razor`).

## Customization

### Adding New Roles

1. Update demo users in `Login.razor`:
```csharp
var users = new Dictionary<string, (string Password, string Role)>
{
    { "newrole", ("password", "NewRole") }
};
```

2. Add role-based menu items in `NavMenu.razor`:
```razor
<AuthorizeView Roles="NewRole">
    <div class="nav-item px-3">
        <NavLink class="nav-link" href="newpage">
            New Page
        </NavLink>
    </div>
</AuthorizeView>
```

3. Create protected pages with the new role:
```razor
@attribute [Authorize(Roles = "NewRole")]
```

### Connecting to a Database

Replace the demo user validation in `Login.razor` with database calls:

```csharp
// Example with Entity Framework
private async Task<(bool isValid, string role)> ValidateCredentials(string username, string password)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Username == username);
    
    if (user != null && VerifyPassword(password, user.PasswordHash))
    {
        return (true, user.Role);
    }
    
    return (false, string.Empty);
}
```

### Styling

- Modify `LoginLayout.razor.css` for login page styling
- Modify `MainLayout.razor.css` for main app styling
- Edit `wwwroot/app.css` for global styles

## Security Notes

⚠️ **Important**: This is a demo application. For production use:

1. **Never store passwords in plain text**
   - Use password hashing (BCrypt, PBKDF2, Argon2)
   
2. **Use a real database**
   - Don't hardcode user credentials
   
3. **Implement proper session management**
   - Use secure cookies
   - Set appropriate expiration times
   
4. **Add HTTPS enforcement**
   - Already included in `Program.cs`
   
5. **Implement CSRF protection**
   - Use `@attribute [ValidateAntiForgeryToken]`
   
6. **Add logging and monitoring**
   - Track login attempts
   - Monitor authorization failures

## License

This is a demonstration project for educational purposes.
