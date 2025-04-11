using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using project_web1.Models;

namespace project_web1.Controllers;

public class HomeController : Controller
{
    private readonly TrungTamTreContext _context;

    public HomeController(TrungTamTreContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var children = _context.Tres.ToList();
        var events = _context.Events.ToList();
        var NguoiChamSoc = _context.NguoiChamSocs.ToList();
        var model = new Tuple<IEnumerable<Tre>, IEnumerable<Event>, IEnumerable<NguoiChamSoc>>(children, events, NguoiChamSoc);
        return View(model);
    }
    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Event()
    {
        var events = _context.Events.ToList();
        return View(events);
    }
    public IActionResult Contact()
    {
        return View();
    }
    public IActionResult About()
    {
        return View();
    }
    public IActionResult Donate()
    {
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
