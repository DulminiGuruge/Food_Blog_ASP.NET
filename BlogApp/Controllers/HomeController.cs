using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using BlogApp.Services.ViewModels;
using BlogApp.Services;
using BlogApp.Data;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBlogEmailSender _emailSender;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<BlogUser> _userManager;


    public HomeController(ILogger<HomeController> logger, IBlogEmailSender emailSender, ApplicationDbContext context, UserManager<BlogUser> userManager)
    {
        _logger = logger;
        _emailSender = emailSender;
        _context = context;
        _userManager = userManager;

    }

    public async Task<IActionResult> Index(int? page)
    {
        var pageNumber = page ?? 1;
        var pageSize = 5;

        //var blogs = await _context.Blogs.Include(b=>b.BlogUser).ToListAsync();
        //return View(blogs);

        var blogs = _context.Blogs
            .Include(b=>b.BlogUser)
           // Where(b => b.Posts.Any(p => p.ReadyStaus == Enum.ReadyStatus.ProductionReady))
            .OrderByDescending(b => b.Created)
            .ToPagedListAsync(pageNumber, pageSize);

        return View(await blogs);
    }
    //public async Task<IActionResult> Index()
    //{
    //    var blogs = await _context.Blogs.Include(b => b.BlogUser).ToListAsync();
    //    return View(blogs);
    //}

    public IActionResult About()
    {
        return View();
    }
    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(ContactMe model)
    {
        //Emailing takes place
        model.Message = $"{model.Message}<hr/> Phone: {model.Phone}";
        await _emailSender.SendContactEmailAsync(model.Email, model.Name, model.Subject, model.Message);
        return RedirectToAction("Index");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

