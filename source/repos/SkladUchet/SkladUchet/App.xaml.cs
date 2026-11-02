using System.Windows;
using SkladUchet.Data;

namespace SkladUchet;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        AppDbContext.Init();
        base.OnStartup(e);
    }
}