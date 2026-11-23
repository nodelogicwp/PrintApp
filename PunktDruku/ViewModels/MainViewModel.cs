using System.Windows.Input;

namespace PunktDruku.ViewModels;

/// <summary>
/// ViewModel ekranu głównego: przechowuje stan formularza i wyników.
/// </summary>
public class MainViewModel : BaseViewModel
{
    public const string EmptyResult = "-";
    public const string InitialStatus = "Uzupełnij dane i wybierz OBLICZ.";

    private string _pagesText = string.Empty;
    private string _copiesText = string.Empty;
    private int _selectedPrintModeIndex;
    private bool _isDuplex;
    private string _printedPagesText = EmptyResult;
    private string _sheetsText = EmptyResult;
    private string _costText = EmptyResult;
    private string _statusText = InitialStatus;
    private bool _isError;

    public MainViewModel()
    {
        CalculateCommand = new Command(() => { });
        ClearCommand = new Command(() => { });
    }

    /// <summary>Pozycje kontrolki Picker w kolejności z wymagań.</summary>
    public IReadOnlyList<string> PrintModeNames { get; } = new[] { "Czarno-biały", "Kolorowy" };

    public string PagesText
    {
        get => _pagesText;
        set => SetProperty(ref _pagesText, value ?? string.Empty);
    }

    public string CopiesText
    {
        get => _copiesText;
        set => SetProperty(ref _copiesText, value ?? string.Empty);
    }

    public int SelectedPrintModeIndex
    {
        get => _selectedPrintModeIndex;
        set => SetProperty(ref _selectedPrintModeIndex, value);
    }

    public bool IsDuplex
    {
        get => _isDuplex;
        set => SetProperty(ref _isDuplex, value);
    }

    public string PrintedPagesText
    {
        get => _printedPagesText;
        private set => SetProperty(ref _printedPagesText, value);
    }

    public string SheetsText
    {
        get => _sheetsText;
        private set => SetProperty(ref _sheetsText, value);
    }

    public string CostText
    {
        get => _costText;
        private set => SetProperty(ref _costText, value);
    }

    public string StatusText
    {
        get => _statusText;
        private set => SetProperty(ref _statusText, value);
    }

    /// <summary>True, gdy status jest komunikatem błędu (kolor #B42318).</summary>
    public bool IsError
    {
        get => _isError;
        private set => SetProperty(ref _isError, value);
    }

    public ICommand CalculateCommand { get; }

    public ICommand ClearCommand { get; }
}
