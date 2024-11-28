using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeraAPI.ZeraAPI.Data;
using ZeraAPI.ZeraAPI.Model;

namespace ZeraAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/order/getall
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.orders.ToListAsync());
        }

        // GET: api/order/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.orders.FindAsync(id);
            if (item == null)
            {
                return NotFound("Order not found.");
            }
            return Ok(item);
        }

        // POST: api/order/create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Invalid order data.");
            }

            // Check that OrderStatus is valid
            if (!Enum.IsDefined(typeof(Order.OrderStatus), order.status))
            {
                return BadRequest("Invalid order status.");
            }

            _context.orders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
        }

        // PUT: api/order/update
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order data is required.");
            }

            var existingOrder = await _context.orders.FindAsync(id);
            if (existingOrder == null)
            {
                return NotFound("Order not found.");
            }

            existingOrder.OrderDate = order.OrderDate;
            existingOrder.ShippingDate = order.ShippingDate;
            existingOrder.ShippingAddress = order.ShippingAddress;
            existingOrder.TotalAmount = order.TotalAmount;
            existingOrder.status = order.status;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/order/delete
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.orders.FindAsync(id);
            if (item == null)
            {
                return NotFound("Order not found.");
            }

            _context.orders.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/order/{id}
        [HttpGet("getOrderById/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.orders
                .Include(o => o.PaymentMethod)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(order);
        }
    }
}