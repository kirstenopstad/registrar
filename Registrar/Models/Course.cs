using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Registrar.Models
{
  public class Course
  {
    // properties, constructors, methods, etc. go here
    [Required(ErrorMessage = "Course name is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Course number is required")]
    public string CourseNumber { get; set; }
    public int CourseId { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
    public List<CourseStudent> JoinCourseStudents { get; }
  }
}