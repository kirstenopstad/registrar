using System.Collections.Generic;

namespace Registrar.Models
{
  public class Course
  {
    // properties, constructors, methods, etc. go here
    public string Name {get;set;}
    public int CourseId {get;set;}
    public List<CourseStudent> JoinCourseStudents {get;}
  }
}
