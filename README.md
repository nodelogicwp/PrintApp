# Punkt druku

Aplikacja mobilna .NET MAUI (Android), która oblicza **koszt wydrukowania dokumentu**
oraz **liczbę zadrukowanych stron i arkuszy papieru**. Użytkownik podaje liczbę stron
(1–500) i kopii (1–100), wybiera tryb druku (czarno-biały 0,25 zł / kolorowy 1,00 zł
za stronę) oraz druk jedno- lub dwustronny. Wynik pojawia się na tym samym ekranie.

**Autor:** Volodymyr Diadiunov

## Funkcje

- pola liczby stron i kopii z klawiaturą numeryczną, Picker trybu druku, Switch druku dwustronnego;
- przyciski **OBLICZ** i **WYCZYŚĆ**, stale widoczne podsumowanie oraz komunikat statusu;
- druk dwustronny: maks. 2 strony na arkuszu, każda kopia zaczyna się na nowym arkuszu;
- walidacja danych (tylko cyfry 0–9, zera wiodące dozwolone, zakresy 1–500 i 1–100);
- każda zmiana formularza kasuje poprzedni wynik; kwoty liczone w typie `decimal` (np. `5,25 zł`).

## Architektura (MVVM)

| Plik | Rola |
|------|------|
| `PunktDruku/MainPage.xaml` | Widok – jedna strona `ContentPage` z `ScrollView` i `VerticalStackLayout` |
| `PunktDruku/ViewModels/MainViewModel.cs` | ViewModel – stan formularza, polecenia OBLICZ/WYCZYŚĆ, reset wyników |
| `PunktDruku/ViewModels/BaseViewModel.cs` | Implementacja `INotifyPropertyChanged` |
| `PunktDruku/Services/PrintCostCalculator.cs` | Obliczenia (oddzielone od kontrolek) i formatowanie kwoty |
| `PunktDruku/Services/InputValidator.cs` | Bezpieczne parsowanie i walidacja liczb |
| `PunktDruku/Models/` | `PrintMode`, `PrintResult` |
| `PunktDruku.Tests/` | Testy jednostkowe xUnit (kalkulator, walidator, scenariusze 1–8 na ViewModelu) |
| `DOKUMENTACJA.md` | Tabela testów i użyte narzędzia |

## Wymagania środowiska

- Windows 10/11 (lub macOS) z **Visual Studio 2022 17.8+** i obciążeniem *.NET Multi-platform App UI development*
  albo VS Code z rozszerzeniem *.NET MAUI*;
- **.NET SDK 8.0** z workloadem MAUI: `dotnet workload install maui-android`;
- Android SDK (API 34) oraz emulator Android (np. Pixel, Android 13/14) lub urządzenie z włączonym debugowaniem USB.

## Uruchomienie

**Visual Studio:** otwórz `PunktDruku.sln`, ustaw projekt `PunktDruku` jako startowy,
wybierz cel *Android Emulators → (twój emulator)* i naciśnij **F5**.

**Wiersz poleceń:**

```bash
dotnet build PunktDruku/PunktDruku.csproj -f net8.0-android
dotnet build PunktDruku/PunktDruku.csproj -f net8.0-android -t:Run   # uruchomienie na emulatorze/urządzeniu
```

**Testy jednostkowe:**

```bash
dotnet test PunktDruku.Tests/PunktDruku.Tests.csproj
```

## Wersje (tagi)

| Tag | Etap |
|-----|------|
| `v0.1.0` | Gotowy interfejs aplikacji |
| `v0.2.0` | Działające obliczenia, walidacja i obsługa formularza |
| `v1.0.0` | Ukończona aplikacja wraz z testami i dokumentacją |
