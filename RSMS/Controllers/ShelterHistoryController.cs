using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using RSMS.Data;
using RSMS.DTO;
using RSMS.Models;

namespace RSMS.Controllers
{
    public class ShelterHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShelterHistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Temperature(string code) 
        {
            var data = await _context.Readings
                .Where(r => r.ShelterCode == code)
                .OrderByDescending(r => r.TimeStamp)
                .Select(r => new TemperatureView
                {
                    Time = r.TimeStamp,
                    Value = r.Temperature,
                })
                .ToListAsync();

            ViewBag.ShelterCode = code;
            return View(data);

        }

        [HttpGet]
        public async Task<IActionResult> GetTemperatureHistory(string shelterCode, DateTime? startDate, DateTime ?endDate) 
        {
            var data = _context.Readings
                .Where(r => r.ShelterCode == shelterCode);
                
            //Applying the date filtering
            if(startDate.HasValue)
            {
                data = data.Where(r => r.TimeStamp >= startDate.Value);
            }
            if (endDate.HasValue) 
            {
                data = data.Where(r => r.TimeStamp <= endDate.Value);
            }

            var query = await data
                .OrderByDescending(q => q.TimeStamp)
                .Select(q => new TemperatureChartDTO
                {
                    Time = q.TimeStamp.ToString("yyyy-MM-dd HH:mm"),
                    Temperature = q.Temperature
                })
                .ToListAsync();
            return Json(query);
        }
    }
}
