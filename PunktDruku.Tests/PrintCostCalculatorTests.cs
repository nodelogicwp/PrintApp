using PunktDruku.Models;
using PunktDruku.Services;

namespace PunktDruku.Tests;

public class PrintCostCalculatorTests
{
    [Fact]
    public void Example_7Pages_3Copies_BlackAndWhite_Duplex()
    {
        PrintResult result = PrintCostCalculator.Calculate(7, 3, PrintMode.BlackAndWhite, isDuplex: true);

        Assert.Equal(21, result.PrintedPages);
        Assert.Equal(12, result.Sheets);
        Assert.Equal(5.25m, result.Cost);
        Assert.Equal("5,25 zł", PrintCostCalculator.FormatCost(result.Cost));
    }

    [Fact]
    public void Example_7Pages_3Copies_BlackAndWhite_SingleSided()
    {
        PrintResult result = PrintCostCalculator.Calculate(7, 3, PrintMode.BlackAndWhite, isDuplex: false);

        Assert.Equal(21, result.PrintedPages);
        Assert.Equal(21, result.Sheets);
        Assert.Equal("5,25 zł", PrintCostCalculator.FormatCost(result.Cost));
    }

    [Fact]
    public void Maximum_500Pages_100Copies_Color_Duplex_HasNoThousandsSeparator()
    {
        PrintResult result = PrintCostCalculator.Calculate(500, 100, PrintMode.Color, isDuplex: true);

        Assert.Equal(50000, result.PrintedPages);
        Assert.Equal(25000, result.Sheets);
        Assert.Equal(50000m, result.Cost);
        Assert.Equal("50000,00 zł", PrintCostCalculator.FormatCost(result.Cost));
    }

    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(2, 1, 1)]
    [InlineData(3, 1, 2)]
    [InlineData(1, 5, 5)]   // każda kopia na nowym arkuszu
    [InlineData(5, 2, 6)]   // 3 arkusze na kopię × 2
    public void Duplex_RoundsSheetsPerCopyUp(int pages, int copies, int expectedSheets)
    {
        PrintResult result = PrintCostCalculator.Calculate(pages, copies, PrintMode.BlackAndWhite, isDuplex: true);

        Assert.Equal(expectedSheets, result.Sheets);
    }

    [Fact]
    public void Duplex_DoesNotChangeCost()
    {
        decimal single = PrintCostCalculator.Calculate(9, 4, PrintMode.Color, false).Cost;
        decimal duplex = PrintCostCalculator.Calculate(9, 4, PrintMode.Color, true).Cost;

        Assert.Equal(single, duplex);
        Assert.Equal("36,00 zł", PrintCostCalculator.FormatCost(duplex));
    }

    [Fact]
    public void Prices_MatchPriceList()
    {
        Assert.Equal(0.25m, PrintCostCalculator.GetPagePrice(PrintMode.BlackAndWhite));
        Assert.Equal(1.00m, PrintCostCalculator.GetPagePrice(PrintMode.Color));
    }
}
