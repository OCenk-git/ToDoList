using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context,UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            IEnumerable<ToDo> toDo = _context.ToDos.Where(x => x.UserID == _userManager.GetUserId(HttpContext.User));

            return View(toDo);
        }

        public IActionResult Edit(int id) 
        { 
            ToDo toDo = _context.ToDos.Find(id);

            return View(toDo); 
        }

        [HttpPost]
        public IActionResult Edit(int id, string beschreibung, DateTime enddatum, string erledigt)
        {
            ToDo toDo = _context.ToDos.Find(id);

            toDo.Beschreibung = beschreibung;
            toDo.EndDatum = enddatum;
            toDo.Erledigt = erledigt;

            _context.SaveChanges();

			IEnumerable<ToDo> toDos = _context.ToDos.Where(x => x.UserID == _userManager.GetUserId(HttpContext.User));

			return View("Index", toDos);
        }

        public IActionResult Delete(int id)
        {
            ToDo toDo = _context.ToDos.Find(id);

            _context.ToDos.Remove(toDo);
            _context.SaveChanges();

            IEnumerable<ToDo> toDos = _context.ToDos.Where(x => x.UserID == _userManager.GetUserId(HttpContext.User));

			return View("Index", toDos);
        }

        public IActionResult Open() 
        {
            IEnumerable<ToDo> toDo = _context.ToDos.Where(x => x.UserID == _userManager.GetUserId(HttpContext.User)).Where(x => x.Erledigt == "Offen");

            return View("Index",toDo); 
        }

        public IActionResult Done() 
        {
			IEnumerable<ToDo> toDo = _context.ToDos.Where(x => x.UserID == _userManager.GetUserId(HttpContext.User)).Where(x => x.Erledigt == "Erledigt");

			return View("Index",toDo); 
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string beschreibung, DateTime enddatum)
        {
            ToDo toDo = new ToDo();

            toDo.Beschreibung = beschreibung;
            toDo.EndDatum = enddatum;
            toDo.StartDatum = DateTime.Now;
            toDo.Erledigt = "Offen";
            toDo.UserID = _userManager.GetUserId(HttpContext.User);

            _context.Add(toDo);
            _context.SaveChanges();

			IEnumerable<ToDo> toDos = _context.ToDos.Where(x => x.UserID == _userManager.GetUserId(HttpContext.User));

			return View("Index", toDos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}