using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeraAPI.ZeraAPI.Data;
using ZeraAPI.ZeraAPI.Model;

namespace ZeraAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class OrderItemController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrderItemController(ApplicationDbContext context)
    {
        _context = context;
    }

    //GET: api/orderitem/getall
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.orderItems.ToListAsync());
    }
    //GET: api/orderitem/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _context.orderItems.FindAsync(id);
        if (item == null)
        {
            return NotFound("Order item not found.");
        }
        return Ok(item);
    }
    // POST: api/orderitem/create
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] OrderItem orderItem)
    {
        if (orderItem == null)
        {
            return BadRequest("Invalid order item");
        }
        _context.orderItems.Add(orderItem);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = orderItem.OrderItemId }, orderItem);
    }

    //PUT: api/orderitem/update
    [HttpPut("update")]
    public async Task<IActionResult> Update(int id, [FromBody] OrderItem orderItem)
    {
        if (id != orderItem.OrderItemId)
        {
            return BadRequest("OrderItemId mismatch. ");
        }
        var existingOrderItem = await _context.orderItems.FindAsync(id);
        if (existingOrderItem == null)
        {
            throw new Exception("Not found");
        }
        existingOrderItem.OrderId = orderItem.OrderId;
        existingOrderItem.Quantity = orderItem.Quantity;
        existingOrderItem.Price = orderItem.Price;
        await _context.SaveChangesAsync();
        return Ok("Updated Successfully");
    }

    // DELETE: api/orderitem/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var orderItem = await _context.orderItems.FindAsync(id);
        if (orderItem == null)
        {
            throw new Exception("Not found");
        }
        _context.orderItems.Remove(orderItem);
        await _context.SaveChangesAsync();
        return Ok("Deleted Successfully");
    }
}