using System.Globalization;
using System.Windows.Input;
using PunktDruku.Models;
using PunktDruku.Services;

namespace PunktDruku.ViewModels;

/// <summary>
/// ViewModel ekranu głównego: przechowuje stan formularza i wyników.
/// </summary>
public class MainViewModel : BaseViewModel
{
    public const string EmptyResult = "-";
    public const string InitialStatus = "Uzupełnij dane i wybierz OBLICZ.";
    public const string SuccessStatus = "Obliczenie zakończone.";

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
        CalculateCommand = new Command(Calculate);
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

    /// <summary>Obsługa przycisku OBLICZ: odczyt danych, obliczenia i prezentacja wyniku.</summary>
    private void Calculate()
    {
        int pages = int.Parse(PagesText.Trim(), CultureInfo.InvariantCulture);
        int copies = int.Parse(CopiesText.Trim(), CultureInfo.InvariantCulture);
        var mode = (PrintMode)SelectedPrintModeIndex;

        PrintResult result = PrintCostCalculator.Calculate(pages, copies, mode, IsDuplex);
        ShowResult(result);
    }

    private void ShowResult(PrintResult result)
    {
        PrintedPagesText = result.PrintedPages.ToString(CultureInfo.InvariantCulture);
        SheetsText = result.Sheets.ToString(CultureInfo.InvariantCulture);
        CostText = PrintCostCalculator.FormatCost(result.Cost);
        IsError = false;
        StatusText = SuccessStatus;
    }
}
