namespace SkladUchet.Models;

public class Batch
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public string BatchNumber { get; set; } = "";
    public int Quantity { get; set; }
    public int Remaining { get; set; }
    public DateTime ReceiptDate { get; set; } = DateTime.Today;
    public DateTime ExpiryDate { get; set; } = DateTime.Today.AddDays(30);
    public decimal PurchasePrice { get; set; }
    public string Status { get; set; } = "В норме";
    public List<WriteOff> WriteOffs { get; set; } = new();

    public void CalculateStatus(int warningDays = 7)
    {
        var daysLeft = (ExpiryDate.Date - DateTime.Today).Days;
        Status = daysLeft < 0 ? "Просрочено"
               : daysLeft <= warningDays ? "Истекает скоро"
               : "В норме";
    }
}