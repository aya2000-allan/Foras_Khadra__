using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace Foras_Khadra.Controllers
{
    public class OpportunityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public OpportunityController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            var opportunities = _context.Opportunities.ToList();

            // تجميع الفرص حسب النوع
            var grouped = opportunities
                .GroupBy(o => o.Type)
                .ToDictionary(g => g.Key, g => g.ToList());

            return View(grouped);
        }

        // ================= CREATE =================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(OpportunityCreateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // رفع الصورة
            string imagePath = null;
            if (model.Image != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
                var path = Path.Combine(_env.WebRootPath, "uploads/opportunities", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                model.Image.CopyTo(stream);

                imagePath = "/uploads/opportunities/" + fileName;
            }

            var opportunity = new Opportunity
            {
                Title = model.Title,
                ImagePath = imagePath,
                PublishedBy = model.PublishedBy,
                Type = model.Type.Value,
                Description = model.Description,
                Details = model.Details,
                AvailableCountries = model.AvailableCountries,
                EligibilityCriteria = model.EligibilityCriteria,
                Benefits = model.Benefits,
                ApplyLink = model.ApplyLink
            };

            _context.Opportunities.Add(opportunity);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ================= EDIT =================
        public IActionResult Edit(int id)
        {
            var opp = _context.Opportunities.Find(id);
            if (opp == null) return NotFound();

            var model = new OpportunityEditVM
            {
                Id = opp.Id,
                Title = opp.Title,
                PublishedBy = opp.PublishedBy,
                Type = opp.Type,
                Description = opp.Description,
                Details = opp.Details,
                AvailableCountries = opp.AvailableCountries,
                EligibilityCriteria = opp.EligibilityCriteria,
                Benefits = opp.Benefits,
                ApplyLink = opp.ApplyLink,
                ImagePath = opp.ImagePath // ← مهم لعرض الصورة الحالية
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult Edit(OpportunityEditVM model)
        {
            var old = _context.Opportunities.Find(model.Id);
            if (old == null) return NotFound();

            // تحديث الصورة إذا رفعت جديدة
            if (model.Image != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
                var path = Path.Combine(_env.WebRootPath, "uploads/opportunities", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                model.Image.CopyTo(stream);

                old.ImagePath = "/uploads/opportunities/" + fileName;
            }

            // تحديث باقي الحقول فقط إذا تم تعديلها
            if (!string.IsNullOrEmpty(model.Title)) old.Title = model.Title;
            if (!string.IsNullOrEmpty(model.PublishedBy)) old.PublishedBy = model.PublishedBy;
            if (model.Type.HasValue) old.Type = model.Type.Value;
            if (!string.IsNullOrEmpty(model.Description)) old.Description = model.Description;
            if (!string.IsNullOrEmpty(model.Details)) old.Details = model.Details;
            if (!string.IsNullOrEmpty(model.AvailableCountries)) old.AvailableCountries = model.AvailableCountries;
            if (!string.IsNullOrEmpty(model.EligibilityCriteria)) old.EligibilityCriteria = model.EligibilityCriteria;
            if (!string.IsNullOrEmpty(model.Benefits)) old.Benefits = model.Benefits;
            if (!string.IsNullOrEmpty(model.ApplyLink)) old.ApplyLink = model.ApplyLink;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // ================= DELETE =================
        public IActionResult Delete(int id)
        {
            var opp = _context.Opportunities.Find(id);
            if (opp != null)
            {
                _context.Opportunities.Remove(opp);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // ================= DETAILS =================
        public IActionResult Details(int id)
        {
            var opp = _context.Opportunities.Find(id);
            if (opp == null) return NotFound();

            return View(opp);
        }

        // ================= FILTER BY TYPE =================
        [Route("Opportunities/{type}")]
        public IActionResult ByType(OpportunityType type)
        {
            var data = _context.Opportunities
                .Where(x => x.Type == type)
                .ToList();

            ViewBag.Type = type;
            return View(data);
        }
    }
}
