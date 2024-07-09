using Employee_Rewards.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyProject.Data;
using MyProject.Models;
using System.Diagnostics;

namespace Employee_Rewards.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        private async Task Prep()
        {
            ViewBag.Email = HttpContext.Session.GetString("UserEmail");
            ViewBag.UserID = HttpContext.Session.GetInt32("UserID");

            var user = await _context.Employees.FindAsync(ViewBag.UserID);

            if (user != null)
            {
                ViewBag.Initials = (user.FirstName[0] + "" + user.LastName[0]).ToUpper();
                ViewBag.Employee = user;
                ViewBag.AccessLevel = user.Access;
            }
        }

        public IActionResult CheckLoggedIn()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return null;
            }
                
        }

        public IActionResult Index()
        {
            CheckLoggedIn();
            Prep();
            return View();
        }

        // GET: About
        public IActionResult About()
        {
            CheckLoggedIn();
            Prep();
            return View();
        }

        // GET: Contact
        public IActionResult Contact()
        {
            CheckLoggedIn();
            Prep();
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        // POST: SubmitContactForm
        [HttpPost]
        public async Task<IActionResult> SubmitContactForm(string name, string email, string message)
        {
            if (ModelState.IsValid)
            {
                // Implement your form submission logic here
                // For example, save the contact info to a database or send an email
                // await _someService.SaveContactFormAsync(name, email, message);

                TempData["SuccessMessage"] = "Thank you for your message! We will get back to you shortly.";
                return RedirectToAction("Contact");
            }

            TempData["ErrorMessage"] = "There was an error submitting your message. Please try again.";
            return RedirectToAction("Contact");
        }
    }
}