using PunktDruku.ViewModels;

namespace PunktDruku;

/// <summary>
/// Jedyny ekran aplikacji. Logika znajduje się w MainViewModel (wzorzec MVVM).
/// </summary>
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}
