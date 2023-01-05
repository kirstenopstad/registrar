using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Registrar.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace Registrar.Controllers
{
  public class StudentsController : Controller
  {
    private readonly RegistrarContext _db;

    public StudentsController(RegistrarContext db)
    {
      _db = db;
    }

    // Routes
    public ActionResult Index()
    {
      List<Student> model = _db.Students.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Name");
      return View();
    }
    [HttpPost]
    public ActionResult Create(Student student)
    {
      // validate
      if (!ModelState.IsValid)
      {
        ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Name");
        return View(student);
      }
      else
      {
        _db.Students.Add(student);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    public ActionResult Details(int id)
    {
      Student thisStudent = _db.Students
        .Include(student => student.JoinCourseStudents)
        .ThenInclude(courseStudent => courseStudent.Course)
        .FirstOrDefault(student => student.StudentId == id);
      return View(thisStudent);
    }

    public ActionResult Edit(int id)
    {
      Student thisStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Name");
      return View(thisStudent);
    }

    [HttpPost]
    public ActionResult Edit(Student student)
    {
      _db.Students.Update(student);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Student thisStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      _db.Students.Remove(thisStudent);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddCourse(int id)
    {
      Student thisStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name");
      return View(thisStudent);
    }

    [HttpPost]
    public ActionResult AddCourse(Student student, int courseId)
    {
      // looking for an instance with the argument set in firstOrdefault() to ensure that there is no instance with those ids
      // 
#nullable enable
      CourseStudent? courseStudentEntity = _db.CourseStudents.FirstOrDefault(courseStudent => (courseStudent.CourseId == courseId && courseStudent.StudentId == student.StudentId));
#nullable disable
      if (courseStudentEntity == null && courseId != 0)
      {
        _db.CourseStudents.Add(new CourseStudent() { CourseId = courseId, StudentId = student.StudentId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = student.StudentId });
    }

    [HttpPost]
    public ActionResult DeleteCourse(int id)
    {
      CourseStudent thisCourseStudent = _db.CourseStudents.FirstOrDefault(entry => entry.CourseStudentId == id);
      _db.CourseStudents.Remove(thisCourseStudent);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = thisCourseStudent.StudentId });
    }
  }
}