using exam.DAL;
using exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace exam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            List<Setting> set=_context.Settins.ToList();    
            return View(set);
        }
        public async Task<IActionResult>Update(int id)
        {
            if (id <= 0) return BadRequest();
            Setting exist=await _context.Settins.FirstOrDefaultAsync(s=>s.Id == id);
            if (exist == null) return NotFound(); 
            return View(exist);
            
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,Setting set)
        {
            if(!ModelState.IsValid) return View(set);
            Setting exist = await _context.Settins.FirstOrDefaultAsync(s => s.Id == id);
            if (exist == null) return NotFound();
            var result = await _context.Settins.AnyAsync(s => s.Key == exist.Key&&s.Id!=id);
            if (!result)
            {
                ModelState.AddModelError("Key", "bu key artiq movcuddur");
                return View(set);
            }
            var result1 = await _context.Settins.AnyAsync(s => s.Value == exist.Value && s.Id != id);
            if (!result1)
            {
                ModelState.AddModelError("Key", "bu value artiq movcuddur");
                return View(set);
            }
            exist.Key= set.Key;
            exist.Value= set.Value;
            await _context.SaveChangesAsync();  
            return RedirectToAction(nameof(Index));
        }
    }
}
