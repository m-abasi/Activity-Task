using ActivityProject.DbContext;
using ActivityProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Activity = ActivityProject.Models.Activity;

namespace ActivityProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var activities = _context.activities.ToList();
            return View(activities);
        }

        public IActionResult Adding()
        {
          
            return View();
        }
        [HttpPost]
        public IActionResult Adding(Activity activity)
        {
            if (_context.activities.Any(a => a.Code == activity.Code))
            {
                ModelState.AddModelError("Code", "این کد قبلاً ثبت شده است.");
                return View(activity);
            }

            if (_context.activities.Any(a => a.Name == activity.Name))
            {
                ModelState.AddModelError("Name", "این نام قبلاً ثبت شده است.");
                return View(activity);
            }

            if (ModelState.IsValid)
            {
                _context.activities.Add(activity);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(activity);
        }

        public IActionResult Edit(int id)
        {
            var activity = _context.activities.FirstOrDefault(a => a.Id == id);
            if (activity == null)
            {
                TempData["ErrorMessage"] = "فعالیت مورد نظر یافت نشد.";
                return RedirectToAction("Index");
            }

            return View(activity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Activity updatedActivity)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedActivity);
            }

            var activity = _context.activities.FirstOrDefault(a => a.Id == updatedActivity.Id);
            if (activity == null)
            {
                return RedirectToAction("Index");
            }

            if (_context.activities.Any(a => a.Code == updatedActivity.Code && a.Id != updatedActivity.Id))
            {
                ModelState.AddModelError("Code", "این کد قبلاً برای فعالیت دیگری ثبت شده است.");
                return View(updatedActivity);
            }

            if (_context.activities.Any(a => a.Name == updatedActivity.Name && a.Id != updatedActivity.Id))
            {
                ModelState.AddModelError("Name", "این نام قبلاً برای فعالیت دیگری ثبت شده است.");
                return View(updatedActivity);
            }

            activity.Name = updatedActivity.Name;
            activity.Code = updatedActivity.Code; 
            activity.IsActive = updatedActivity.IsActive;

            _context.SaveChanges();
            TempData["SuccessMessage"] = $"فعالیت '{updatedActivity.Name}' با موفقیت ویرایش شد.";

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var activity = _context.activities.FirstOrDefault(a => a.Id == id);
            if (activity == null)
            {
                TempData["ErrorMessage"] = "فعالیت مورد نظر یافت نشد.";
                return RedirectToAction("Index");
            }

            return View(activity);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Activity activityFromForm)
        {
            var activity = _context.activities.FirstOrDefault(a => a.Id == activityFromForm.Id);

            if (activity == null)
            {
                TempData["ErrorMessage"] = "فعالیت مورد نظر یافت نشد.";
                return RedirectToAction("Index");
            }

            _context.activities.Remove(activity);
            _context.SaveChanges();

            TempData["SuccessMessage"] = $"فعالیت '{activity.Name}' با موفقیت حذف شد.";

            return RedirectToAction("Index");
        }




    }
}
