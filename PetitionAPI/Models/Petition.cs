namespace PetitionAPI.Models;

public class Petition
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime EntryDate { get; set; }
}