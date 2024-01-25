
using exam.DAL;
using exam.Models;
using Microsoft.AspNetCore.Mvc;


namespace exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Service> ser = _context.Services.ToList();
            return View(ser);
        }

       
    }
}