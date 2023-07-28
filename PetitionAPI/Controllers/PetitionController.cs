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
        Random rnd = new Random();
        int newId = rnd.Next();
        while (GetPetition(newId).Result.Value != null)
        {
            newId = rnd.Next();
        }
        petition.Id = newId;
      
        petition.EntryDate = DateTime.UtcNow;
        _context.Petitions.Add(petition);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPetition), new { id = petition.Id }, petition);
    }


    // POST to merge petitions
    [HttpPost("merge")]
    public async Task<ActionResult<Petition>> MergePetitions([FromBody] List<Petition> petitions, [FromQuery] string newName)
    {
        var mergedDescription = string.Join("\n\n", petitions.Select(p => p.Description));

        var mergedPetition = new Petition
        {
            Name = newName,
            Description = mergedDescription,
            // ... other properties ...
        };

        await PostPetition(mergedPetition);

        foreach (var petition in petitions)
        {
            await DeletePetition(petition.Id);
        }

        return Ok(mergedPetition);
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