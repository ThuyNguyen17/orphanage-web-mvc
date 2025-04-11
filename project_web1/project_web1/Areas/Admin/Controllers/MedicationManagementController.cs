using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_web1.Areas.Admin.Models;
using project_web1.Models;

namespace project_web1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MedicationManagementController : Controller
    {
        private readonly TrungTamTreContext _context;

        public MedicationManagementController(TrungTamTreContext context)
        {
            _context = context;
        }

        // GET: Admin/MedicationManagement
        public IActionResult Index()
        {
            var danhSach = _context.ThuocMuaNgoais.ToList();
            var tongChi = danhSach.Sum(p => p.TongTien ?? 0);

            var vm = new ThuocMuaNgoaiIndexViewModel
            {
                DanhSachPhieu = danhSach,
                TongChi = tongChi,
                PhieuMoi = new ThuocMuaNgoai()
            };

            return View(vm); // Không cần dùng ViewBag nữa
        }

        // GET: Admin/MedicationManagement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var purchase = await _context.ThuocMuaNgoais
                .Include(t => t.CtphieuMuas)
                .ThenInclude(c => c.MaSoThuocNavigation)
                .FirstOrDefaultAsync(m => m.MaSoMuaThuoc == id);

            if (purchase == null)
                return NotFound();

            return View(purchase);
        }

        // POST: Admin/MedicationManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ThuocMuaNgoai thuocMua)
        {
            if (ModelState.IsValid)
            {
                thuocMua.NgayMua ??= DateTime.Now;

                _context.ThuocMuaNgoais.Add(thuocMua);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Tạo phiếu mua thuốc thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Khi có lỗi → nạp lại dữ liệu cần thiết và render lại view
            var danhSach = _context.ThuocMuaNgoais.ToList();
            var tongChi = danhSach.Sum(p => p.TongTien ?? 0);

            var vm = new ThuocMuaNgoaiIndexViewModel
            {
                DanhSachPhieu = danhSach,
                TongChi = tongChi,
                PhieuMoi = thuocMua // Gửi lại dữ liệu lỗi cho view
            };

            TempData["Error"] = "Vui lòng điền đầy đủ thông tin!";
            return View("Index", vm);
        }


        // GET: Admin/MedicationManagement/Edit/5
        public IActionResult Edit(int id)
        {
            var thuoc = _context.ThuocMuaNgoais.FirstOrDefault(t => t.MaSoMuaThuoc == id);
            if (thuoc == null)
            {
                return NotFound();
            }
            return View(thuoc);
        }


        // POST: Admin/MedicationManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ThuocMuaNgoai thuoc)
        {
            if (ModelState.IsValid)
            {
                _context.Update(thuoc);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(thuoc); // nếu có lỗi validation
        }

        // GET: Admin/MedicationManagement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var thuocMua = await _context.ThuocMuaNgoais
                .FirstOrDefaultAsync(m => m.MaSoMuaThuoc == id);
            if (thuocMua == null)
                return NotFound();

            return View(thuocMua);
        }

        // POST: Admin/MedicationManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thuocMua = await _context.ThuocMuaNgoais.FindAsync(id);
            if (thuocMua != null)
            {
                _context.ThuocMuaNgoais.Remove(thuocMua);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa phiếu mua thuốc thành công!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
