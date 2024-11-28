using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeraAPI.ZeraAPI.Data;
using ZeraAPI.ZeraAPI.Model;

namespace ZeraAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }
    // GET: api/Product/all
    [HttpGet("all")]
    public async Task<IActionResult> getAllProducts()
    {
        var productExists = await _context.products.AnyAsync();
        if (!productExists)
        {
            return NotFound("No products found.");
        }
        return Ok(await _context.products.ToListAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _context.products.FindAsync(id);
        if (product == null)
        {
            return NotFound("Product not found.");
        }
        return Ok(product);
    }
    // GET: api/Product/name
    [HttpGet("name")]
    public async Task<IActionResult> GetProductByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Search term cannot be empty.");
        }
        var product = await _context.products.Where(p => p.ProductName.ToLower().Contains(name.ToLower())).ToListAsync();
        if (product == null || !product.Any())
        {
            return NotFound("Product not found.");
        }
        return Ok(product);
    }
    //POST: api/product/add
    [HttpPost("add")]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        if (product == null)
        {
            return BadRequest("Invalid product");
        }
        var ExistingProduct = await _context.products.AnyAsync(p => p.ProductName.ToLower() == product.ProductName.ToLower());
        if (ExistingProduct)
        {
            return Conflict("A product with the same name already exists. ");
        }
        _context.products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.ProductId)
        {
            return BadRequest("Product ID mismatch. ");
        }
        var existingProdct = await _context.products.FindAsync(id);
        if (existingProdct == null)
        {
            throw new Exception("Not found");
        }
        existingProdct.ProductName = product.ProductName;
        existingProdct.Producttype = product.Producttype;
        existingProdct.Price = product.Price;
        existingProdct.ImageURL = product.ImageURL;
        existingProdct.Description = product.Description;

        await _context.SaveChangesAsync();
        return Ok("Updated Successfully");
    }
    //DELETE: api/product/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.products.FindAsync(id);
        if (product == null)
        {
            throw new Exception("Not found");
        }
        _context.products.Remove(product);
        await _context.SaveChangesAsync();
        return Ok("Deleted Successfully");
    }
}