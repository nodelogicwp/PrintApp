using System.Globalization;
using PunktDruku.Models;

namespace PunktDruku.Services;

/// <summary>
/// Oblicza koszt wydruku oraz liczbę zadrukowanych stron i arkuszy papieru.
/// Klasa nie zna kontrolek interfejsu - przyjmuje liczby i zwraca wynik.
/// </summary>
public static class PrintCostCalculator
{
    /// <summary>Cena jednej zadrukowanej strony czarno-białej (zł).</summary>
    public const decimal BlackAndWhitePagePrice = 0.25m;

    /// <summary>Cena jednej zadrukowanej strony kolorowej (zł).</summary>
    public const decimal ColorPagePrice = 1.00m;

    // Format kwoty: przecinek dziesiętny, bez separatorów tysięcy, np. 5,25 zł.
    private static readonly NumberFormatInfo CostFormat = new()
    {
        NumberDecimalSeparator = ",",
        NumberGroupSeparator = string.Empty
    };

    /// <summary>Zwraca stawkę za jedną zadrukowaną stronę dla danego trybu.</summary>
    public static decimal GetPagePrice(PrintMode mode) => mode switch
    {
        PrintMode.BlackAndWhite => BlackAndWhitePagePrice,
        PrintMode.Color => ColorPagePrice,
        _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Nieznany tryb druku.")
    };

    /// <summary>
    /// Wykonuje obliczenia dla poprawnych danych:
    /// zadrukowane strony = strony × kopie;
    /// arkusze jednej kopii = strony (jednostronnie) lub ⌈strony / 2⌉ (dwustronnie),
    /// a każda kopia zaczyna się na nowym arkuszu, więc arkusze = arkusze kopii × kopie;
    /// koszt = zadrukowane strony × stawka (druk dwustronny nie zmienia kosztu).
    /// </summary>
    public static PrintResult Calculate(int pages, int copies, PrintMode mode, bool isDuplex)
    {
        if (pages <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pages), pages, "Liczba stron musi być dodatnia.");
        }

        if (copies <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(copies), copies, "Liczba kopii musi być dodatnia.");
        }

        int printedPages = pages * copies;
        int sheetsPerCopy = isDuplex ? (pages + 1) / 2 : pages;
        int sheets = sheetsPerCopy * copies;
        decimal cost = printedPages * GetPagePrice(mode);

        return new PrintResult(printedPages, sheets, cost);
    }

    /// <summary>Formatuje kwotę, np. 5.25m → "5,25 zł".</summary>
    public static string FormatCost(decimal cost) =>
        cost.ToString("0.00", CostFormat) + " zł";
}
