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
    [HttpGet]
    public async Task<IActionResult> Index(string search, int page = 1)
    {
        const int pageSize = 10;

        var query = _context.Articles.AsQueryable();

     if (!string.IsNullOrWhiteSpace(search))
{
    query = query.Where(a =>
        a.TitleAr.Contains(search) ||
        a.TitleEn.Contains(search) ||
        a.TitleFr.Contains(search));
}


        int totalItems = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var articles = await query
            .OrderByDescending(a => a.PublishDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.Page = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.Search = search;

        return View(articles);
    }

    // DETAILS
    public async Task<IActionResult> Details(int id)
    {
        var article = await _context.Articles
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
            return NotFound();

        // أحدث المقالات (لـ اقرأ أيضًا)
        ViewBag.LatestArticles = await _context.Articles
            .Where(a => a.Id != id)
            .OrderByDescending(a => a.PublishDate)
            .Take(6) // نأخذ عدد أكبر والفيو يختار 3
            .ToListAsync();

        return View(article);
    }

    // CREATE
    public IActionResult Create()
    {
        ViewBag.IsAdmin = User.IsInRole("Admin");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Article article, IFormFile Image)
    {
        if (User.IsInRole("Admin"))
        {
            article.Author = "فرص خضراء";
        }

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
        ViewBag.IsAdmin = User.IsInRole("Admin");
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

        existing.TitleAr = article.TitleAr;
        existing.TitleEn = article.TitleEn;
        existing.TitleFr = article.TitleFr;
        existing.ContentAr = article.ContentAr;
        existing.ContentEn = article.ContentEn;
        existing.ContentFr = article.ContentFr;
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
