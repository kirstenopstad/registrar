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

    // TODO: Fix AddStudent & AddCourse routes -- DbUpdateException: MySqlException: Column 'Name' cannot be null
    
    [HttpPost]
    public ActionResult AddStudent(int studentId, int id)
    // public ActionResult AddStudent(Student student, int id)
    {
      Student thisStudent = _db.Students.FirstOrDefault(student => student.StudentId == studentId);
      _db.Students.Update(thisStudent);
      // _db.Students.Update(student);

      Department thisDepartment = _db.Departments
        .Include(dep => dep.Students)
        .FirstOrDefault(dep => dep.DepartmentId == id);
      _db.Departments.Update(thisDepartment);

      _db.SaveChanges();
      return RedirectToAction("Details", new {id =  thisDepartment.DepartmentId});
    }
    [HttpPost]
    // public ActionResult AddCourse(int courseId, int id)
    public ActionResult AddCourse(Course course, int id)
    {
      // Course thisCourse = _db.Courses.FirstOrDefault(course => course.CourseId == courseId);
      // _db.Courses.Update(thisCourse);
      _db.Courses.Update(course);

      Department thisDepartment = _db.Departments.FirstOrDefault(dep => dep.DepartmentId == id);
      _db.Departments.Update(thisDepartment);

      _db.SaveChanges();
      return RedirectToAction("Details", new {id =  thisDepartment.DepartmentId});
    }
  }
}