# Punkt druku – dokumentacja testów

**Autor:** Volodymyr Diadiunov  
**Projekt:** aplikacja mobilna .NET MAUI „Punkt druku” (INF.04)

## Użyte narzędzia

| Element | Wersja / opis |
|---------|---------------|
| System (środowisko, w którym przygotowano i zweryfikowano kod) | Ubuntu 24.04 LTS (x64) |
| IDE / edytor | wiersz poleceń `dotnet` CLI; projekt przygotowany do otwarcia w Visual Studio 2022 (17.8+) |
| .NET SDK | 8.0.134 |
| .NET MAUI (Microsoft.Maui.Controls) | 8.0.100 |
| Framework testów | xUnit 2.9.2, Microsoft.NET.Test.Sdk 17.11.1 |
| Platforma docelowa | Android (`net8.0-android`, min. API 21) |
| Emulator / urządzenie | **nie użyto** – patrz „Stan weryfikacji” |

## Stan weryfikacji

- Kod C# oraz XAML (kompilator XamlC z kompilowanymi powiązaniami `x:DataType`) kompilują się
  bez błędów i ostrzeżeń względem Microsoft.Maui.Controls 8.0.100.
- Logika aplikacji (obliczenia, walidacja, zmiany stanu formularza) jest sprawdzona automatycznie:
  **49 testów jednostkowych, wszystkie zaliczone** (`dotnet test PunktDruku.Tests`).
- Środowisko, w którym przygotowano projekt, nie miało Android SDK ani akceleracji sprzętowej dla emulatora,
  dlatego **nie zbudowano pakietu APK i nie uruchomiono aplikacji na emulatorze**. Kolumna
  „Wynik zaobserwowany” opisuje wynik testu automatycznego na ViewModelu (te same właściwości, które są
  powiązane z kontrolkami widoku). Zrzuty `mobile1.png`–`mobile4.png` należy wykonać po uruchomieniu
  aplikacji w Visual Studio na emulatorze Android.

## Tabela testów

| Nr | Dane / czynność | Wynik oczekiwany | Wynik zaobserwowany | Ocena |
|----|-----------------|------------------|---------------------|-------|
| 1 | Pierwsze uruchomienie aplikacji. | Puste pola z podpowiedziami 1-500 i 1-100, wybrany „Czarno-biały”, Switch wyłączony, wyniki „-”, status „Uzupełnij dane i wybierz OBLICZ.” | Pola puste, indeks Pickera 0 („Czarno-biały”), IsDuplex = false, wyniki „-”, status początkowy (test `FirstLaunch_ShowsInitialState`). | Zgodny |
| 2 | 7 stron, 3 kopie, czarno-biały, druk dwustronny, OBLICZ. | Zadrukowane strony: 21, Arkusze papieru: 12, Koszt: 5,25 zł, status „Obliczenie zakończone.”, dane formularza bez zmian. | 21 / 12 / „5,25 zł”, status „Obliczenie zakończone.”, pola 7 i 3 oraz Switch bez zmian (`Calculate_7Pages_3Copies_Duplex`). | Zgodny |
| 3 | Dane z próby 2, druk jednostronny, ponownie OBLICZ. | Strony: 21, Arkusze: 21, Koszt: 5,25 zł. | Po wyłączeniu Switcha wynik skasowany; po OBLICZ 21 / 21 / „5,25 zł” (`Calculate_SameData_SingleSided`). | Zgodny |
| 4 | 500 stron, 100 kopii, kolorowy, druk dwustronny, OBLICZ. | Strony: 50000, Arkusze: 25000, Koszt: 50000,00 zł (bez separatora tysięcy). | 50000 / 25000 / „50000,00 zł” (`Calculate_Maximum_Color_Duplex`). | Zgodny |
| 5 | Oba pola puste, OBLICZ. | Tylko komunikat „Podaj liczbę stron od 1 do 500.” w kolorze #B42318, wyniki „-”. | Status „Podaj liczbę stron od 1 do 500.”, IsError = true (kolor błędu), wyniki „-” (`Calculate_BothEmpty_ShowsPagesErrorFirst`). | Zgodny |
| 6 | 7 stron, 0 kopii, OBLICZ. | Komunikat „Podaj liczbę kopii od 1 do 100.” w kolorze błędu, wyniki „-”, zachowane pola, tryb i Switch. | Status „Podaj liczbę kopii od 1 do 100.”, IsError = true, wyniki „-”, pola „7” i „0”, tryb i Switch bez zmian (`Calculate_ZeroCopies_ShowsCopiesError_AndKeepsForm`). | Zgodny |
| 7 | Po poprawnym obliczeniu zmiana: a) liczby stron, b) liczby kopii, c) trybu, d) przełącznika – przed każdą próbą ponowne OBLICZ. | W każdym przypadku wyniki wracają do „-”, status początkowy, brak automatycznego przeliczenia. Zmiana po błędzie usuwa komunikat błędu. | We wszystkich 4 przypadkach wyniki „-” i status początkowy; po błędzie zmiana pola usuwa komunikat (`ChangingAnyFormElement_ResetsResult` ×4, `ChangingForm_AfterError_RemovesErrorMessage`). | Zgodny |
| 8 | WYCZYŚĆ po obliczeniu, a następnie po błędzie walidacji. | Puste pola, „Czarno-biały”, Switch wyłączony, wyniki „-”, status początkowy (w obu przypadkach). | Oba przypadki: pola puste, indeks Pickera 0, IsDuplex = false, wyniki „-”, status początkowy, brak koloru błędu (`Clear_AfterCalculation_…`, `Clear_AfterValidationError_…`). | Zgodny |

### Dodatkowe testy walidacji (`InputValidatorTests`)

Akceptowane: `1`, `500`, `0007`, `"  42  "`, tabulator/nowa linia na brzegach, bardzo długi zapis z zerami wiodącymi.
Odrzucane: pusty tekst, same spacje, `0`, `000`, `501`, `+5`, `-5`, `5.0`, `5,5`, `1e2`, `abc`, `1 0`,
cyfry spoza zakresu 0-9 (np. arabskie), liczba przekraczająca zakres `int`. Kopie: `1` i `100` poprawne, `0` i `101` błędne.

## Zrzuty ekranu

| Plik | Zawartość | Stan |
|------|-----------|------|
| `PunktDruku/1.png` | lokalna konfiguracja Git (user.name, user.email) i `.gitignore` | wykonany |
| `PunktDruku/2.png` | wynik testów jednostkowych `dotnet test` | wykonany |
| `PunktDruku/tagi.png` | lista tagów z adnotacjami | wykonany |
| `mobile1.png` – `mobile4.png` | emulator + IDE: stan początkowy, wynik próby 2, błąd próby 6, stan po WYCZYŚĆ | do wykonania na emulatorze |
