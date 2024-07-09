using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyProject.Helpers;
using MyProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MyProject.Data;

namespace MyProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: Login
        public IActionResult Login()
        {
            TempData["ErrorMessage"] = "";
            return View();
        }

        // GET: About
        public IActionResult About()
        {
            return View();
        }

        // GET: Contact
        public IActionResult Contact()
        {
            return View();
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
                return View("Contact");
            }

            TempData["ErrorMessage"] = "There was an error submitting your message. Please try again.";
            return View("Contact");
        }

           
        // POST: LoginForm
        [HttpPost]
        public async Task<IActionResult> LoginForm(string email, string password)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError(string.Empty, "Email and Password are required.");
                    return View("Index");
                }

                var user = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Email == email);

                if (user != null)
                {
                    var passwordEntity = await _context.Passwords
                        .FirstOrDefaultAsync(p => p.EmployeeID == user.EmployeeID);

                    if (passwordEntity != null)
                    {
                        // Log or inspect values for debugging
                        var salt = Convert.FromBase64String(passwordEntity.Salt);
                        var hashedPassword = PasswordHasher.HashPasswordWithSalt(password, salt);

                        var hashedPasswordHex = BitConverter.ToString(hashedPassword).Replace("-", "").ToLower();

                        if (hashedPasswordHex == passwordEntity.PasswordHash)
                        {
                            // Authentication successful
                            HttpContext.Session.SetString("UserEmail", email);
                            HttpContext.Session.SetInt32("UserID", user.EmployeeID);
                            // Redirect to the dashboard or another page
                            TempData["ErrorMessage"] = "";
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    TempData["ErrorMessage"] = "Password";
                } else
                {
                    TempData["ErrorMessage"] = "Email";
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View("Login");
        }
    }
}
