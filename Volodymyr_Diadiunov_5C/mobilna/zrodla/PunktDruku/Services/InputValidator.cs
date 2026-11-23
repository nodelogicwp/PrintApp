using System.Globalization;

namespace PunktDruku.Services;

/// <summary>
/// Bezpieczne parsowanie liczb wpisanych w polach formularza.
/// </summary>
public static class InputValidator
{
    public const int MinPages = 1;
    public const int MaxPages = 500;
    public const int MinCopies = 1;
    public const int MaxCopies = 100;

    public const string InvalidPagesMessage = "Podaj liczbę stron od 1 do 500.";
    public const string InvalidCopiesMessage = "Podaj liczbę kopii od 1 do 100.";

    /// <summary>
    /// Próbuje odczytać liczbę całkowitą z zakresu [min, max].
    /// Po usunięciu białych znaków z początku i końca tekst może zawierać wyłącznie cyfry 0-9
    /// (zera wiodące są dozwolone). Odrzucany jest pusty tekst, znaki + i -, ułamki, litery,
    /// spacje wewnętrzne oraz liczby spoza zakresu. Metoda nigdy nie zgłasza wyjątku.
    /// </summary>
    public static bool TryParseInRange(string? text, int min, int max, out int value)
    {
        value = 0;

        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        string trimmed = text.Trim();

        foreach (char character in trimmed)
        {
            // Tylko cyfry ASCII - char.IsDigit akceptowałby także cyfry z innych alfabetów.
            if (character < '0' || character > '9')
            {
                return false;
            }
        }

        // Usuwamy zera wiodące, aby bardzo długi zapis typu 000…0007 nie powodował przepełnienia.
        string significantDigits = trimmed.TrimStart('0');
        if (significantDigits.Length == 0)
        {
            significantDigits = "0";
        }

        if (significantDigits.Length > 9 ||
            !int.TryParse(significantDigits, NumberStyles.None, CultureInfo.InvariantCulture, out int parsed))
        {
            return false;
        }

        if (parsed < min || parsed > max)
        {
            return false;
        }

        value = parsed;
        return true;
    }

    /// <summary>Waliduje liczbę stron (1-500).</summary>
    public static bool TryParsePages(string? text, out int pages) =>
        TryParseInRange(text, MinPages, MaxPages, out pages);

    /// <summary>Waliduje liczbę kopii (1-100).</summary>
    public static bool TryParseCopies(string? text, out int copies) =>
        TryParseInRange(text, MinCopies, MaxCopies, out copies);
}
