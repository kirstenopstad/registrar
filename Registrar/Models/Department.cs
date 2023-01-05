using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Registrar.Models
{
  public class Department
  {
    [Required(ErrorMessage = "Name must be entered!")]
    public string Name { get; set; }
    public int DepartmentId { get; set; }
    public List<Student> Students { get; set; }
    public List<Course> Courses { get; set; }
  }
}