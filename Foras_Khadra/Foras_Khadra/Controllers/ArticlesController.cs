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
   public async Task<IActionResult> Index(int page = 1, int pageSize = 6, string? search = null)
{
    if (page < 1) page = 1;

    var query = _context.Articles.AsQueryable();

    // بحث اختياري (حسب العنوان أو الكاتب)
    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(a => a.Title.Contains(search) || a.Author.Contains(search));
    }

    // حساب عدد الصفحات
    var totalCount = await query.CountAsync();
    var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    if (totalPages < 1) totalPages = 1;

    // لو الصفحة أكبر من الموجود
    if (page > totalPages) page = totalPages;

    // جلب بيانات الصفحة الحالية فقط
    var articles = await query
        .OrderByDescending(a => a.PublishDate)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    // إرسال معلومات الباجينيشن للـ View
    ViewBag.Page = page;
    ViewBag.TotalPages = totalPages;
    ViewBag.Search = search;

    return View(articles);
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
