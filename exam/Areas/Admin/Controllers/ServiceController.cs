using exam.DAL;
using exam.Models;
using exam.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;

namespace exam.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ServiceController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            List<Service> ser = _context.Services.ToList();
            return View(ser);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Create(Service ser)
        {
            if(!ModelState.IsValid) return View(ser);
            if(ser.Photo!= null)
            {
                if (!ser.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError("Photo", "File tipi uyqun deyil");
                    return View(ser);
                }
                if (!ser.Photo.ValidateSize(2*1024))
                {
                    ModelState.AddModelError("Photo", "File olcusu uyqun deyil");
                    return View(ser);
                }
            }
            string filename = await ser.Photo.CreateAsync(_env.WebRootPath, "assets", "img");
            Service service = new Service
            {
                Image= filename,
                Name=ser.Name,
                Description=ser.Description,
            };
            await _context.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult>Update(int id)
        {
            if (id <= 0) return BadRequest();
            Service exist=await _context.Services.FirstOrDefaultAsync(s=>s.Id==id);
            if(exist==null) return NotFound();  
            return View(exist);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,Service ser)
        {
            if (!ModelState.IsValid) return View(ser);
            Service exist = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (exist == null) return NotFound();
            if (ser.Photo != null)
            {
                if (!ser.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError("Photo", "File tipi uyqun deyil");
                    return View(ser);
                }
                if (!ser.Photo.ValidateSize(2 * 1024))
                {
                    ModelState.AddModelError("Photo", "File olcusu uyqun deyil");
                    return View(ser);
                }
                string filename = await ser.Photo.CreateAsync(_env.WebRootPath, "assets", "img");
                exist.Image.DeleteAsync(_env.WebRootPath, "assets", "img");
                exist.Image = filename;
            }
            exist.Name= ser.Name;
            exist.Description= ser.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult>Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Service exist = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (exist == null) return NotFound();
            exist.Image.DeleteAsync(_env.WebRootPath, "assets", "img");
             _context.Services.Remove(exist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
