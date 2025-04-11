using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using project_web1.Areas.Admin.Models;
using project_web1.Models;
using System.IO;

namespace project_web1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ManageChildrenController : Controller
    {
        private readonly TrungTamTreContext _context;
        private readonly IWebHostEnvironment _env;

        public ManageChildrenController(TrungTamTreContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var children = await _context.Tres.Include(t => t.MaNhanVienNavigation).ToListAsync();
            ViewBag.Caretakers = await _context.NguoiChamSocs.Select(n => new SelectListItem
            {
                Value = n.MaNhanVien.ToString(),
                Text = n.HoTen
            }).ToListAsync();
            return View(children);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChild([FromForm] Tre model, IFormFile AnhDaiDienFile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Dữ liệu không hợp lệ",
                        errors = ModelState.ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                        )
                    });
                }

                // Xử lý ảnh nếu có
                model.AnhDaiDien = await ProcessUploadedImage(AnhDaiDienFile);

                _context.Tres.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Thêm trẻ thành công",
                    child = new
                    {
                        model.MaSoTre,
                        model.HoTen,
                        NgaySinh = model.NgaySinh?.ToString("yyyy-MM-dd"),
                        model.GioiTinh,
                        model.TrangThai,
                        model.AnhDaiDien
                    },
                    imageUrl = $"/images/{model.AnhDaiDien}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi server",
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetChildrenList()
        {
            try
            {
                var children = await _context.Tres
                    .Select(child => new
                    {
                        maSoTre = child.MaSoTre,
                        hoTen = child.HoTen,
                        ngaySinh = child.NgaySinh != null ? child.NgaySinh.Value.ToString("yyyy-MM-dd") : null,
                        trangThai = child.TrangThai,
                        imageUrl = $"/images/{(string.IsNullOrEmpty(child.AnhDaiDien) ? "Default.jpg" : child.AnhDaiDien)}"
                    })
                    .ToListAsync();

                return Ok(new { success = true, children });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Lỗi khi lấy danh sách trẻ: {ex.Message}" });
            }
        }

        public IActionResult ChildDetails(int id)
        {
            var child = _context.Tres
                .Include(t => t.HoSoSucKhoes)
                .FirstOrDefault(t => t.MaSoTre == id);

            if (child == null)
            {
                return NotFound();
            }

            var latestHealthStatus = child.HoSoSucKhoes
                .OrderByDescending(h => h.MaHoSo)
                .FirstOrDefault();

            ViewData["HealthStatus"] = latestHealthStatus?.TinhTrangSucKhoe ?? "Đang cập nhật";
            ViewData["Weight"] = latestHealthStatus?.CanNang ?? 0;
            ViewData["Height"] = latestHealthStatus?.ChieuCao ?? 0;
            ViewData["DiUng"] = latestHealthStatus?.DiUng;

            return View(child);
        }

        // GET: Update
        // GET: Update

        [HttpDelete]
        public async Task<IActionResult> DeleteChild(int id)
        {
            var child = await _context.Tres.FindAsync(id);
            if (child == null) return NotFound();

            _context.Tres.Remove(child);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        private async Task<string> ProcessUploadedImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return "Default.jpg"; // Default image if none provided
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var originalFileName = Path.GetFileName(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, originalFileName);

            if (System.IO.File.Exists(filePath))
            {
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
                var extension = Path.GetExtension(originalFileName);
                var uniqueFileName = $"{fileNameWithoutExt}_{Guid.NewGuid()}{extension}";
                filePath = Path.Combine(uploadsFolder, uniqueFileName);
                originalFileName = uniqueFileName;
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return originalFileName;
        }
    }
}
