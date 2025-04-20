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
            SlowMo = 1000
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
    public async Task SuperAdminTest()
    {
        await _page.GotoAsync("http://localhost:5000/");

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

    [TestMethod]
    public async Task CreateTicketTest()
    {
        await _page.GotoAsync("http://localhost:5000/tech-solutions");

        var iframeLocator = _page.FrameLocator("iframe[title=\"Customer Ticket View\"]");

        await iframeLocator.Locator("select[name='product']").SelectOptionAsync(new[] { "45" });
        await iframeLocator.Locator("select[name='category']").SelectOptionAsync(new[] { "2" });

        await iframeLocator.GetByRole(AriaRole.Textbox, new() { Name = "Enter your email.." }).ClickAsync();
        await iframeLocator.GetByRole(AriaRole.Textbox, new() { Name = "Enter your email.." }).FillAsync("gristoffer@mail.com");
        await iframeLocator.GetByRole(AriaRole.Textbox, new() { Name = "Enter your email.." }).PressAsync("Tab");

        await iframeLocator.GetByRole(AriaRole.Textbox, new() { Name = "Enter title.." }).FillAsync("Grymt");
        await iframeLocator.GetByRole(AriaRole.Textbox, new() { Name = "Enter title.." }).PressAsync("Tab");
        await iframeLocator.GetByRole(AriaRole.Textbox, new() { Name = "Enter message.." }).FillAsync("Denna produkten var faktiskt svin bra!");

        _page.Dialog += async (_, dialog) =>
        {
            Console.WriteLine($"Dialog message: {dialog.Message}");
            await dialog.DismissAsync();
        };

        await iframeLocator.GetByRole(AriaRole.Button, new() { Name = "Create Ticket" }).ClickAsync();
    }

    [TestMethod]
    public async Task CustomerChatAndAgentTest()
    {
        await _page.GotoAsync("http://localhost:5000/customer/84VkrukvUExlwPZI/chat");
        await _page.GetByRole(AriaRole.Textbox).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox).FillAsync("Trevligt,Trevligt");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Send" }).ClickAsync();

        await _page.GotoAsync("http://localhost:5000");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email.." }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email.." }).FillAsync("tryne@hotmail.com");
        await _page.Keyboard.PressAsync("Tab");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password.." }).FillAsync("asd123");
        await _page.GetByRole(AriaRole.Button, new() { Name = "login!" }).ClickAsync();

        await _page.GetByRole(AriaRole.Link, new() { Name = "Grymt Ticket id:" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox).FillAsync("Ja det är en synnerligen grisig produkt!");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Send" }).ClickAsync();

        await _page.GotoAsync("http://localhost:5000/customer/84VkrukvUExlwPZI/chat");
        await _page.GetByRole(AriaRole.Textbox).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox).FillAsync("Jag menar det!");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Send" }).ClickAsync();
    }

    [TestMethod]
    public async Task AdminAddProductAndAddAgentTest()
    {
        _page.Dialog += async (sender, dialog) =>
{
    Console.WriteLine($"Dialog message: {dialog.Message}");
    await dialog.DismissAsync();
};
        await _page.GotoAsync("http://localhost:5000");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email.." }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email.." }).FillAsync("grune@grymt.se");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password.." }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password.." }).FillAsync("hejhej");
        await _page.GetByRole(AriaRole.Button, new() { Name = "login!" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Products" }).ClickAsync();
        await _page.GetByRole(AriaRole.Listitem).Filter(new() { HasText = "Name:Trotter PedicurePrice:" }).GetByRole(AriaRole.Button).Nth(1).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Show Inactive Products" }).ClickAsync();
        await _page.GetByRole(AriaRole.Listitem).Filter(new() { HasText = "Trotter PedicureActivate" }).GetByRole(AriaRole.Button).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Add product" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Name:" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Name:" }).FillAsync("Sviniga Äpplen");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Description:" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Description:" }).FillAsync("Svingoda, lätt smutsiga äpplen");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Price:" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Price:" }).FillAsync("20");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Category:" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Category:" }).FillAsync("Föda");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "⬅️ Back" }).ClickAsync();
        await _page.GetByRole(AriaRole.Listitem).Filter(new() { HasText = "Name:Sviniga ÄpplenPrice:" }).GetByRole(AriaRole.Button).Nth(2).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "⬅️ Back" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Support Agents" }).ClickAsync();
        await _page.GetByRole(AriaRole.Listitem).Filter(new() { HasText = "Name:Mud MixerEmail:mud-mixer" }).GetByRole(AriaRole.Button).Nth(1).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Show Inactive Agents" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Activate" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Add Support Agent" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Name:" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Name:" }).FillAsync("Svinulf");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email:" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email:" }).FillAsync("svinulf@grismail.com");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "⬅️ Back" }).ClickAsync();
        await _page.GotoAsync("http://localhost:5000/agents");
        await _page.GetByRole(AriaRole.Listitem).Filter(new() { HasText = "Name:SvinulfEmail:svinulf@" }).GetByRole(AriaRole.Button).Nth(2).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "⬅️ Back" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Categories" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Inactivate" }).Nth(1).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Show Inactive Categories" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Activate", Exact = true }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Category Name.." }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Category Name.." }).FillAsync("slaktfrågor");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Add Category" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).Nth(2).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Sign Out" }).ClickAsync();
    }
}

