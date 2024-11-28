using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZeraAPI.Model;
using ZeraAPI.ZeraAPI.Data;
using ZeraAPI.ZeraAPI.Model;
using ZeraAPI.ZeraAPI.viewModels;

namespace JwtAuth01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<UserCustomised> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext Context;


        public AuthController(UserManager<UserCustomised> userManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            Context = context;
        }

        //http://localhost:5167/api/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest($"{model.Email} is already associated with an existing account. ");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model.Password != model.PasswordConfirmation)
            {
                return BadRequest("Passwords do not match !");
            }
            var user = new UserCustomised
            {
                Name = model.Name,
                UserName = model.Email,
                Email = model.Email,
                Password = model.Password,
                Address = model.Address
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new { Message = $"{model.Name} successfully created an account. " });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByEmailAsync(model.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid Email or Password.");
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                return BadRequest("User email is invalid.");
            }

            var AuthClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiryMinutes"]!)),
                claims: AuthClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Decode the JWT to extract the payload (no signature or header)
            var payload = new JwtSecurityTokenHandler().ReadJwtToken(tokenString).Payload;

            // Get the email from the payload and use it as CartId
            var cartId = payload[ClaimTypes.Email]?.ToString();

            if (cartId == null)
            {
                return BadRequest("No email found in the token payload.");
            }

            try
            {
                // Check if a cart already exists for the user
                var existingCart = await Context.carts.FirstOrDefaultAsync(c => c.CartId == cartId);

                if (existingCart != null)
                {
                    // Cart already exists, return success with no changes
                    return Ok(new
                    {
                        token = tokenString,
                        expiration = token.ValidTo,
                        message = "Login successful. Cart already exists.",
                        CartId = cartId
                    });
                }

                // Create a new cart if none exists
                var newCart = new Cart
                {
                    Id = Guid.NewGuid().ToString(),
                    CartId = cartId,
                    cartItems = new List<CartItem>(),
                };

                Context.carts.Add(newCart);
                await Context.SaveChangesAsync();

                return Ok(new
                {
                    token = tokenString,
                    expiration = token.ValidTo,
                    message = "Login successful. New cart created.",
                    CartId = cartId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating or checking cart: {ex.Message}");
            }
        }



        //http://localhost:5167/api/Auth/Status        
        [HttpGet("Status")]
        public IActionResult Status()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var userName = User.Identity.Name; // Default claim for the username
                var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                return Ok(new
                {
                    message = "User is logged in.",
                    userName = userName,
                    email = email
                });
            }

            return Unauthorized(new
            {
                message = "User is not logged in."
            });
        }

    }
}