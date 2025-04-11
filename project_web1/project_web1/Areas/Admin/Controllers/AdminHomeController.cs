using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_web1.Models;

namespace project_web1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Chỉ cho phép admin truy cập
    public class AdminHomeController : Controller
    {
        private readonly TrungTamTreContext _context;

        public AdminHomeController(TrungTamTreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new HomeIndexViewModel
            {
                TotalTreTrongTam = _context.Tres.Count(t => t.TrangThai == "Trung Tam"),
                TotalTreDaNhanNuoi = _context.Tres.Count(t => t.TrangThai == "Nhan Nuoi"),
                TotalSoTien = _context.HoaDonTaiTros.Sum(p => (decimal?)p.SoTien) ?? 0,
                TodoList = _context.TodoItems.ToList(),
                TreList = _context.Tres.ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetTodos()
        {
            var todos = await _context.TodoItems
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
            return Json(todos);
        }
        [HttpPost]
        public async Task<IActionResult> AddTodo([FromBody] TodoItem todo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    todo.CreatedDate = DateTime.Now;
                    _context.TodoItems.Add(todo);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> ToggleTodo(int id)
        {
            var todo = await _context.TodoItems.FindAsync(id);
            if (todo != null)
            {
                todo.IsCompleted = !todo.IsCompleted;
                await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Không tìm thấy công việc." });
        }
        [HttpDelete]
        public async Task<JsonResult> DeleteTodo(int id)
        {
            var todo = await _context.TodoItems.FindAsync(id);
            if (todo != null)
            {
                _context.TodoItems.Remove(todo);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Không tìm thấy công việc để xóa." });
        }

    }
}
