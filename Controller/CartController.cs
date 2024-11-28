using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeraAPI.DTO;
using ZeraAPI.ZeraAPI.Data;
using ZeraAPI.ZeraAPI.Model;

namespace ZeraAPI.Controller;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }
    //http://localhost:5167/api/cart/Userid
    [HttpGet("usercart")]
    public async Task<IActionResult> GetAllCartByUserId()
    {
        // Extract the cartId (which is the user's email) from the JWT token
        var cartId = User.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(cartId))
        {
            return Unauthorized("CartId not found. Please log in.");
        }

        // Retrieve the cart, including its associated cartItems
        var cart = await _context.carts
            .Where(c => c.CartId == cartId)
            .Include(c => c.cartItems)
            .ThenInclude(ci => ci.product)
            .FirstOrDefaultAsync();

        if (cart == null)
        {
            return NotFound("Cart not found.");
        }

        return Ok(cart);
    }

    //http://localhost:5167/api/cart/createCart

    [HttpPost("createCart")]
    public async Task<IActionResult> CreateCart([FromBody] Cart cart)
    {
        if (cart == null)
        {
            return BadRequest("Cart data is null.");
        }

        foreach (var item in cart.cartItems)
        {
            var productExists = await _context.products.AnyAsync(p => p.ProductId == item.ProductId);
            if (!productExists)
            {
                return BadRequest($"Product with ID {item.ProductId} does not exist.");
            }
        }

        _context.Add(cart);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAllCartByUserId), new { id = cart.Id }, cart);
    }

    //http://localhost:5167/api/cart/update

    [HttpPut("update")]
    public async Task<IActionResult> UpdateCart(string id, [FromBody] Cart cart)
    {
        if (id != cart.CartId)
        {
            return BadRequest("cart ID mismatch");
        }
        _context.Entry(cart).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }
    //http://localhost:5167/api/cart/Delete
    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteCart([FromBody] CartIdRequest request)
    {
        var cart = await _context.carts.FindAsync(request.cartId);
        if (cart == null)
        {
            return NotFound("Cart not found.");
        }
        _context.carts.Remove(cart);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}