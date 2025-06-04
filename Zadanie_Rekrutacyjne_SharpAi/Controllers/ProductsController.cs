using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zadanie_Rekrutacyjne_SharpAi.Data;
using Zadanie_Rekrutacyjne_SharpAi.Models;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ApiDbContext _context;

    public ProductsController(ApiDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll() =>
        await _context.Products.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> Get(int id)
    {
        var produkt = await _context.Products.FindAsync(id);
        return produkt is null ? NotFound() : Ok(produkt);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product produkt)
    {
        _context.Products.Add(produkt);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = produkt.Id }, produkt);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product produkt)
    {
        if (id != produkt.Id) return BadRequest();
        _context.Entry(produkt).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var produkt = await _context.Products.FindAsync(id);
        if (produkt is null) return NotFound();
        _context.Products.Remove(produkt);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
