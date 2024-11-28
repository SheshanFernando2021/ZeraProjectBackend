using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeraAPI.ZeraAPI.Data;
using ZeraAPI.ZeraAPI.Model;

namespace ZeraAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class WishListitemController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public WishListitemController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/wishListitem/all
    [HttpGet("all")]
    public async Task<IActionResult> GetAll() {
        return Ok(await _context.wishListItems.ToListAsync());
    }

    // GET: api/wishListitem/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) {
        var wishListItem = await _context.wishListItems.FindAsync(id);
        if (wishListItem == null) {
            return NotFound("Wishlist item not found.");
        }
        return Ok(wishListItem);
    }
    // POST: api/wishListitem/create
    [HttpPost("create")]
public async Task<IActionResult> Create([FromBody] WishListItem wishListItem) {
    if (wishListItem == null) {
        return BadRequest("Invalid wishlist item.");
    }

    // Check if the ProductId exists in the Products table
    var productExists = await _context.products.AnyAsync(p => p.ProductId == wishListItem.ProductId);
    if (!productExists) {
        return BadRequest("Product not found.");
    }

    _context.wishListItems.Add(wishListItem);
    await _context.SaveChangesAsync();
    return CreatedAtAction(nameof(GetById), new { id = wishListItem.WishListItemId }, wishListItem);
}

    // PUT: api/wishListitem/update
    [HttpPut("update")]
    public async Task<IActionResult> Update(int id, [FromBody] WishListItem wishListItem) {
        if (id!= wishListItem.WishListItemId) {
            return BadRequest("Wishlist itemId mismatch. ");
        }
        var existingWishListItem = await _context.wishListItems.FindAsync(id);
        if (existingWishListItem == null) {
            throw new Exception("Not found");
        }
        existingWishListItem.ProductId = wishListItem.ProductId;
        await _context.SaveChangesAsync();
        return Ok("Updated Successfully");
    }
    // DELETE: api/wishListitem/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        var wishListItem = await _context.wishListItems.FindAsync(id);
        if(wishListItem == null){
            throw new Exception("Not found");
        }
        _context.wishListItems.Remove(wishListItem);
        await _context.SaveChangesAsync();
        return Ok("Deleted Successfully");
    }

}
