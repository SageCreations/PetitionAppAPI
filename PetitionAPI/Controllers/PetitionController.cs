using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetitionAPI.Models;

namespace PetitionAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PetitionController : ControllerBase
{
    private readonly MyDbContext _context;

    public PetitionController(MyDbContext context)
    {
        _context = context;
    }
    
    // GET all posts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Petition>>> GetPosts()
    {
        return await _context.Petitions.ToListAsync();
    }
    
    // GET by Id
    [HttpGet("{id}")]
    public async Task<ActionResult<Petition>> GetPetition(int id)
    {
        var petition = await _context.Petitions.FindAsync(id);

        if (petition == null)
        {
            return NotFound();
        }
        
        return petition;
    }
    
    // POST entry submission
    [HttpPost]
    public async Task<ActionResult<Petition>> PostPetition(Petition petition)
    {
        petition.EntryDate = DateTime.UtcNow;
        _context.Petitions.Add(petition);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPetition), new { id = petition.Id }, petition);
    }
    
    // DELETE by specific Id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePetition(int id)
    {
        var petition = await _context.Petitions.FindAsync(id);
        if (petition == null)
        {
            return NotFound();
        }

        _context.Petitions.Remove(petition);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}