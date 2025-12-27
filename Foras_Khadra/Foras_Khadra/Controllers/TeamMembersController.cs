using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Microsoft.AspNetCore.Mvc;

public class TeamMembersController : Controller
{
    private readonly ApplicationDbContext _context;

    public TeamMembersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // INDEX (About Us)
    public IActionResult Index()
    {
        var members = _context.TeamMembers.ToList();
        return View(members);
    }

    // CREATE
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(TeamMember model)
    {
        if (ModelState.IsValid)
        {
            _context.TeamMembers.Add(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // EDIT
    public IActionResult Edit(int id)
    {
        var member = _context.TeamMembers.Find(id);
        return View(member);
    }

    [HttpPost]
    public IActionResult Edit(TeamMember model)
    {
        if (ModelState.IsValid)
        {
            _context.TeamMembers.Update(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // DELETE
    public IActionResult Delete(int id)
    {
        var member = _context.TeamMembers.Find(id);
        _context.TeamMembers.Remove(member);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
