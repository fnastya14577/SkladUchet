using System.Windows;
using SkladUchet.ViewModels;

namespace SkladUchet;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}