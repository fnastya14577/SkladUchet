using System;
using System.Linq;
using System.Windows;
using SkladUchet.Data;
using SkladUchet.Models;

namespace SkladUchet;

public partial class AddBatchWindow : Window
{
    public AddBatchWindow()
    {
        InitializeComponent();
        LoadProducts();
    }

    private void LoadProducts()
    {
        using var db = new AppDbContext();
        CmbProducts.ItemsSource = db.Products.ToList();
        if (CmbProducts.Items.Count > 0) CmbProducts.SelectedIndex = 0;
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (CmbProducts.SelectedItem is not Product product)
        {
            MessageBox.Show("Выберите товар!"); return;
        }
        if (!int.TryParse(TxtQuantity.Text, out int qty) || qty <= 0)
        {
            MessageBox.Show("Введите корректное количество!"); return;
        }
        if (DpExpiryDate.SelectedDate == null)
        {
            MessageBox.Show("Выберите дату окончания срока годности!"); return;
        }

        using var db = new AppDbContext();
        var newBatch = new Batch
        {
            ProductId = product.Id,
            BatchNumber = string.IsNullOrWhiteSpace(TxtBatchNumber.Text) ? "Б/Н" : TxtBatchNumber.Text,
            Quantity = qty,
            Remaining = qty,
            ReceiptDate = DateTime.Today,
            ExpiryDate = DpExpiryDate.SelectedDate.Value
        };
        newBatch.CalculateStatus();

        db.Batches.Add(newBatch);
        db.SaveChanges();

        DialogResult = true;
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}