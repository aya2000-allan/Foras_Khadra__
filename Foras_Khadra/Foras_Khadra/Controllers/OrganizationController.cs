using CountryData; // مكتبة CountryData
using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Foras_Khadra.Migrations;
using Foras_Khadra.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nager.Country;

namespace Foras_Khadra.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrganizationController(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<SelectListItem> GetCountries()
        {
            var provider = new CountryProvider();
            var countries = provider.GetCountries();

            return countries
                .OrderBy(c => c.CommonName)
                .Select(c => new SelectListItem
                {
                    Value = c.CommonName,
                    Text = c.CommonName
                })
                .ToList();
        }



        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterOrganizationViewModel
            {
                Countries = GetCountries(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterOrganizationViewModel model)
        {
            // إعادة تهيئة القوائم عند إعادة العرض بسبب Validation errors
            model.Countries = GetCountries();

            if (!ModelState.IsValid)
                return View(model);

            var organization = new Foras_Khadra.Models.Organization
            {
                AccountType = model.AccountType,
                Name = model.Name,
                Sector = model.Sector,
                Country = model.Country,
                Location = model.Location,
                ContactName = model.ContactName,
                ContactEmail = model.ContactEmail,
                PhoneNumber = model.PhoneNumber,
                Website = string.IsNullOrEmpty(model.Website) ? null : model.Website,
                Password = model.Password,
                CreatedAt = DateTime.Now
            };

            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();

            return RedirectToAction("RegisterSuccess");
        }

        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View();
        }


    }
}
