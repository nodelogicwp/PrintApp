using PunktDruku.ViewModels;

namespace PunktDruku.Tests;

/// <summary>
/// Testy ViewModelu odpowiadające scenariuszom 1-8 z instrukcji.
/// </summary>
public class MainViewModelTests
{
    private static void AssertInitialState(MainViewModel vm)
    {
        Assert.Equal("-", vm.PrintedPagesText);
        Assert.Equal("-", vm.SheetsText);
        Assert.Equal("-", vm.CostText);
        Assert.Equal("Uzupełnij dane i wybierz OBLICZ.", vm.StatusText);
        Assert.False(vm.IsError);
    }

    private static MainViewModel CreateCalculated()
    {
        var vm = new MainViewModel { PagesText = "7", CopiesText = "3", IsDuplex = true };
        vm.CalculateCommand.Execute(null);
        Assert.Equal("Obliczenie zakończone.", vm.StatusText);
        return vm;
    }

    [Fact] // Scenariusz 1
    public void FirstLaunch_ShowsInitialState()
    {
        var vm = new MainViewModel();

        Assert.Equal(string.Empty, vm.PagesText);
        Assert.Equal(string.Empty, vm.CopiesText);
        Assert.Equal(0, vm.SelectedPrintModeIndex);
        Assert.Equal("Czarno-biały", vm.PrintModeNames[vm.SelectedPrintModeIndex]);
        Assert.False(vm.IsDuplex);
        AssertInitialState(vm);
    }

    [Fact] // Scenariusz 2
    public void Calculate_7Pages_3Copies_Duplex()
    {
        var vm = CreateCalculated();

        Assert.Equal("21", vm.PrintedPagesText);
        Assert.Equal("12", vm.SheetsText);
        Assert.Equal("5,25 zł", vm.CostText);
        Assert.False(vm.IsError);
        Assert.Equal("7", vm.PagesText); // formularz bez zmian
        Assert.Equal("3", vm.CopiesText);
        Assert.True(vm.IsDuplex);
    }

    [Fact] // Scenariusz 3
    public void Calculate_SameData_SingleSided()
    {
        var vm = CreateCalculated();
        vm.IsDuplex = false;
        vm.CalculateCommand.Execute(null);

        Assert.Equal("21", vm.PrintedPagesText);
        Assert.Equal("21", vm.SheetsText);
        Assert.Equal("5,25 zł", vm.CostText);
    }

    [Fact] // Scenariusz 4
    public void Calculate_Maximum_Color_Duplex()
    {
        var vm = new MainViewModel { PagesText = "500", CopiesText = "100", SelectedPrintModeIndex = 1, IsDuplex = true };
        vm.CalculateCommand.Execute(null);

        Assert.Equal("50000", vm.PrintedPagesText);
        Assert.Equal("25000", vm.SheetsText);
        Assert.Equal("50000,00 zł", vm.CostText);
    }

    [Fact] // Scenariusz 5
    public void Calculate_BothEmpty_ShowsPagesErrorFirst()
    {
        var vm = new MainViewModel();
        vm.CalculateCommand.Execute(null);

        Assert.True(vm.IsError);
        Assert.Equal("Podaj liczbę stron od 1 do 500.", vm.StatusText);
        Assert.Equal("-", vm.PrintedPagesText);
        Assert.Equal("-", vm.SheetsText);
        Assert.Equal("-", vm.CostText);
    }

    [Fact] // Scenariusz 6
    public void Calculate_ZeroCopies_ShowsCopiesError_AndKeepsForm()
    {
        var vm = CreateCalculated();
        vm.CopiesText = "0";
        vm.SelectedPrintModeIndex = 1;
        vm.CalculateCommand.Execute(null);

        Assert.True(vm.IsError);
        Assert.Equal("Podaj liczbę kopii od 1 do 100.", vm.StatusText);
        Assert.Equal("-", vm.CostText);
        Assert.Equal("7", vm.PagesText);
        Assert.Equal("0", vm.CopiesText);
        Assert.Equal(1, vm.SelectedPrintModeIndex);
        Assert.True(vm.IsDuplex);
    }

    [Theory] // Scenariusz 7
    [InlineData("pages")]
    [InlineData("copies")]
    [InlineData("mode")]
    [InlineData("duplex")]
    public void ChangingAnyFormElement_ResetsResult(string element)
    {
        var vm = CreateCalculated();

        switch (element)
        {
            case "pages": vm.PagesText = "8"; break;
            case "copies": vm.CopiesText = "4"; break;
            case "mode": vm.SelectedPrintModeIndex = 1; break;
            case "duplex": vm.IsDuplex = false; break;
        }

        AssertInitialState(vm);
    }

    [Fact] // Scenariusz 7 - zmiana po błędzie usuwa komunikat błędu
    public void ChangingForm_AfterError_RemovesErrorMessage()
    {
        var vm = new MainViewModel();
        vm.CalculateCommand.Execute(null);
        Assert.True(vm.IsError);

        vm.PagesText = "5";

        AssertInitialState(vm);
    }

    [Fact] // Scenariusz 8a
    public void Clear_AfterCalculation_RestoresInitialState()
    {
        var vm = CreateCalculated();
        vm.SelectedPrintModeIndex = 1;
        vm.CalculateCommand.Execute(null);

        vm.ClearCommand.Execute(null);

        Assert.Equal(string.Empty, vm.PagesText);
        Assert.Equal(string.Empty, vm.CopiesText);
        Assert.Equal(0, vm.SelectedPrintModeIndex);
        Assert.False(vm.IsDuplex);
        AssertInitialState(vm);
    }

    [Fact] // Scenariusz 8b
    public void Clear_AfterValidationError_RestoresInitialState()
    {
        var vm = new MainViewModel();
        vm.CalculateCommand.Execute(null);
        Assert.True(vm.IsError);

        vm.ClearCommand.Execute(null);

        Assert.Equal(string.Empty, vm.PagesText);
        Assert.Equal(string.Empty, vm.CopiesText);
        AssertInitialState(vm);
    }
}
