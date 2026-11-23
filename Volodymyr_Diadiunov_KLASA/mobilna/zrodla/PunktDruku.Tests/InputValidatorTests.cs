using PunktDruku.Services;

namespace PunktDruku.Tests;

public class InputValidatorTests
{
    [Theory]
    [InlineData("1", 1)]
    [InlineData("500", 500)]
    [InlineData("7", 7)]
    [InlineData("0007", 7)]
    [InlineData("  42  ", 42)]
    [InlineData("\t12\n", 12)]
    [InlineData("0000000000000000000000000000000500", 500)]
    public void Pages_AcceptsValidValues(string text, int expected)
    {
        Assert.True(InputValidator.TryParsePages(text, out int pages));
        Assert.Equal(expected, pages);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("0")]
    [InlineData("000")]
    [InlineData("501")]
    [InlineData("+5")]
    [InlineData("-5")]
    [InlineData("5.0")]
    [InlineData("5,5")]
    [InlineData("1e2")]
    [InlineData("abc")]
    [InlineData("1 0")]
    [InlineData("٣")]              // cyfra arabska - nie jest cyfrą 0-9
    [InlineData("99999999999999999999")]
    public void Pages_RejectsInvalidValues(string? text)
    {
        Assert.False(InputValidator.TryParsePages(text, out _));
    }

    [Theory]
    [InlineData("1", true)]
    [InlineData("100", true)]
    [InlineData("0", false)]
    [InlineData("101", false)]
    public void Copies_Range(string text, bool expected)
    {
        Assert.Equal(expected, InputValidator.TryParseCopies(text, out _));
    }
}
