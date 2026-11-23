namespace PunktDruku;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Aplikacja zawsze korzysta z jasnego motywu (wymaganie projektu).
        UserAppTheme = AppTheme.Light;

        MainPage = new MainPage();
    }
}
