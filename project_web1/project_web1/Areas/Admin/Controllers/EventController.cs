using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_web1.Models;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class EventController : Controller
{
    private readonly TrungTamTreContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public EventController(TrungTamTreContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        return View(_context.Events.ToList());
    }
    [HttpGet]
    public IActionResult GetEventById(int id)
    {
        var eventItem = _context.Events.Find(id);

        if (eventItem == null)
        {
            return Json(new
            {
                success = false,
                message = "Không tìm thấy sự kiện"
            });
        }

        return Json(new
        {
            success = true,
            eventData = new // tránh dùng từ khóa 'event'
            {
                id = eventItem.Id,
                title = eventItem.Title,
                eventDate = eventItem.EventDate,
                time = eventItem.Time,
                location = eventItem.Location,
                description = eventItem.Description,
                imageUrl = eventItem.ImageUrl
            }
        });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Event model, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            try
            {
                model.ImageUrl = await ProcessUploadedImage(imageFile);
                _context.Add(model);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Thêm sự kiện thành công",
                    eventId = model.Id
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Lỗi khi thêm sự kiện",
                    error = ex.Message
                });
            }
        }

        return Json(new
        {
            success = false,
            errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
        });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Event model, IFormFile? ImageFile)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var eventToUpdate = await _context.Events.FindAsync(model.Id);
                if (eventToUpdate == null)
                {
                    ViewBag.ErrorMessage = "Không tìm thấy sự kiện";
                    return View("Error");
                }

                // Cập nhật các trường cơ bản
                eventToUpdate.Title = model.Title;
                eventToUpdate.EventDate = model.EventDate;
                eventToUpdate.Time = model.Time;
                eventToUpdate.Location = model.Location;
                eventToUpdate.Description = model.Description;

                // Nếu có ảnh mới được upload
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(eventToUpdate.ImageUrl))
                    {
                        DeleteImage(eventToUpdate.ImageUrl); // Hàm xóa ảnh cũ trong wwwroot
                    }

                    // Lưu ảnh mới
                    var imagePath = await ProcessUploadedImage(ImageFile); // Trả về đường dẫn URL ảnh
                    eventToUpdate.ImageUrl = imagePath;
                }

                // Cập nhật vào database
                _context.Events.Update(eventToUpdate);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi cập nhật: " + ex.Message;
                return View("Error");
            }
        }

        ViewBag.ErrorMessage = "Dữ liệu không hợp lệ";
        return View("Edit", model);
    }
    private async Task<string> ProcessUploadedImage(IFormFile imageFile)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/events");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        return "/uploads/events/" + fileName; // Đường dẫn ảnh dùng trong thẻ <img>
    }

    private void DeleteImage(string imageUrl)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        try
        {
            var eventToDelete = _context.Events.Find(id);
            if (eventToDelete == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Không tìm thấy sự kiện với ID: " + id
                });
            }

            // Xóa ảnh nếu có
            if (!string.IsNullOrEmpty(eventToDelete.ImageUrl))
            {
                DeleteImage(eventToDelete.ImageUrl);
            }

            _context.Events.Remove(eventToDelete);
            _context.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Xóa sự kiện thành công",
                deletedId = id
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Lỗi server khi xóa sự kiện",
                error = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }
}