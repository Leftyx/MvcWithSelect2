using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWithSelect2.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(string id)
        {
            Models.Person viewModel = new Models.Person();

            if (!string.IsNullOrEmpty(id))
            {
                viewModel.Code = 1;
                viewModel.Name = "John";
                viewModel.Country = "UK"; viewModel.CountryDescription = "United Kingdom";
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(Models.Person viewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Errors");
                return (View(viewModel));
            }
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult FetchCountries(string country)
        {
            IList<Models.Country> countries = new List<Models.Country>();

            countries.Add(new Models.Country() { Code = "US", Description = "U.S.A." });
            countries.Add(new Models.Country() { Code = "IT", Description = "Italy" });
            countries.Add(new Models.Country() { Code = "FR", Description = "France" });
            countries.Add(new Models.Country() { Code = "DE", Description = "Germany" });
            countries.Add(new Models.Country() { Code = "UK", Description = "United Kingdom" });

            if (string.IsNullOrEmpty(country))
            {
                countries = countries.Where(f => f.Description.StartsWith(country.ToUpper())).ToList();
            }

            var jSonResult = countries.Select(f => new
            {
                Code = f.Code,
                Description = f.Description,
            }).ToList();

            return (Json(jSonResult, JsonRequestBehavior.DenyGet));
        }

    }
}
