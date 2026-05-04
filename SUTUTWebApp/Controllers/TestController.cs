using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SUTUTWebApp.Models.Entities;
using System;

namespace SUTUTWebApp.Controllers
{
    public class TestController : Controller
    {
        private readonly MasterContext _context;

        public TestController(MasterContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return Json(_context.Utrkas.Take(5).ToList());
        }
    }
}
