using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace PlaywrightTests;

[TestClass]
public class DemoTest : PageTest
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IBrowserContext _browserContext;
    private IPage _page;

    [TestInitialize]
    public async Task Setup()
    {
        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            SlowMo = 1000 // Lägger in en fördröjning så vi kan se vad som händer
        });
        _browserContext = await _browser.NewContextAsync();
        _page = await _browserContext.NewPageAsync();
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        await _browserContext.CloseAsync();
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    [TestMethod]
    public async Task GetShopLink()
    {
        await _page.GotoAsync("http://localhost:5173/");

        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email.." }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email.." }).FillAsync("super_gris@mail.com");
        await _page.Keyboard.PressAsync("Tab");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password.." }).FillAsync("kung");
        await _page.GetByRole(AriaRole.Button, new() { Name = "login!" }).ClickAsync();

        await _page.GetByRole(AriaRole.Button, new() { Name = "Companies" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "block" }).Nth(1).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Show Inactive Companies" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Activate" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Hide Inactive Companies" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Add Company" }).ClickAsync();

        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Name:" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Name:" }).FillAsync("SvinStian");

        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email:" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email:" }).FillAsync("svinstianstudios@mail.com");
        await _page.Keyboard.PressAsync("Tab");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Phone:" }).FillAsync("07532532626");
        await _page.Keyboard.PressAsync("Tab");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Description:" }).FillAsync("En grisigt lyxig studio med högkvalitativa tråg och skydd mot rovdjur");
        await _page.Keyboard.PressAsync("Tab");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Domain:" }).FillAsync("http://svinstian.com");

        _page.Dialog += async (_, dialog) =>
        {
            Console.WriteLine($"Dialog message: {dialog.Message}");
            await dialog.DismissAsync();
        };

        await _page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "⬅️ Back" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "delete" }).Nth(4).ClickAsync();
    }
}

