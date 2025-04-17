namespace Server.Tests;
using server;
using System.Text.RegularExpressions;

public class UnitTests
{

    [Theory]
    [InlineData(8)]
    [InlineData(12)]
    [InlineData(20)]
    public void TestGenerateSlugControllingLengthAndAllowedCharacters(int length)
    {
        var slug1 = TicketRoutes.GenerateSlugs(length);
        var slug2 = TicketRoutes.GenerateSlugs(length);

        Assert.NotNull(slug1);
        Assert.NotNull(slug2);
        Assert.Equal(length, slug1.Length);
        Assert.Equal(length, slug2.Length);
        Assert.Matches("^[A-Za-z0-9]+$", slug1);
        Assert.Matches("^[A-Za-z0-9]+$", slug2);
        Assert.NotEqual(slug1, slug2);
    }

    [Theory]
    [InlineData(8)]
    [InlineData(12)]
    public void GeneratePassword_ShouldHaveCorrectLengthAndCharacters(int length)
    {
        var password = UserRoutes.GeneratePassword(length);
        var password2 = UserRoutes.GeneratePassword(length);

        Assert.NotNull(password);
        Assert.NotNull(password2);
        Assert.Equal(length, password.Length);
        Assert.Equal(length, password2.Length);
        Assert.Matches("^[A-Za-z0-9!@#$%^&*()\\-_=+<,>.]+$", password);
        Assert.Matches("^[A-Za-z0-9!@#$%^&*()\\-_=+<,>.]+$", password2);
    }
}
