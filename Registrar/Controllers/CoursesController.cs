using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Registrar.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Registrar.Controllers
{
  public class CoursesController : Controller
  {
    private readonly RegistrarContext _db;

    public CoursesController(RegistrarContext db)
    {
      _db = db;
    }

    // Routes
    public ActionResult Index()
    {
      List<Course> model = _db.Courses.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Course course)
    {
      // validate
      if (!ModelState.IsValid)
      {
        return View(course);
      }
      else
      {
        _db.Courses.Add(course);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
    public ActionResult Details(int id)
    {
      Course thisCourse = _db.Courses
        .Include(course => course.JoinCourseStudents)
        .ThenInclude(courseStudent => courseStudent.Student)
        .FirstOrDefault(course => course.CourseId == id);
      return View(thisCourse);
    }

    public ActionResult Edit(int id)
    {
      Course thisCourse = _db.Courses.FirstOrDefault(course => course.CourseId == id);
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Name");
      return View(thisCourse);
    }

    [HttpPost]
    public ActionResult Edit(Course course)
    {
      _db.Courses.Update(course);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Course thisCourse = _db.Courses.FirstOrDefault(course => course.CourseId == id);
      _db.Courses.Remove(thisCourse);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddStudent(int id)
    {
      Course thisCourse = _db.Courses.FirstOrDefault(course => course.CourseId == id);
      ViewBag.StudentId = new SelectList(_db.Students, "StudentId", "Name");
      return View(thisCourse);
    }

    [HttpPost]
    public ActionResult AddStudent(Course course, int studentId)
    {
      // looking for an instance with the argument set in firstOrdefault() to ensure that there is no instance with those ids
      // 
#nullable enable
      CourseStudent? courseStudentEntity = _db.CourseStudents.FirstOrDefault(courseStudent => (courseStudent.CourseId == course.CourseId && courseStudent.StudentId == studentId));
#nullable disable
      if (courseStudentEntity == null && studentId != 0)
      {
        _db.CourseStudents.Add(new CourseStudent() { StudentId = studentId, CourseId = course.CourseId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = course.CourseId });
    }
    
    [HttpPost]
    public ActionResult DeleteCourse(int id)
    {
      CourseStudent thisCourseStudent = _db.CourseStudents.FirstOrDefault(entry => entry.CourseStudentId == id);
      _db.CourseStudents.Remove(thisCourseStudent);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = thisCourseStudent.CourseId });
    }

    [HttpPost]
    public ActionResult CourseStatus(int id, string source)
    {
      CourseStudent thisCS = _db.CourseStudents.FirstOrDefault(entry => entry.CourseStudentId == id);
      if (thisCS.Status == false)
      {
        thisCS.Status = true;
      }
      else 
      {
        thisCS.Status = false;
      }
      _db.CourseStudents.Update(thisCS);
      _db.SaveChanges();
      // if from student details, send via students controller
      if (source == "Students")
      {
        return RedirectToAction("Details", "Students", new { id = thisCS.StudentId });
      }
      return RedirectToAction("Details", new { id = thisCS.CourseId });
    }
  }
}