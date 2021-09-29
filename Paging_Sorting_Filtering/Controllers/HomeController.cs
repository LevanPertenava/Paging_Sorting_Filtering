using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Paging_Sorting_Filtering.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Paging_Sorting_Filtering.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index(string sortBy, string search, int page = 1)
        {
            if (page < 1)
                page = 1;

            var employeeQuery = (from e in _context.Employees select e);

            if (!string.IsNullOrEmpty(search))
            {
                employeeQuery = employeeQuery.Where(s => s.FirstName.Contains(search) || s.LastName.Contains(search) || s.Email.Contains(search));
            }

            int employeesCount = employeeQuery.Count();
            int totalPages = employeesCount % 10 != 0 ? employeesCount / 10 + 1 : employeesCount / 10;

            if (employeesCount == 0)
            {
                return View("NotFound");
            }

            if (page > totalPages)
                return Json("Page was not found");

            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchText = search;

            var employees = MyTools.Paging(employeeQuery, page, pageSize: 10);

            if (!string.IsNullOrEmpty(sortBy))
            {
                var sorted = MyTools.Sorting(employees, sortBy);
                return View(sorted);
            }
            return View(employees);
        }
    }
}

