using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_web1.Models;
using project_web1.Areas.Admin.Models;


namespace project_web1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Chỉ cho phép admin truy cập
    public class SponsorshipManagementController : Controller
    {
        private readonly TrungTamTreContext _context;

        public SponsorshipManagementController(TrungTamTreContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var sponsors = _context.NhaTaiTros.Include(s => s.HoaDonTaiTros).ToList();

            var viewModelList = sponsors.Select(s => new SponsorViewModel
            {
                NhaTaiTro = s,
                TotalAmount = s.HoaDonTaiTros?.Sum(h => h.SoTien) ?? 0
            }).ToList();

            ViewBag.TotalAmount = viewModelList.Sum(x => x.TotalAmount);
            return View(viewModelList);
        }


        [HttpGet]
        public IActionResult AddSponsor()
        {
            var model = new NhaTaiTroViewModel(); // Khởi tạo model rỗng
            return View(model);
        }
        // GET: Hiển thị form
        [HttpGet]
        public IActionResult AddSponsorForm() // ✅ Tên khác hẳn
        {
            return View("AddSponsor"); // ✅ Trả về đúng view tên AddSponsor.cshtml
        }

        // POST: Xử lý form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSponsor(NhaTaiTroViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.NhaTaiTros.Add(model.NhaTaiTro);
                await _context.SaveChangesAsync();

                model.HoaDonTaiTro.MaSoNtt = model.NhaTaiTro.MaSoNtt;
                _context.HoaDonTaiTros.Add(model.HoaDonTaiTro);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                // Debug xem lỗi gì
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(modelError.ErrorMessage); // Hoặc log ra file
                }

                return View(model); // Trả lại view với lỗi
            }

            return View(model); // Dòng này sẽ không bao giờ được thực hiện.
                                // ✅ đúng cú pháp, trả về lại view cùng tên với model
        }




        [HttpGet]
        public IActionResult DetailsSpon(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var sponsor = _context.NhaTaiTros
                .Include(s => s.HoaDonTaiTros)
                .FirstOrDefault(s => s.MaSoNtt == id);

            if (sponsor == null)
            {
                return NotFound();
            }

            // Tính tổng số tiền đã tài trợ
            var totalAmount = sponsor.HoaDonTaiTros?.Sum(h => h.SoTien) ?? 0;
            ViewBag.TotalAmount = totalAmount;

            return View(sponsor);
        }
        // GET: Hiển thị form chỉnh sửa Nhà Tài Trợ
        [HttpGet]
        public async Task<IActionResult> EditSpon(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var sponsorEntity = await _context.NhaTaiTros.FirstOrDefaultAsync(s => s.MaSoNtt == id);
            if (sponsorEntity == null)
            {
                return NotFound();
            }
            var viewModel = new NhaTaiTroViewModel
            {
                NhaTaiTro = new NhaTaiTro
                {
                    MaSoNtt = sponsorEntity.MaSoNtt,
                    Ten = sponsorEntity.Ten,
                    Sdt = sponsorEntity.Sdt,
                    DiaChi = sponsorEntity.DiaChi,
                    GhiChu = sponsorEntity.GhiChu
                }
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSpon(int id, NhaTaiTroViewModel model)
        {
            if (id != model.NhaTaiTro.MaSoNtt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var sponsor = await _context.NhaTaiTros.FindAsync(id);
                if (sponsor == null)
                {
                    return NotFound();
                }

                sponsor.Ten = model.NhaTaiTro.Ten;
                sponsor.Sdt = model.NhaTaiTro.Sdt;
                sponsor.DiaChi = model.NhaTaiTro.DiaChi;
                sponsor.GhiChu = model.NhaTaiTro.GhiChu;

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật nhà tài trợ thành công!";
                return RedirectToAction("DetailsSpon", new { id = sponsor.MaSoNtt });
            }

            return View(model);
        }
    }
        }
    
