using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeraAPI.ZeraAPI.Data;
using ZeraAPI.ZeraAPI.Model;

namespace ZeraAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class WishListController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public WishListController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/wishList/getall
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll() {
        return Ok(await _context.wishLists.ToListAsync());
    }

    // GET: api/wishList/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) {
        var wishList = await _context.wishLists.FindAsync(id);
        if (wishList == null) {
            return NotFound("Wishlist not found.");
        }
        return Ok(wishList);
    }
    // POST: api/wishList/create
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] WhishList wishList) {
        if (wishList == null) {
            return BadRequest("Invalid wishlist.");
        }
        _context.wishLists.Add(wishList);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = wishList.WhishListId }, wishList);
    }
    // PUT: api/wishList/update
    [HttpPut("update")]
    public async Task<IActionResult> Update(int id, [FromBody] WhishList wishList) {
        if (id!= wishList.WhishListId) {
            return BadRequest("WishlistId mismatch. ");

        }
        var existingWishList = await _context.wishLists.FindAsync(id);
        if (existingWishList == null) {
            throw new Exception("Not found");
        }
        existingWishList.Id = wishList.Id;
        existingWishList.CreatedAt = wishList.CreatedAt;
        existingWishList.UpdatedAt = wishList.UpdatedAt;
        await _context.SaveChangesAsync();
        return Ok("Updated Successfully");
    }
    // DELETE: api/wishList/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        var wishList = await _context.wishLists.FindAsync(id);
        if (wishList == null) {
            throw new Exception("Not found");
        }
        _context.wishLists.Remove(wishList);
        await _context.SaveChangesAsync();
        return Ok("Deleted Successfully");
    }
}
