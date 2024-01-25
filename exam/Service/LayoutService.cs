using exam.DAL;
using Microsoft.EntityFrameworkCore;

namespace exam.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<string, string>> GetServiceAsync()
        {
            return await _context.Settins.ToDictionaryAsync(s=>s.Key, s=>s.Value);
        }
    }
}
