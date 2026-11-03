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

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddBatchWindow { Owner = this };
        if (addWindow.ShowDialog() == true)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.RefreshCommand.Execute(null);
            }
        }
    }
}