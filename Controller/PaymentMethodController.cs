using Microsoft.AspNetCore.Mvc;
using ZeraAPI.ZeraAPI.Model;
using ZeraAPI.ZeraAPI.Data;
using ZeraAPI.Model;
namespace ZeraAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class PaymentMethodController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PaymentMethodController(ApplicationDbContext context)
    {
        _context = context;
    }
    // POST: api/paymentmethod/create
    [HttpPost("create")]
    public async Task<IActionResult> CreatePaymentMethod(PaymentMethod paymentMethod)
    {
        if (paymentMethod == null)
        {
            return BadRequest("Invalid payment method details.");
        }

        _context.PaymentMethods.Add(paymentMethod);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPaymentMethodById), new { id = paymentMethod.PaymentMethodId }, paymentMethod);
    }

    // GET: api/paymentmethod/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaymentMethodById(int id)
    {
        var paymentMethod = await _context.PaymentMethods.FindAsync(id);
        if (paymentMethod == null)
        {
            return NotFound("Payment method not found.");
        }
        return Ok(paymentMethod);
    }
}