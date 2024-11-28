////using Microsoft.AspNetCore.Identity;
////using Microsoft.AspNetCore.Mvc;
////using System.Security.Claims;
////using ZeraAPI.Model;
////using ZeraAPI.ZeraAPI.Data;
////using ZeraAPI.ZeraAPI.viewModels;

////namespace ZeraAPI.Controller;

////[ApiController]
////[Route("api/[controller]")]
////public class AccountController : ControllerBase
////{
////    private readonly ApplicationDbContext _context;
////    private readonly UserManager<UserCustomised> _userManager;
////    private readonly SignInManager<UserCustomised> _signInManager;

////    public AccountController(SignInManager<UserCustomised> signInManager, ApplicationDbContext context, UserManager<UserCustomised> userManager)
////    {
////        this._signInManager = signInManager;
////        this._context = context;
////        this._userManager = userManager;
////    }
////    //Registering a user.
////    [HttpPost("registerUser")]
////    public async Task<IActionResult> RegisterUser ([FromBody] Register registerModel){
////        if(registerModel == null)
////        {
////            return BadRequest("Invalid Data");
////        }
////        var ExistingUser = await _userManager.FindByEmailAsync(registerModel.Email);
////        if(ExistingUser != null)
////        {
////            return BadRequest("Email is already in use. ");
////        }
////        var user = new UserCustomised{
////            UserName = registerModel.Email,
////            Email = registerModel.Email,
////            Name = registerModel.Name,
////            Address = registerModel.Address
////        };
////        var result = await _userManager.CreateAsync(user, registerModel.Password);
////        if (result.Succeeded){
////            return Ok(new {Message = "User registered successfully ! "});
////        }
////        else{
////            return BadRequest(result.Errors);
////        }
////    }
////    //Login
////    [HttpPost("loginUser")]
////    public async Task<IActionResult> LoginUser ([FromBody] Login loginModel){
////        if(loginModel == null)
////        {
////            return BadRequest("Invalid Data");
////        }
////        var user = await _userManager.FindByEmailAsync(loginModel.UserName);
////        if(user == null)
////        {
////            return BadRequest("Invalid Email or Password");
////        }
////        var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false,false);
////        if(result.Succeeded)
////        {
////            return Ok(new {Message = "User logged in successfully ! "});
////        }

////        else
////        {
////            return BadRequest("Invalid Email or Password");
////        }
////    }

////// Logout
////    [HttpPost("logoutUser")]
////    public async Task<IActionResult> LogoutUser()
////    {
////        await _signInManager.SignOutAsync();
////        Response.Cookies.Delete(".AspNetCore.Identity.Application");
////        Response.Cookies.Delete("Identity.External");
////        Response.Cookies.Delete("Identity.TwoFactorUserId");

////        return Ok(new { Message = "User logged out successfully!" });
////    }

////    // Status - Check if user is logged in

////    //public IActionResult Status()
////    //{
////    //    if (User.Identity.IsAuthenticated)
////    //    {
////    //        return Ok(new { Message = "User is logged in", UserName = User.Identity.Name });
////    //    }
////    //    else
////    //    {
////    //        return Unauthorized(new { Message = "User is not logged in" });
////    //    }
////    //}
////    [HttpGet("status")]
////    public async Task<IActionResult> Status() {
////        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
////        if (string.IsNullOrEmpty(userId))
////        {
////            return BadRequest(new {Message = "user is not logged in" });
////        }
////        var user = await _userManager.FindByIdAsync(userId);
////        if (user == null)
////        {
////            return Unauthorized(new { Message = "user not found"});
////        }
////        return Ok(new {Message = "User is logged in", UserId= userId, Username = user.Name});
////    }
////}



//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using ZeraAPI.Model;
//using ZeraAPI.ZeraAPI.Data;
//using ZeraAPI.ZeraAPI.viewModels;

//namespace ZeraAPI.Controller;

//[ApiController]
//[Route("api/[controller]")]
//public class AccountController : ControllerBase
//{
//    private readonly ApplicationDbContext _context;
//    private readonly UserManager<UserCustomised> _userManager;
//    private readonly SignInManager<UserCustomised> _signInManager;
//    private readonly IConfiguration _configuration;

//    public AccountController(SignInManager<UserCustomised> signInManager, ApplicationDbContext context, UserManager<UserCustomised> userManager, IConfiguration configuration)
//    {
//        _signInManager = signInManager;
//        _context = context;
//        _userManager = userManager;
//        _configuration = configuration;
//    }

//    // Register a user
//    [HttpPost("registerUser")]
//    public async Task<IActionResult> RegisterUser([FromBody] Register registerModel)
//    {
//        if (registerModel == null)
//        {
//            return BadRequest("Invalid Data");
//        }

//        var existingUser = await _userManager.FindByEmailAsync(registerModel.Email);
//        if (existingUser != null)
//        {
//            return BadRequest("Email is already in use.");
//        }

//        var user = new UserCustomised
//        {
//            UserName = registerModel.Email,
//            Email = registerModel.Email,
//            Name = registerModel.Name,
//            Address = registerModel.Address
//        };

//        var result = await _userManager.CreateAsync(user, registerModel.Password);
//        if (result.Succeeded)
//        {
//            return Ok(new { Message = "User registered successfully!" });
//        }
//        else
//        {
//            return BadRequest(result.Errors);
//        }
//    }

//    // Login user and return JWT token
//    [HttpPost("loginUser")]
//    public async Task<IActionResult> LoginUser([FromBody] Login loginModel)
//    {
//        if (loginModel == null)
//        {
//            return BadRequest("Invalid Data");
//        }

//        var user = await _userManager.FindByEmailAsync(loginModel.UserName);
//        if (user == null)
//        {
//            return BadRequest("Invalid Email or Password");
//        }

//        var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
//        if (result.Succeeded)
//        {
//            // Generate JWT token
//            var token = GenerateJwtToken(user);
//            return Ok(new { Token = token });
//        }

//        return BadRequest("Invalid Email or Password");
//    }

//    // Logout user
//    [HttpPost("logoutUser")]
//    public async Task<IActionResult> LogoutUser()
//    {
//        await _signInManager.SignOutAsync();
//        // No need to remove cookies anymore, because we're using JWT
//        return Ok(new { Message = "User logged out successfully!" });
//    }

//    // Check if user is logged in by validating the JWT token
//    [HttpGet("status")]
//    public IActionResult Status()
//    {
//        if (User.Identity.IsAuthenticated)
//        {
//            return Ok(new { Message = "User is logged in", Username = User.Identity.Name });
//        }
//        else
//        {
//            return Unauthorized(new { Message = "User is not logged in" });
//        }
//    }

//    // Helper method to generate JWT token
//    private string GenerateJwtToken(UserCustomised user)
//    {
//        var claims = new[]
//        {
//            new Claim(ClaimTypes.NameIdentifier, user.Id),
//            new Claim(ClaimTypes.Name, user.UserName),
//            new Claim("FullName", user.Name),
//        };

//        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
//        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//        var token = new JwtSecurityToken(
//            issuer: _configuration["JwtSettings:Issuer"],
//            audience: _configuration["JwtSettings:Audience"],
//            claims: claims,
//            expires: DateTime.Now.AddHours(1),
//            signingCredentials: creds
//        );

//        return new JwtSecurityTokenHandler().WriteToken(token);
//    }
//}

