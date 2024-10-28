using Microsoft.AspNetCore.Authentication.Cookies; // För att hantera cookie-baserad autentisering
using Microsoft.AspNetCore.Authentication; // Grundläggande autentisering funktioner
using Microsoft.AspNetCore.Mvc; // För att använda MVC-funktionalitet, som controllers och views
using ReconomyApp.Models; // Importerar modeller från applikationen
using System.Diagnostics; // För felsökning och loggning
using System.Security.Claims; // För att hantera användarens identitet och claims
using System.Threading.Tasks; // För att möjliggöra asynkrona metoder

namespace ReconomyApp.Controllers
{
    // HomeController ärver från Controller, vilket innebär att den kan hantera webbförfrågningar
    public class HomeController : Controller
    {
        // Logger för att logga information under körning
        private readonly ILogger<HomeController> _logger;

        // Konstruktor som tar emot en logger-instans och tilldelar den till den privata variabeln _logger
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: /Home/Index
        // Hanterar GET-begäran till /Home/Index och returnerar en vy
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/CheckIn
        // Hanterar GET-begäran till /Home/CheckIn och returnerar en vy
        public IActionResult CheckIn()
        {
            return View();
        }

        // GET: /Home/Privacy
        // Hanterar GET-begäran till /Home/Privacy och returnerar en vy
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: /Home/DesiredPage
        // Hanterar GET-begäran till /Home/DesiredPage och returnerar en vy för den nya sidan
        public IActionResult DesiredPage()
        {
            return View(); // Renderar vyn för den nya sidan
        }

        // GET: /Home/Login
        // Hanterar GET-begäran till /Home/Login och returnerar inloggningssidan
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Home/Login
        // Hanterar POST-begäran för inloggning, verifierar användaren och loggar in dem om giltiga uppgifter ges
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Kontrollera om användaren är giltig
            if (IsValidUser(username, password))
            {
                // Skapa en lista med claims (påståenden) för användaren, t.ex. deras användarnamn
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                    // Lägg till fler claims om det behövs
                };

                // Skapa en ClaimsIdentity med hjälp av de givna claims och specifik autentiseringsmetod
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Logga in användaren genom att signera in dem med cookie-autentisering
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Omdirigera användaren till den önskade sidan efter en lyckad inloggning
                return RedirectToAction("DesiredPage");
            }

            // Om inloggningen misslyckas, sätt ett felmeddelande och visa CheckIn-sidan igen
            ViewBag.Error = "Invalid username or password";
            return View("CheckIn");
        }

        // Hanterar användarens utloggning
        public async Task<IActionResult> Logout()
        {
            // Logga ut användaren genom att radera autentiseringsinformationen
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Omdirigera till CheckIn-sidan efter utloggning
            return RedirectToAction("CheckIn");
        }

        // GET: /Home/Error
        // Hanterar fel och returnerar en vy som visar felinformation
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // En enkel metod för att kontrollera om användarnamnet och lösenordet är giltiga
        private bool IsValidUser(string username, string password)
        {
            // Här ska egentligen riktig autentisering ske, men just nu är den hårdkodad
            return username == "admin" && password == "password";
        }
    }
}
