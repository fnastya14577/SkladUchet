using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using SkladUchet.Data;
using SkladUchet.Models;
using SkladUchet.Services;

namespace SkladUchet.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly AppDbContext _db = new();

    public ObservableCollection<Batch> Batches { get; } = new();
    public ObservableCollection<Product> Products { get; } = new();
    public ObservableCollection<Batch> Alerts { get; } = new();
    public ObservableCollection<WriteOff> WriteOffs { get; } = new();

    private Batch? _selectedBatch;
    public Batch? SelectedBatch
    {
        get => _selectedBatch;
        set
        {
            _selectedBatch = value;
            OnPropertyChanged();
        }
    }

    private string _statusText = "";
    public string StatusText
    {
        get => _statusText;
        set { _statusText = value; OnPropertyChanged(); }
    }

    public RelayCommand RefreshCommand { get; }
    public RelayCommand WriteOffCommand { get; }

    public MainViewModel()
    {
        RefreshCommand = new RelayCommand(_ => LoadData());
        WriteOffCommand = new RelayCommand(_ => DoWriteOff(), _ => SelectedBatch != null);
        LoadData();
    }

    private void LoadData()
    {
        Batches.Clear(); Alerts.Clear(); Products.Clear(); WriteOffs.Clear();

        var batchesFromDb = _db.Batches.Include(b => b.Product).ToList();

        foreach (var b in batchesFromDb)
        {
            b.CalculateStatus(); 
            Batches.Add(b);

            if (b.Remaining > 0 && b.Status != "В норме")
                Alerts.Add(b);
        }

        _db.SaveChanges();

        foreach (var p in _db.Products.ToList())
            Products.Add(p);

        foreach (var w in _db.WriteOffs.Include(w => w.Batch).OrderByDescending(w => w.Date).ToList())
            WriteOffs.Add(w);

        StatusText = $"Товаров: {Products.Count} | Партий: {Batches.Count} | " +
                     $"Требуют внимания: {Alerts.Count}";
    }

    private void DoWriteOff()
    {
        if (SelectedBatch == null) return;

        var input = Microsoft.VisualBasic.Interaction.InputBox(
            $"Сколько списать из партии {SelectedBatch.BatchNumber}? " +
            $"(остаток: {SelectedBatch.Remaining})",
            "Списание товара", "1");

        if (!int.TryParse(input, out var qty) || qty <= 0) return;

        try
        {
            BatchService.WriteOff(SelectedBatch.Id, qty, 1,
                SelectedBatch.Status == "Просрочено" ? "Порча (Просрочка)" : "Продажа/Расход");

            LoadData();
            MessageBox.Show("Списание успешно выполнено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка списания", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? n = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
}