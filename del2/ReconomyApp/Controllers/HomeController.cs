using Microsoft.AspNetCore.Authentication.Cookies; // F�r att hantera cookie-baserad autentisering
using Microsoft.AspNetCore.Authentication; // Grundl�ggande autentisering funktioner
using Microsoft.AspNetCore.Mvc; // F�r att anv�nda MVC-funktionalitet, som controllers och views
using ReconomyApp.Models; // Importerar modeller fr�n applikationen
using System.Diagnostics; // F�r fels�kning och loggning
using System.Security.Claims; // F�r att hantera anv�ndarens identitet och claims
using System.Threading.Tasks; // F�r att m�jligg�ra asynkrona metoder

namespace ReconomyApp.Controllers
{
    // HomeController �rver fr�n Controller, vilket inneb�r att den kan hantera webbf�rfr�gningar
    public class HomeController : Controller
    {
        // Logger f�r att logga information under k�rning
        private readonly ILogger<HomeController> _logger;

        // Konstruktor som tar emot en logger-instans och tilldelar den till den privata variabeln _logger
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: /Home/Index
        // Hanterar GET-beg�ran till /Home/Index och returnerar en vy
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/CheckIn
        // Hanterar GET-beg�ran till /Home/CheckIn och returnerar en vy
        public IActionResult CheckIn()
        {
            return View();
        }

        // GET: /Home/Privacy
        // Hanterar GET-beg�ran till /Home/Privacy och returnerar en vy
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: /Home/DesiredPage
        // Hanterar GET-beg�ran till /Home/DesiredPage och returnerar en vy f�r den nya sidan
        public IActionResult DesiredPage()
        {
            return View(); // Renderar vyn f�r den nya sidan
        }

        // GET: /Home/Login
        // Hanterar GET-beg�ran till /Home/Login och returnerar inloggningssidan
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Home/Login
        // Hanterar POST-beg�ran f�r inloggning, verifierar anv�ndaren och loggar in dem om giltiga uppgifter ges
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Kontrollera om anv�ndaren �r giltig
            if (IsValidUser(username, password))
            {
                // Skapa en lista med claims (p�st�enden) f�r anv�ndaren, t.ex. deras anv�ndarnamn
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                    // L�gg till fler claims om det beh�vs
                };

                // Skapa en ClaimsIdentity med hj�lp av de givna claims och specifik autentiseringsmetod
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Logga in anv�ndaren genom att signera in dem med cookie-autentisering
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Omdirigera anv�ndaren till den �nskade sidan efter en lyckad inloggning
                return RedirectToAction("DesiredPage");
            }

            // Om inloggningen misslyckas, s�tt ett felmeddelande och visa CheckIn-sidan igen
            ViewBag.Error = "Invalid username or password";
            return View("CheckIn");
        }

        // Hanterar anv�ndarens utloggning
        public async Task<IActionResult> Logout()
        {
            // Logga ut anv�ndaren genom att radera autentiseringsinformationen
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

        // En enkel metod f�r att kontrollera om anv�ndarnamnet och l�senordet �r giltiga
        private bool IsValidUser(string username, string password)
        {
            // H�r ska egentligen riktig autentisering ske, men just nu �r den h�rdkodad
            return username == "admin" && password == "password";
        }
    }
}
