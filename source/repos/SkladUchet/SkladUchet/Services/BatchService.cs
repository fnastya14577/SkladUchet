using Microsoft.EntityFrameworkCore;
using SkladUchet.Data;

namespace SkladUchet.Services;

public static class BatchService
{
    public static void WriteOff(int batchId, int qty, int userId, string reason)
    {
        using var db = new AppDbContext();
        var batch = db.Batches.FirstOrDefault(b => b.Id == batchId)
                    ?? throw new InvalidOperationException("Партия не найдена");
        if (qty <= 0 || qty > batch.Remaining)
            throw new InvalidOperationException("Некорректное количество");

        batch.Remaining -= qty;
        db.WriteOffs.Add(new Models.WriteOff
        {
            BatchId = batchId,
            UserId = userId,
            Quantity = qty,
            Reason = reason
        });
        db.SaveChanges();
    }
}