using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ArticlesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ArticlesController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    // INDEX
    public async Task<IActionResult> Index()
    {
        return View(await _context.Articles
            .OrderByDescending(a => a.PublishDate)
            .ToListAsync());
    }

    // DETAILS
    public async Task<IActionResult> Details(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article == null) return NotFound();
        return View(article);
    }

    // CREATE
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Article article, IFormFile Image)
    {
        if (Image != null)
        {
            string uploads = Path.Combine(_env.WebRootPath, "uploads");
            string fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
            string filePath = Path.Combine(uploads, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await Image.CopyToAsync(stream);

            article.ImagePath = "/uploads/" + fileName;
        }

        article.PublishDate = DateTime.Now;
        _context.Add(article);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // EDIT
    public async Task<IActionResult> Edit(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article == null) return NotFound();
        return View(article);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Article article, IFormFile Image)
    {
        var existing = await _context.Articles.FindAsync(id);

        if (Image != null)
        {
            string uploads = Path.Combine(_env.WebRootPath, "uploads");
            string fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
            string filePath = Path.Combine(uploads, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await Image.CopyToAsync(stream);

            existing.ImagePath = "/uploads/" + fileName;
        }

        existing.Title = article.Title;
        existing.Content = article.Content;
        existing.Author = article.Author;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // DELETE (بدون صفحة)
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article != null)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
