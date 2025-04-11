using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_web1.Models;
using Microsoft.EntityFrameworkCore;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ScheduleCheckupController : Controller
{
    private readonly TrungTamTreContext _context;

    public ScheduleCheckupController(TrungTamTreContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewBag.ChildrenList = _context.Tres.ToList();
        ViewBag.NhanVienYTeList = _context.NhanVienYtes.ToList();
        var phieuKhamList = _context.PhieuKhamBenhs.Include(p => p.MaSoTreNavigation).Include(p => p.IdnvyteNavigation).ToList();
        return View(phieuKhamList);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreatePhieuKham(PhieuKhamBenh model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Index");
            }

            model.NgayGhi = DateTime.Now;
            _context.PhieuKhamBenhs.Add(model);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Thêm phiếu khám thành công";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Lỗi server: " + ex.Message;
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult GetPhieuKhamById(int id)
    {
        var phieu = _context.PhieuKhamBenhs
            .Include(p => p.MaSoTreNavigation)
            .Include(p => p.IdnvyteNavigation)
            .FirstOrDefault(p => p.MaPhieuKham == id);

        if (phieu == null)
        {
            return Json(new { success = false, message = "Không tìm thấy phiếu khám" });
        }

        return Json(new
        {
            success = true,
            maPhieuKham = phieu.MaPhieuKham,
            canNang = phieu.CanNang,
            chieuCao = phieu.ChieuCao,
            tinhTrangSucKhoe = phieu.TinhTrangSucKhoe,
            ngayKham = phieu.NgayGhi
        });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdatePhieuKham([FromBody] PhieuKhamBenh model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
        }

        try
        {
            var phieu = _context.PhieuKhamBenhs.Find(model.MaPhieuKham);
            if (phieu == null)
            {
                return Json(new { success = false, message = "Không tìm thấy phiếu khám" });
            }

            // Chỉ cập nhật những trường có trong model
            if (model.CanNang.HasValue)
            {
                phieu.CanNang = model.CanNang.Value;
            }

            if (model.ChieuCao.HasValue)
            {
                phieu.ChieuCao = model.ChieuCao.Value;
            }

            if (model.TinhTrangSucKhoe != null)
            {
                phieu.TinhTrangSucKhoe = model.TinhTrangSucKhoe;
            }

            if (model.NgayGhi != null)
            {
                phieu.NgayGhi = model.NgayGhi;
            }

            _context.Update(phieu);
            _context.SaveChanges();

            return Json(new { success = true, message = "Cập nhật thành công" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Lỗi: " + ex.Message });
        }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePhieuKham(int id)
    {
        try
        {
            var phieu = _context.PhieuKhamBenhs.Find(id);
            if (phieu == null)
            {
                return Json(new { success = false, message = "Không tìm thấy phiếu khám" });
            }

            _context.PhieuKhamBenhs.Remove(phieu);
            _context.SaveChanges();

            return Json(new { success = true, message = "Xóa thành công" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Lỗi: " + ex.Message });
        }
    }
}