namespace SkladUchet.Models;

public class WriteOff
{
    public int Id { get; set; }
    public int BatchId { get; set; }
    public Batch? Batch { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public string Reason { get; set; } = "";
    public string? Comment { get; set; }
}