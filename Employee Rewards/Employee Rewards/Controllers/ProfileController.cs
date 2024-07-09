using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Models;

namespace Employee_Rewards.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProfileController(AppDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
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

        public async Task<IActionResult> Profile()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Login");
            }

            var employee = await _context.Employees
                .Include(e => e.Region)
                .Include(e => e.Department)
                .Include(e => e.Ethnicity)
                .FirstOrDefaultAsync(e => e.Email == userEmail);

            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.Departments = await _context.Departments.ToListAsync();
            ViewBag.Regions = await _context.Regions.ToListAsync();
            ViewBag.Ethnicities = await _context.Ethnicities.ToListAsync();
            ViewBag.Genders = new List<string> { "Male", "Female", "Other" };

            return View(employee);
        }

        public async Task<IActionResult> Index()
        {
            await Prep();
            return await Profile();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile croppedImage, int employeeID)
        {
            var user = await _context.Employees.FindAsync(employeeID);

            if (user != null)
            {
                if (croppedImage != null && croppedImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images/profilepictures");
                    var filePath = Path.Combine(uploadsFolder, user.EmployeeID + ".png");

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await croppedImage.CopyToAsync(fileStream);
                    }

                    user.ProfileImageRoot = "/images/profilepictures/" + user.EmployeeID + ".png";
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, imageUrl = user.ProfileImageRoot });
                }
                return Json(new { success = false, message = "No image uploaded." });
            }
            return Json(new { success = false, message = "Error: User not found." });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(Employee model)
        {
            var user = await _context.Employees.FindAsync(model.EmployeeID);

            if (user != null)
            {
                user.FirstName = model.FirstName.ToLower();
                user.LastName = model.LastName.ToLower();
                user.Email = model.Email.ToLower();
                user.Mobile = model.Mobile;
                user.Address = model.Address.ToLower();
                user.DOB = model.DOB;
                user.EthnicityID = model.EthnicityID;
                user.Gender = model.Gender;
                user.DepartmentID = model.DepartmentID;
                user.RegionID = model.RegionID;
                user.AccountStatus = model.AccountStatus;
                user.Access = model.Access;

                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Profile Updated Successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error updating profile: " + ex.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Error: User not found.";
                return RedirectToAction("Index");
            }
        }

    }
}