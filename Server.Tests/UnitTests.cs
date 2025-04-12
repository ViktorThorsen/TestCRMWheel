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
        var slug = TicketRoutes.GenerateSlugs(length);

        Assert.NotNull(slug);
        Assert.Equal(length, slug.Length);
        Assert.Matches("^[A-Za-z0-9]+$", slug);
    }

    [Theory]
    [InlineData(8)]
    [InlineData(12)]
    public void GeneratePassword_ShouldHaveCorrectLengthAndCharacters(int length)
    {
        var password = UserRoutes.GeneratePassword(length);

        Assert.NotNull(password);
        Assert.Equal(length, password.Length);
        Assert.Matches("^[A-Za-z0-9!@#$%^&*()\\-_=+<,>.]+$", password);
    }
}
