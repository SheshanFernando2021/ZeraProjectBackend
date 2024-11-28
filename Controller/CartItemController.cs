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
public class CartItemController : ControllerBase
{

    private readonly ApplicationDbContext _context;

    public CartItemController(ApplicationDbContext context)
    {
        _context = context;
    }
    //api/cartitem/cartitemall
    [HttpGet("cartitemall")]
    public async Task<ActionResult<List<CartItem>>> GetAllCartItems()
    {
        return await _context.cartItems.ToListAsync();
    }

    // api/cartitem/{cartId}
    [HttpGet("{cartId}")]
    public async Task<IActionResult> GetCartItems(string cartId)
    {
        var cartItems = await _context.cartItems
            .Where(ci => ci.CartId == cartId).ToListAsync();

        if (cartItems == null || !cartItems.Any())
        {
            return NotFound("Cart items not found.");
        }
        return Ok(cartItems);
    }

    //POST: cartitem
    [HttpPost("create")]
    public async Task<IActionResult> CreateCartItem([FromBody] CartItem cartItem)
    {
        if (cartItem == null)
        {
            return BadRequest("Invalid CartItem data.");
        }
        _context.cartItems.Add(cartItem);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCartItems), new { cartID = cartItem.CartId }, cartItem);
    }

    //http://localhost:5167/api/CartItem/add
    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        if (request == null || request.ProductId <= 0)
        {
            return BadRequest("Invalid product ID.");
        }

        var cartId = User.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(cartId))
        {
            return Unauthorized("CartId not found. Please log in.");
        }

        var cart = await _context.carts
            .FirstOrDefaultAsync(c => c.CartId == cartId);

        if (cart == null)
        {
            return NotFound("Cart not found for the user.");
        }

        var product = await _context.products.FindAsync(request.ProductId);
        if (product == null)
        {
            return NotFound("Product not found.");
        }

        var existingCartItem = await _context.cartItems
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == request.ProductId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += 1; // Or any logic for quantity update
            _context.Entry(existingCartItem).State = EntityState.Modified;
        }
        else
        {
            var newCartItem = new CartItem
            {
                CartId = cartId,
                ProductId = request.ProductId,
                Quantity = 1, // Default quantity
                Price = product.Price
            };
            _context.cartItems.Add(newCartItem);
        }

        await _context.SaveChangesAsync();
        return Ok("Product added to cart.");
    }




    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCartItem(int id, [FromBody] CartItem cartItem)
    {
        if (cartItem == null)
        {
            return BadRequest("Cart item was not found");
        }
        _context.Entry(cartItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCartItem(int id)
    {
        var cartItem = await _context.cartItems.FirstOrDefaultAsync(c => c.CartItemId == id);
        if (cartItem == null)
        {
            return NotFound($"Cart item with ID {id} not found.");
        }

        _context.cartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}