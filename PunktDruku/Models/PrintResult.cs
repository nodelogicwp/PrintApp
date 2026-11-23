namespace PunktDruku.Models;

/// <summary>Wynik obliczeń: liczba zadrukowanych stron, arkuszy papieru i koszt w zł.</summary>
public sealed record PrintResult(int PrintedPages, int Sheets, decimal Cost);
