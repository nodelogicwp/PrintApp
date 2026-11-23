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
        ClearCommand = new Command(Clear);
    }

    /// <summary>Pozycje kontrolki Picker w kolejności z wymagań.</summary>
    public IReadOnlyList<string> PrintModeNames { get; } = new[] { "Czarno-biały", "Kolorowy" };

    public string PagesText
    {
        get => _pagesText;
        set
        {
            if (SetProperty(ref _pagesText, value ?? string.Empty))
            {
                ResetToInitialState();
            }
        }
    }

    public string CopiesText
    {
        get => _copiesText;
        set
        {
            if (SetProperty(ref _copiesText, value ?? string.Empty))
            {
                ResetToInitialState();
            }
        }
    }

    public int SelectedPrintModeIndex
    {
        get => _selectedPrintModeIndex;
        set
        {
            // Picker może chwilowo zgłosić -1 (brak wyboru) - ignorujemy taką wartość.
            if (value >= 0 && SetProperty(ref _selectedPrintModeIndex, value))
            {
                ResetToInitialState();
            }
        }
    }

    public bool IsDuplex
    {
        get => _isDuplex;
        set
        {
            if (SetProperty(ref _isDuplex, value))
            {
                ResetToInitialState();
            }
        }
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
        // Kolejność walidacji: najpierw strony, potem kopie - pokazujemy tylko pierwszy błąd.
        if (!InputValidator.TryParsePages(PagesText, out int pages))
        {
            ShowError(InputValidator.InvalidPagesMessage);
            return;
        }

        if (!InputValidator.TryParseCopies(CopiesText, out int copies))
        {
            ShowError(InputValidator.InvalidCopiesMessage);
            return;
        }

        var mode = SelectedPrintModeIndex == (int)PrintMode.Color ? PrintMode.Color : PrintMode.BlackAndWhite;

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

    /// <summary>
    /// Obsługa przycisku WYCZYŚĆ: puste pola, druk czarno-biały, druk jednostronny,
    /// wyniki "-" i status początkowy.
    /// </summary>
    private void Clear()
    {
        PagesText = string.Empty;
        CopiesText = string.Empty;
        SelectedPrintModeIndex = (int)PrintMode.BlackAndWhite;
        IsDuplex = false;
        ResetToInitialState();
    }

    /// <summary>
    /// Wywoływana po każdej zmianie formularza (pola, Picker, Switch):
    /// usuwa poprzedni wynik oraz komunikat błędu. Nowe obliczenie dopiero po OBLICZ.
    /// </summary>
    private void ResetToInitialState()
    {
        ClearResults();
        IsError = false;
        StatusText = InitialStatus;
    }

    /// <summary>Błąd walidacji: wyniki "-", komunikat w kolorze błędu, formularz bez zmian.</summary>
    private void ShowError(string message)
    {
        ClearResults();
        IsError = true;
        StatusText = message;
    }

    private void ClearResults()
    {
        PrintedPagesText = EmptyResult;
        SheetsText = EmptyResult;
        CostText = EmptyResult;
    }
}
