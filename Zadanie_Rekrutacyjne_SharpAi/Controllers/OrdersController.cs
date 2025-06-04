using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zadanie_Rekrutacyjne_SharpAi.Models;
using Zadanie_Rekrutacyjne_SharpAi.Data;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ApiDbContext _context;

    public OrderController(ApiDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll() =>
        await _context.Orders
            .Include(o => o.Products)
            .ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> Get(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Products)
            .FirstOrDefaultAsync(o => o.Id == id);

        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> Create(CreateOrderDto dto)
    {
        var products = await _context.Products
            .Where(p => dto.ProductIds.Contains(p.Id))
            .ToListAsync();

        if (products.Count != dto.ProductIds.Count)
        {
            return BadRequest("Some of the products don't exist.");
        }

        var order = new Order
        {
            Products = products
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateOrderDto dto)
    {
        var order = await _context.Orders
            .Include(o => o.Products)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();

        var products = await _context.Products
            .Where(p => dto.ProductIds.Contains(p.Id))
            .ToListAsync();

        order.Products = products;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var zamowienie = await _context.Orders.FindAsync(id);
        if (zamowienie is null) return NotFound();
        _context.Orders.Remove(zamowienie);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
