using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using university.Data;
using university.Models;

namespace university.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(){
            return View(await _context.Students.ToListAsync());
            }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
             {
                return NotFound();
                }
            var student = await _context.Students
            .Include(s => s.Enrollments)
                 .ThenInclude(e => e.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
             { return NotFound();
             }
            return View(student);
        }
        [HttpGet]
         public IActionResult Create()
         { ViewData["Title"] = "Create";
            return View();
            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("EnrollmentDate,FirstMidName,LastName")] Student student)
        {    try
             {
                if (ModelState.IsValid)
                {
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("/Index");

                }
            }
            catch (DbUpdateException /* ex */)
             {
        //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
            "Try again, and if the problem persists " +
            "see your system administrator.");
            }
            return View(student);
            }
        [HttpGet]
         public IActionResult Edit()
         { ViewData["Title"] = "Edit";
            return View();
            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id){
            if (id == null){    
                return NotFound();
    }   
            var studentToUpdate = await _context.Students.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Student>(
                 studentToUpdate,
                "",
                s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
    {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("/Index");
                }
                catch (DbUpdateException /* ex */)
        {
            //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
        }
    }
             return View(studentToUpdate);
             }
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
{
    if (id == null)
    {
        return NotFound();
    }

    var student = await _context.Students
        .AsNoTracking()
        .FirstOrDefaultAsync(m => m.ID == id);
    if (student == null)
    {
        return NotFound();
    }

    if (saveChangesError.GetValueOrDefault())
    {
        ViewData["ErrorMessage"] =
            "Delete failed. Try again, and if the problem persists " +
            "see your system administrator.";
    }

    return View(student);
}


}
    }
