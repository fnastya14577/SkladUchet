namespace SkladUchet.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public string Unit { get; set; } = "шт";
    public string? Manufacturer { get; set; }
    public int MinShelfLifeDays { get; set; } = 7;
    public decimal Price { get; set; }
    public string? Barcode { get; set; }
    public string? Description { get; set; }
    public List<Batch> Batches { get; set; } = new();
}