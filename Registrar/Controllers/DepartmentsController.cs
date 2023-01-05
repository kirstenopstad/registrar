using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Registrar.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Registrar.Controllers
{
  public class DepartmentsController : Controller
  {
    private readonly RegistrarContext _db;

    public DepartmentsController(RegistrarContext db)
    {
      _db = db;
    }

        // Routes
    public ActionResult Index()
    {
      List<Department> model = _db.Departments.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }
    [HttpPost]
    public ActionResult Create(Department department)
    {
      _db.Departments.Add(department);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Department thisDepartment = _db.Departments
        .Include(department => department.Students)
        .Include(department => department.Courses)
        .FirstOrDefault(department => department.DepartmentId == id);
      ViewBag.StudentId = new SelectList(_db.Students, "StudentId", "Name");
      ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name");
      return View(thisDepartment);
    }

    public ActionResult Edit(int id)
    {
      Department thisDepartment = _db.Departments.FirstOrDefault(department => department.DepartmentId == id);
      return View(thisDepartment);
    }
    [HttpPost]
    public ActionResult Edit(Department department)
    {
      _db.Departments.Update(department);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = department.DepartmentId});
    }
    [HttpPost]
    public ActionResult Delete(int id)
    {
      Department thisDepartment = _db.Departments.FirstOrDefault(department => department.DepartmentId == id);
      _db.Departments.Remove(thisDepartment);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}