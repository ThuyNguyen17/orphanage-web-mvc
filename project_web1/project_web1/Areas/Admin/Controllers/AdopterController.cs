using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_web1.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace project_web1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdopterController : Controller
    {
        private readonly TrungTamTreContext _context;

        public AdopterController(TrungTamTreContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IActionResult Index()
        {
            var model = new AdopterViewModel
            {
                Adopters = _context.NguoiNhanNuois.ToList(),
                Children = _context.Tres.Where(t => t.TrangThai == "Nhan Nuoi").ToList(),
                AvailableChildren = _context.Tres.Where(t => t.TrangThai == "Trung Tam").ToList(),
                TotalAdopters = _context.NguoiNhanNuois.Count(),
                TotalAdoptedChildren = _context.Tres.Count(t => t.TrangThai == "Nhan Nuoi")
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Adopter/Create")]
        public async Task<IActionResult> Create(NguoiNhanNuoi model)
        {
            Console.WriteLine("Create method called with model: " + model.ToString());

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();

                Console.WriteLine("Validation errors: " + string.Join(", ", errors));
                return BadRequest(new { success = false, message = "Validation failed", errors });
            }

            try
            {
                Console.WriteLine("do try");
                var child = await _context.Tres.FindAsync(model.MaSoTre);
                if (child == null)
                    return NotFound(new { success = false, message = "Child not found" });

                child.TrangThai = "Nhan Nuoi";

                var adopter = new NguoiNhanNuoi
                {
                    HoTen = model.HoTen,
                    Sdt = model.Sdt,
                    NgaySinh = model.NgaySinh,
                    Email = model.Email,
                    MaSoTre = model.MaSoTre,
                    DiaChi = model.DiaChi
                };
                Console.WriteLine($"HoTen: {model.HoTen}, Sdt: {model.Sdt}, NgaySinh: {model.NgaySinh}, Email: {model.Email}, MaSoTre: {model.MaSoTre}");

                await _context.NguoiNhanNuois.AddAsync(adopter);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Adopter added successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while creating adopter", detail = ex.Message });
            }
        }


        [HttpPost]
        [Route("Admin/Adopter/Update")]
        public async Task<IActionResult> Update(NguoiNhanNuoi model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }

            try
            {
                var existingAdopter = await _context.NguoiNhanNuois.FindAsync(model.MaSoNnn);
                if (existingAdopter == null)
                {
                    return NotFound(new { success = false, message = "Adopter not found" });
                }

                // Update properties
                existingAdopter.HoTen = model.HoTen;
                existingAdopter.NgaySinh = model.NgaySinh;
                existingAdopter.Sdt = model.Sdt;
                existingAdopter.DiaChi = model.DiaChi;
                existingAdopter.Email = model.Email;
                existingAdopter.MaSoTre = model.MaSoTre;

                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Updated successfully!" });
            }
            catch (Exception ex)
            {
                // Log error here
                return StatusCode(500, new { success = false, message = "An error occurred while updating adopter" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var adopter = await _context.NguoiNhanNuois.FindAsync(id);
                if (adopter == null)
                {
                    return NotFound(new { success = false, message = "Adopter not found" });
                }

                // Update child status back to "Trung Tam"
                var child = await _context.Tres.FindAsync(adopter.MaSoTre);
                if (child != null)
                {
                    child.TrangThai = "Trung Tam";
                }

                _context.NguoiNhanNuois.Remove(adopter);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Deleted successfully!" });
            }
            catch (Exception ex)
            {
                // Log error here
                return StatusCode(500, new { success = false, message = "An error occurred while deleting adopter" });
            }
        }
    }
}